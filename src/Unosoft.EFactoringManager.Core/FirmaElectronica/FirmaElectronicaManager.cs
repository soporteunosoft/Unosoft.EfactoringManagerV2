using Abp.Domain.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.Certificados;
using Unosoft.EfactoringManager.Certificados.Dto.FirmaDigital;
using Unosoft.EfactoringManager.Certificados.Enum;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.DetalleDocumentos;
using Unosoft.EFactoringManager.Documentos;
using Unosoft.EFactoringManager.FirmasDetalleDocumentos;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Oficina;
using Unosoft.EFactoringManager.Terceros;

namespace Unosoft.EFactoringManager.FirmaElectronica
{
    public class FirmaElectronicaManager : IDomainService
    {
        private readonly FirmasDetalleDocumentosManager _firmasDetalleDocumentosManager;
        private readonly DocumentosManager _documentosManager; //tipo firma, 
        private readonly DetalleDocumentosManager _detalleDocumentosManager;
        private readonly TercerosManager _tercerosManager;
        private readonly UserManager _userManager;
        private readonly AndesFEConfiguration _andesFEConfiguration;
        private readonly OficinaManager _oficinaManager;


        public FirmaElectronicaManager(DocumentosManager documentosManager,
                                       FirmasDetalleDocumentosManager firmasDetalleDocumentosManager,
                                       DetalleDocumentosManager detalleDocumentosManager,
                                       UserManager userManager,
                                       TercerosManager tercerosManager,
                                       OficinaManager oficinaManager,
                                       IOptions<AndesFEConfiguration> _settings)
        {
            _documentosManager = documentosManager;
            _firmasDetalleDocumentosManager = firmasDetalleDocumentosManager;
            _detalleDocumentosManager = detalleDocumentosManager;
            _userManager = userManager;
            _tercerosManager = tercerosManager;
            _oficinaManager = oficinaManager;
            _andesFEConfiguration = _settings.Value;
        }

        public async Task<StringBuilder> CrearProceso(List<int> detalleDocumentos, List<int> terceros, long idUsuario, bool ordenIndiferente = true)
        {

            try
            {

                User user = await _userManager.GetUserByIdAsync(idUsuario);

                var oficina = await _oficinaManager.Get(1);

                string idUsuarioMod = user.UserName;

                var proxy = new FirmaElectronicaProxy(_andesFEConfiguration.UrlBase, _andesFEConfiguration.UserName, _andesFEConfiguration.Password);

                //Listado con la información del tercero
                var ListaTerceros = await _tercerosManager.ObtenerTerceroPorId(terceros);

                List<Firmante> listaFirmantes = new List<Firmante>();

                int ordenFirma = 1;
                foreach (var idTercero in terceros)
                {
                    listaFirmantes.Add(
                        ListaTerceros.Where(x => x.Id == idTercero)
                        .Select(x => new Firmante()
                        {
                            Contador = ordenIndiferente ? 0 : ordenFirma, // TO DO Orden de firma
                            Documento = x.IdTercero,
                            NumeroTercero = x.Id,
                            Email = x.Email,
                            Indicativo = Convert.ToInt32(x.Ciudades.Pais.IndicativoPais),
                            Nombre = x.RazonSocial,
                            Pais = x.Ciudades.Pais.NomPais,
                            Telefono = x.TelMovil,
                            TipoDocumento = ((int)x.TiposIdentificacion.TipoIdentificacionAndes).ToString()

                        }).FirstOrDefault()
                        );
                    ordenFirma++;
                }


                StringBuilder errorMensaje = new StringBuilder();



                Identity.Terceros terceroPadre = null;
                foreach (var idDetDocumento in detalleDocumentos)
                {
                    try
                    {
                        var detalleDoc = await _detalleDocumentosManager.ObtenerDetalleDocumentoId(idDetDocumento);

                        if (!string.IsNullOrEmpty(detalleDoc.IdFirmaElectronica))
                            throw new Exception($"El documento {detalleDoc.Documentos.TipoDocumento.NomDocumento} cuenta con un proceso de firma electrónica pendiente");

                        if (terceroPadre == null)
                            terceroPadre = await _tercerosManager.ObtenerTerceroPorId(detalleDoc.NumeroTercero);

                        var uploadResponse = await proxy.Upload(detalleDoc.DirDocumento);

                        foreach (var x in detalleDoc.FirmasDetalleDocumentos.ToList())
                        {
                            await _firmasDetalleDocumentosManager.EliminarFirmasDetalleDocumentos(x);
                        }

                        FirmaDigitalRequest request = new FirmaDigitalRequest()
                        {
                            //TO DO Obtener los datos de vencimiento y tipo notificación del tecero padre
                            Firmantes = listaFirmantes,
                            DiasVence = terceroPadre.PlazoFirmar != null ? terceroPadre.PlazoFirmar.Value.ToString() : "30",
                            Documentoid = uploadResponse.Id,
                            NombreCreador = oficina.NomOficina,
                            Notificacion = terceroPadre.CanalNotificacionFirma ?? "1", // "1-vía email, 2-vía SMS, 3-vía WhatsApp o 4- vía SMA y WhatsApp",
                            Modificar = "false",
                            Callback = _andesFEConfiguration.Callback
                        };

                        FEResponseDTO createResponse = await proxy.Create((TipoFirmaElectronica)detalleDoc.Documentos.TipoFirmaElectronica, request);


                        // Actualizar Detalle Documento  
                        detalleDoc.IdFirmaElectronica = createResponse.Id;
                        detalleDoc.FecModifica = DateTime.Now;
                        detalleDoc.Firma = false;

                        await _detalleDocumentosManager.Actualizar(detalleDoc);

                        foreach (var x in listaFirmantes)
                        {
                            await _firmasDetalleDocumentosManager.InsertarFirmasDetalleDocumentos(
                                new Identity.FirmasDetalleDocumentos()
                                {
                                    OrdenFirma = x.Contador,
                                    Firma = false,
                                    HoraModifica = DateTime.Now.ToString("HH:mm:ss"),
                                    FecModifica = DateTime.Now,
                                    IdDetDoc = detalleDoc.Id,
                                    NumeroTercero = detalleDoc.NumeroTercero,
                                    NumTerceroFirma = x.NumeroTercero,
                                    IdUsrMod = user.UserName
                                });
                        }


                    }
                    catch (Exception ex)
                    {
                        errorMensaje.Append(ex.Message);
                    }
                }

                return errorMensaje;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear proceso FE : " + ex.ToString());
            }
        }


        public async Task Consultar(int detalleDocumentoId)
        {
            try
            {

                var detalleDocumento = await _detalleDocumentosManager.ObtenerDetalleDocumentoId(detalleDocumentoId);

                if (detalleDocumento == null || string.IsNullOrEmpty(detalleDocumento.IdFirmaElectronica))
                    return;

                var proxy = new FirmaElectronicaProxy(_andesFEConfiguration.UrlBase, _andesFEConfiguration.UserName, _andesFEConfiguration.Password);

                var response = await proxy.Consultar(detalleDocumento.IdFirmaElectronica);

                if (response.Estado == true)
                {
                    await Notificar(response.Id, response.Estado ? "True" : "False", response.Url);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Notificar(string id, string estado, string url)
        {

            if (string.IsNullOrEmpty(id))
                return;


            var detalleDocumento = await _detalleDocumentosManager.ObtenerDetalleDocumentoByIdFirmaElectronica(id);

            if (detalleDocumento == null || string.IsNullOrEmpty(detalleDocumento.DirDocumento))
                return;

            detalleDocumento.Firma = estado.ToLower() == "true";
            if (detalleDocumento.Firma == true)
            {
                var oficina = await _oficinaManager.Get(1);

                var terceroPadre = await _tercerosManager.ObtenerTerceroPorId(detalleDocumento.NumeroTercero);
                var fileName = $"{Path.GetFileNameWithoutExtension(detalleDocumento.DirDocumento)}_FE.pdf";
                string rutaFichero = $"{oficina.DirRaizDocumentos}\\DOCUMENTOS_CLIENTES_PLATAFORMA\\{terceroPadre.NomTercero.Replace(" ", "_")}\\Documentos_Juridicos";


                if (!Directory.Exists(rutaFichero))
                {
                    Directory.CreateDirectory(rutaFichero);
                }

                using var httpClient = new HttpClient();
                var fileBytes = httpClient.GetByteArrayAsync(url).GetAwaiter().GetResult();
                File.WriteAllBytes($"{rutaFichero}\\{fileName}", fileBytes);
                detalleDocumento.DirDocumentoFirmado = $"{rutaFichero}\\{fileName}";
            }
            else  //EL documento ha sido anulado
            {
                detalleDocumento.Firma = null;
                detalleDocumento.IdFirmaElectronica = null;
                foreach (var x in detalleDocumento.FirmasDetalleDocumentos.ToList())
                {
                    await _firmasDetalleDocumentosManager.EliminarFirmasDetalleDocumentos(x);
                }

            }

            await _detalleDocumentosManager.Actualizar(detalleDocumento);

        }

        public async Task Anular(int detalleDocumentoId)
        {

            var detalleDocumento = await _detalleDocumentosManager.ObtenerDetalleDocumentoId(detalleDocumentoId);

            if (detalleDocumento == null || string.IsNullOrEmpty(detalleDocumento.IdFirmaElectronica))
                return;

            var proxy = new FirmaElectronicaProxy(_andesFEConfiguration.UrlBase, _andesFEConfiguration.UserName, _andesFEConfiguration.Password);

            if (await proxy.Anular(detalleDocumento.IdFirmaElectronica))
            {

                detalleDocumento.Firma = null;
                detalleDocumento.IdFirmaElectronica = null;


                await _detalleDocumentosManager.Actualizar(detalleDocumento);
            }

        }

    }
}
