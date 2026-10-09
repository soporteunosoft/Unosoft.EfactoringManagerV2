using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.DetalleSolicitudes;
using Unosoft.EFactoringManager.Dispapeles;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Oficina;
using Unosoft.EFactoringManager.SolicitudesOperaciones;
using Unosoft.EFactoringManager.TiposEventosRadian;
using UnoSoft.EFactoringManager.Services;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.Titulos
{
    public class TitulosManager : IDomainService
    {
        #region Atributos
        private readonly ITituloRepositorio _tituloRepositorio;
        private readonly ITerceroRepositorio _terceroRepositorio;
        private readonly IOficinasRepositorio _oficinaRepositorio;
        private readonly IDetalleSolicitudesRepositorio _detalleSolicitudesRepositorio;

        private readonly Terceros.TercerosManager _tercerosManager;
        private readonly DispapelesManager _dispapelesManager;
        private readonly OficinaManager _oficinaManager;
        private readonly TiposEventosRadianManager _tiposEventosRadian;
        private readonly DetalleSolicitudesManager _detalleSolicitudesManager;
        private readonly Ciudades.CiudadesManager _ciudadesManager;
        private readonly AppSettings _appSettings;

        public decimal PorcNegociacion { get; private set; }

        //private readonly SolicitudesOperacionesManager _solicitudesOperacionesManager;
        #endregion

        #region Constructor de la clase
        public TitulosManager(
                ITituloRepositorio tituloRepositorio,
                ITerceroRepositorio terceroRepositorio,
                Terceros.TercerosManager tercerosManager,
                IOficinasRepositorio oficinaRepositorio,
                IDetalleSolicitudesRepositorio detalleSolicitudesRepositorio,
                DispapelesManager dispapelesManager,
                OficinaManager oficinaManager,
                TiposEventosRadianManager tiposEventosRadian,
                DetalleSolicitudesManager detalleSolicitudesManager,
                Ciudades.CiudadesManager ciudadesManager,
                IOptions<AppSettings> appSettings
            )
        {
            _tituloRepositorio = tituloRepositorio;
            _terceroRepositorio = terceroRepositorio;
            _tercerosManager = tercerosManager;
            _detalleSolicitudesRepositorio = detalleSolicitudesRepositorio;
            _oficinaRepositorio = oficinaRepositorio;
            _dispapelesManager = dispapelesManager;
            _oficinaManager = oficinaManager;
            _tiposEventosRadian = tiposEventosRadian;
            _detalleSolicitudesManager = detalleSolicitudesManager;
            _ciudadesManager = ciudadesManager;
            _appSettings = appSettings.Value;

        }
        #endregion

        public async Task<Tuple<bool, string>> RegistrarFactura(int numeroTercero, string tipoDocumento, int idOficina, int idSolicitud, decimal porcNegociacion, DateTime fecSolicitud, int? tipoEndoso, List<TitulosFileInfo> files)
        {
            var validInoviceExtension = new[] { ".xml", ".xls", ".xlsx" };

            var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefaultAsync();

            var tercero = _terceroRepositorio.GetbyId(numeroTercero);

            if (tercero == null)
                return new Tuple<bool, string>(false, "OK");

            var fileInvoice = files.FirstOrDefault(f => validInoviceExtension.Contains(f.Extension));

            if (fileInvoice == null)
                return new Tuple<bool, string>(false, "OK");

            //var solicitud = null  //_solicitudesOperacionesManager.ObtenerSolicitudOpePorId(tipoDocumento, idOficina, idSolicitud);
            //WIGELO:19/01/2022 REVISAR COMO PASAR LA DATA DE PORC DE NEGOCIACION
            var titulo = await GetTituloFromXml(tercero.Id, oficina, porcNegociacion, fecSolicitud, tipoEndoso, tercero, fileInvoice.Content,
                (fileInvoice.Extension == ".xls" || fileInvoice.Extension == ".xlsx"), tipoDocumento);



            var filePdf = files.FirstOrDefault(f => f.Extension == ".pdf");

            if (filePdf != null)
                SubirRepresentacionTitulos(oficina, titulo, tercero, filePdf.Content, filePdf.FileName);

            if (ValidarTitulo(titulo))
            {
                try
                {
                    await _tituloRepositorio.InsertAsync(titulo);
                }
                catch (Exception)
                {
                    return new Tuple<bool, string>(false, "Error al insertar la factura, valide si la factura no se encuentra ya en el sistema");
                }
                await GuardarFacturas(tercero.Id, fileInvoice.Content, titulo.IdTitulo);

                //FOR DO WIGELO:03/01/2022 REVISAR SI NO PASA UNA LISTA
                await _detalleSolicitudesManager.Insertar(tipoDocumento, idOficina, idSolicitud, new List<Identity.Titulos>() { titulo }, tercero);
                return new Tuple<bool, string>(true, "OK");
            }
            else { return new Tuple<bool, string>(false, "OK"); }
        }

        public async Task<Tuple<bool, string>> RegistrarFactura(int numeroTercero, byte[] xml)
        {
            try
            {
                DateTime fechaFinalTitulo;
                DocumentDataExtractor documentDataExtractor = new DocumentDataExtractor();

                //int EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoAplica;
                int EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gCargado;

                var doc = documentDataExtractor.Extract(xml);
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                if (doc.DescriptionDocument != "")
                {
                    doc = documentDataExtractor.Extract(encoding.GetBytes(doc.DescriptionDocument));
                }

                var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefaultAsync();

                var tercero = _terceroRepositorio.GetbyId(numeroTercero);

                #region [Se realiza la consulta de la factura en Dispapeles]
                if (doc.DocumentData?.Cufe != null)
                {
                    var facturaDian = await _dispapelesManager.ConsultarFactura(doc.DocumentData?.Cufe);

                    var eventosRadian = await _tiposEventosRadian.ObtenerTiposEventosRadian();

                    if (eventosRadian.Any(e => e.Rechazo && facturaDian.Eventos.Select(x => x.CodigoEvento).ToList().Contains(e.TipoEvento)))
                    {
                        EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoAplica;
                    }
                    // comparamos eventos obligatorios con eventos de la factura
                    else if (eventosRadian.Count(e => e.Obligatorio) - 1 > eventosRadian.Count(e => e.Obligatorio && facturaDian.Eventos.Select(x => x.CodigoEvento).ToList().Contains(e.TipoEvento)))
                    {
                        EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoAplica;
                    }
                    else
                    {
                        EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gCargado;
                    }
                }
                #endregion

                #region Registro Info del tercero
                var terceroPagador = await _tercerosManager.ObtenerTerceroPorId(doc.ReceiverIdentification);

                if (terceroPagador == null)
                {
                    terceroPagador = new Identity.Terceros();
                    terceroPagador.IdTercero = doc.ReceiverIdentification;
                    terceroPagador.DigitoVerificacion = Convert.ToInt16(doc.DocumentData.ReceiverIdentificationType);
                    terceroPagador.RazonSocial = doc.DocumentData.ReceiverName;
                    terceroPagador.NomTercero = doc.DocumentData.ReceiverName;
                    terceroPagador.ApeTercero = "";
                    //Nuevo Luis
                    terceroPagador.Email = doc.DocumentData.ReceiverEmail;
                    terceroPagador.DirOficina = doc.DocumentData.ReceiverDirOficina;
                    terceroPagador.TelDomicilio = doc.DocumentData.ReceiverTelDomicilio;
                    //AJUSTAR DATA QUEMADA
                    terceroPagador.TipoIdentificacion = 2;
                    terceroPagador.TipoContribuyente = 2;
                    terceroPagador.IdOficina = 1;
                    terceroPagador.IdCiudad = 169;
                    terceroPagador.FecApertura = DateTime.Now;
                    terceroPagador.Cliente = false;
                    terceroPagador.Pagador = true;
                    terceroPagador.Inversionista = false;
                    terceroPagador.Empleado = false;
                    terceroPagador.FecModifica = DateTime.Now;
                    terceroPagador.IdUsrMod = numeroTercero.ToString();
                    var idPagador = await _tercerosManager.Insertar(terceroPagador);
                }
                #endregion

                if (Convert.ToDateTime(doc.DueDate) > DateTime.Now)
                {
                    fechaFinalTitulo = doc.DueDate;
                }
                else
                {
                    fechaFinalTitulo = DateTime.Now.AddDays(30);
                }

                if (doc.SenderIdentification == tercero.TerceroPadre.IdTercero)
                {
                    Identity.Titulos titulo = new Identity.Titulos()
                    {
                        //IdCliente = doc.SenderIdentification,
                        IdTitulo = doc.DocumentId,
                        TipoTitulo = (int)EFactoringManagerConsts.TiposTitulos.gFacturaElectronica,
                        NumeroTercero = tercero.IdTerceroPadre.Value,
                        //TipoTercero = (int)EFactoringManagerConsts.TiposTitulos.gFacturaElectronica,
                        NumeroPagador = terceroPagador.Id,
                        TipoMoneda = doc.CurrencyType,
                        VrBrutoTitulo = Math.Round(doc.DocumentData.TotalValue - doc.DocumentData.ValTax.Amount),
                        VrNetoTitulo = Math.Round(doc.DocumentData.TotalValue - doc.DocumentData.ValTaxReteFuente.Amount - doc.DocumentData.ValTaxReteIVA.Amount - doc.DocumentData.ValTaxReteICA.Amount),
                        PorcIvaTitulo = doc.DocumentData.ValTax.Percent,
                        VrIvaTitulo = Math.Round(doc.DocumentData.ValTax.Amount),
                        PorcReteFuenteTitulo = doc.DocumentData.ValTaxReteFuente.Percent,
                        VrReteFuenteTitulo = Math.Round(doc.DocumentData.ValTaxReteFuente.Amount),
                        PorcReteIvaTitulo = doc.DocumentData.ValTaxReteIVA.Percent,
                        VrReteIvaTitulo = Math.Round(doc.DocumentData.ValTaxReteIVA.Amount),
                        PorcReteIcaTitulo = doc.DocumentData.ValTaxReteICA.Percent,
                        VrReteIcaTitulo = Math.Round(doc.DocumentData.ValTaxReteICA.Amount),
                        VrDescuentoTitulo = 0,
                        VrAnticipoTitulo = 0,
                        TasaInversionista = 0,
                        PorcGravBancario = 0,
                        VrGravBancario = 0,
                        PorcNegociacion = tercero.PorcNegociacion,
                        FecRecepcion = DateTime.Now,
                        HoraRecepcion = DateTime.Now.ToString("HH:mm:ss"),
                        FecInicial = DateTime.Now,
                        FecEmision = Convert.ToDateTime(doc.IssueDate),
                        FecProbable = null,
                        FecFinal = fechaFinalTitulo,
                        // FecFinal = Convert.ToDateTime(doc.DueDate),
                        Dias = (Convert.ToDateTime(fechaFinalTitulo) - DateTime.Now).Days,
                        CufeTitulo = doc.DocumentData.Cufe,
                        FecCufeTitulo = Convert.ToDateTime(doc.IssueDate),

                        //Logica para validar el estado con variable

                        //EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gCargado,
                        EstadoTitulo = EstadoFactura,

                        FecModifica = DateTime.Now,
                        IdUsrMod = numeroTercero.ToString(),
                        HoraModifica = DateTime.Now.ToString("HH:mm:ss"),
                        VrSubTotalTitulo = Math.Round(doc.DocumentData.ExtensionValue - 0 - 0) + doc.DocumentData.ValTax.Amount,

                        VrNegociacion = Decimal.Round(doc.DocumentData.TotalValue * ((decimal)tercero.PorcNegociacion / 100), 4),

                        VrPresente = doc.DocumentData.TotalValue * Decimal.Round(((decimal)tercero.PorcNegociacion / 100), 4),

                        VrReserva = doc.DocumentData.TotalValue - (doc.DocumentData.TotalValue * Decimal.Round(((decimal)tercero.PorcNegociacion / 100), 4))
                    };
                    if (ValidarTitulo(titulo))
                    {
                        await _tituloRepositorio.InsertAsync(titulo);
                        await GuardarFacturas(numeroTercero, xml, doc.DocumentId);
                        return new Tuple<bool, string>(true, "OK");
                    }
                    else { return new Tuple<bool, string>(false, "Error al validar el título"); }
                }
                else { return new Tuple<bool, string>(false, "Error al procesar el documento Identificador del sender no coincide con tercero asociado"); }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GuardarFacturas(int numeroTercero, byte[] xml, string IdFactura)
        {
            var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefaultAsync();
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);

            var rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ", "_") + "\\Facturas";

            Identity.DetalleDocumentos detalleDocumentos = new Identity.DetalleDocumentos();
            IdFactura = string.Format(IdFactura + "_{0:ddMMyyyyHHmm}.xml", DateTime.Now);
            if (!(Directory.Exists(rutaFichero)))
            {
                Directory.CreateDirectory(rutaFichero);
            }
            if (Directory.Exists(rutaFichero))
            {
                System.IO.File.WriteAllBytes(rutaFichero + "\\" + IdFactura, xml);
                return true;
            }
            else return false;
        }

        private async Task<Identity.Titulos> GetTituloFromXml(int numeroTercero, Identity.Oficinas oficina, decimal porcNegociacion, DateTime fecSolicitud, int? tipoEndoso, Identity.Terceros terceroPadre, byte[] document, bool esxls, string tipoDocumento)
        {
            try
            {
                DateTime fechaFinalTitulo;
                //DateTime FecVencimiento;
                DocumentDataExtractor documentDataExtractor = new DocumentDataExtractor();
                int EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gCargado;
                //int EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoAplica;
                DocumentHeader doc;

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

                if (!esxls)
                {
                    doc = documentDataExtractor.Extract(document);
                    if (!string.IsNullOrEmpty(doc.DescriptionDocument) && doc.DescriptionDocument.StartsWith("<"))
                        doc = documentDataExtractor.Extract(encoding.GetBytes(doc.DescriptionDocument));
                }
                else
                {
                    doc = documentDataExtractor.ExtractFromXls(document);
                }

                #region Registro Info del tercero
                var terceroPagador = await _tercerosManager.ObtenerTerceroPorId(doc.ReceiverIdentification);

                var ciudadPagador = await _ciudadesManager.ObntenerCiudadPorCodDane(doc.DocumentData.ReceiverCiudad);

                bool NoCrearPagador = _appSettings.NoCrearPagador;

                if (terceroPagador == null && !NoCrearPagador)
                {
                    terceroPagador = new Identity.Terceros()
                    {
                        //FOR DO:SE DEBE REVISAR LA INSERCIÓN DEL PAGADOR
                        IdTercero = doc.ReceiverIdentification,
                        DigitoVerificacion = doc.DocumentData.ReceiverIdentificationType != "" ?  Convert.ToInt16(doc.DocumentData.ReceiverIdentificationType) : null,
                        RazonSocial = doc.DocumentData.ReceiverName,
                        NomTercero = doc.DocumentData.ReceiverName,
                        ApeTercero = "",
                        //Nuevo Luis
                        Email = doc.DocumentData.ReceiverEmail,
                        DirOficina = doc.DocumentData.ReceiverDirOficina,
                        TelDomicilio = doc.DocumentData.ReceiverTelDomicilio,
                        IdCiudad = ciudadPagador?.IdCiudad,
                        //WIGELO:19/01/2022 FOR DO:AJUSTAR DATA QUEMADA
                        TipoIdentificacion = 2,
                        TipoContribuyente = 2,
                        IdOficina = 1,
                        FecApertura = DateTime.Now,
                        Cliente = false,
                        Pagador = true,
                        Inversionista = false,
                        Empleado = false,
                        FecModifica = DateTime.Now,
                        IdUsrMod = numeroTercero.ToString(),
                    };

                    try
                    {
                        var idPagador = await _tercerosManager.Insertar(terceroPagador);
                        terceroPagador.Id = idPagador;
                    }
                    catch (Exception)
                    {
                        //POR FAVOR REGISTRE EL PAGADOR EN OFICINA VIRTUAL
                    }
                }
                #endregion

                //FecVencimiento = Convert.ToDateTime(doc.DueDate);
                fechaFinalTitulo = Convert.ToDateTime(doc.DueDate); // > DateTime.Now ? doc.DueDate : DateTime.Now.AddDays(30);
                //fechaFinalTitulo = FecVencimiento;

                if (doc.SenderIdentification == terceroPadre.IdTercero)
                {
                    string MensajePr = null;
                    string CodigoErr = null;

                    if (fechaFinalTitulo < DateTime.Now && tipoEndoso != (int)EFactoringManagerConsts.TipoEndoso.CesionOrdinaria && tipoDocumento == EFactoringManagerConsts.gSolicitudCufe)
                    {
                        EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable;
                        CodigoErr = "Factura Vencida";
                        MensajePr = "La Factura esta vencida y por ende solo se puede hacer ENDOSO EN CESIÓN ORDINARIA";
                    }

                    Identity.Titulos titulo = new Identity.Titulos()
                    {

                        IdTitulo = doc.DocumentId,
                        TipoTitulo = (int)EFactoringManagerConsts.TiposTitulos.gFacturaElectronica,
                        NumeroTercero = terceroPadre.Id,
                        //TipoTercero = (int)EFactoringManagerConsts.TiposTitulos.gFacturaElectronica,
                        NumeroPagador = terceroPagador.Id,
                        TipoMoneda = doc.CurrencyType,
                        VrBrutoTitulo = Math.Round(doc.DocumentData.TotalValue - doc.DocumentData.ValTax.Amount),

                        VrNetoTitulo = Math.Round(doc.DocumentData.TotalValue - doc.DocumentData.ValTaxReteFuente.Amount - doc.DocumentData.ValTaxReteIVA.Amount - doc.DocumentData.ValTaxReteICA.Amount),

                        PorcIvaTitulo = doc.DocumentData.ValTax.Percent,
                        VrIvaTitulo = Math.Round(doc.DocumentData.ValTax.Amount),
                        PorcReteFuenteTitulo = doc.DocumentData.ValTaxReteFuente.Percent,
                        VrReteFuenteTitulo = Math.Round(doc.DocumentData.ValTaxReteFuente.Amount),
                        PorcReteIvaTitulo = doc.DocumentData.ValTaxReteIVA.Percent,
                        VrReteIvaTitulo = Math.Round(doc.DocumentData.ValTaxReteIVA.Amount),
                        PorcReteIcaTitulo = doc.DocumentData.ValTaxReteICA.Percent,
                        VrReteIcaTitulo = Math.Round(doc.DocumentData.ValTaxReteICA.Amount),
                        VrDescuentoTitulo = 0,
                        VrAnticipoTitulo = 0,
                        TasaInversionista = 0,
                        PorcGravBancario = 0,
                        VrGravBancario = 0,
                        //PorcNegociacion = terceroPadre.PorcNegociacion,
                        PorcNegociacion = (float)porcNegociacion,
                        FecRecepcion = DateTime.Now,
                        HoraRecepcion = DateTime.Now.ToString("HH:mm:ss"),
                        FecInicial = fecSolicitud,
                        FecEmision = Convert.ToDateTime(doc.IssueDate),
                        //WIGELO:25/04/2023 --REVISAR SE COMENTAREA FECPROBABLE
                        FecProbable = fechaFinalTitulo,
                        //FecProbable = null,
                        //FecFinal = Convert.ToDateTime(doc.DueDate),
                        FecFinal = fechaFinalTitulo,
                        Dias = (Convert.ToDateTime(fechaFinalTitulo) - fecSolicitud).Days,
                        CufeTitulo = doc.DocumentData.Cufe,
                        FecCufeTitulo = Convert.ToDateTime(doc.IssueDate),

                        //FOR DO: DEJAR Logica para validar el estado con DISPAPELES con variable
                        EstadoTitulo = EstadoFactura,

                        CodigoError = CodigoErr,
                        MensajeProcesamiento = MensajePr,

                        //EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gCargado,


                        FecModifica = DateTime.Now,
                        IdUsrMod = numeroTercero.ToString(),
                        HoraModifica = DateTime.Now.ToString("HH:mm:ss"),

                        VrSubTotalTitulo = Math.Round(doc.DocumentData.ExtensionValue - 0 - 0) + doc.DocumentData.ValTax.Amount,


                        //VrNegociacion = Decimal.Round(doc.DocumentData.TotalValue * ((decimal)porcNegociacion / 100), 0),
                        VrNegociacion = Math.Round((doc.DocumentData.TotalValue - doc.DocumentData.ValTaxReteFuente.Amount - doc.DocumentData.ValTaxReteIVA.Amount - doc.DocumentData.ValTaxReteICA.Amount) * ((decimal)porcNegociacion / 100), 0),



                        //VrPresente = doc.DocumentData.TotalValue * Decimal.Round(((decimal)porcNegociacion / 100), 4),

                        //VrReserva = doc.DocumentData.TotalValue - (doc.DocumentData.TotalValue * Decimal.Round(((decimal)porcNegociacion / 100), 4))
                        //VrReserva = Decimal.Round(doc.DocumentData.TotalValue - (doc.DocumentData.TotalValue * ((decimal)porcNegociacion / 100)), 0),
                        VrReserva = Decimal.Round((doc.DocumentData.TotalValue - doc.DocumentData.ValTaxReteFuente.Amount - doc.DocumentData.ValTaxReteIVA.Amount - doc.DocumentData.ValTaxReteICA.Amount) - ((doc.DocumentData.TotalValue - doc.DocumentData.ValTaxReteFuente.Amount - doc.DocumentData.ValTaxReteIVA.Amount - doc.DocumentData.ValTaxReteICA.Amount) * ((decimal)porcNegociacion / 100)), 0),

                    };
                    return titulo;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Identity.Titulos>> ObtenerTitulosPorTercero(int numeroTercero)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);

            if (tercero == null)
                return null;

            return await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.IdTerceroPadre)
                                              .OrderByDescending(a => a.FecInicial)
                                              .Include(x => x.Pagador)
                                              .Include(X => X.Tercero)
                                              .Include(x => x.EstadosTitulos).ToListAsync();
        }

        public async Task<List<Identity.Titulos>> ObtenerTitulos()
        {
            return await _tituloRepositorio.GetAll()
                                     .OrderByDescending(a => a.FecInicial)
                                     .Include(x => x.Pagador)
                                     .Include(x => x.Tercero)
                                     .Include(x => x.EstadosTitulos).ToListAsync();
        }

        public async Task<List<Identity.Titulos>> ObtTitulosPorFiltros(string idPagador,
                                                            string FecRecepcionInicial,
                                                            string FecRecepcionFinal,
                                                            string FecFinalInicial,
                                                            string FecFinalFinal,
                                                            decimal VrNetoTituloInicial,
                                                            decimal VrNetoTituloFinal,
                                                            int? TipoTitulo, int numeroTercero)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);

            if (tercero == null)
                return null;

            return await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.IdTerceroPadre.Value &&
                                                    (idPagador == "" || a.Pagador.IdTercero == idPagador) &&
                                                    (FecRecepcionInicial == "" || a.FecInicial >= Convert.ToDateTime(FecRecepcionInicial)) &&
                                                    (FecRecepcionFinal == "" || a.FecInicial <= Convert.ToDateTime(FecRecepcionFinal)) &&
                                                    (FecFinalInicial == "" || a.FecFinal >= Convert.ToDateTime(FecFinalInicial)) &&
                                                    (FecFinalFinal == "" || a.FecFinal <= Convert.ToDateTime(FecFinalFinal)) &&
                                                    (VrNetoTituloInicial == 0 || a.VrNetoTitulo >= VrNetoTituloInicial) &&
                                                    (VrNetoTituloFinal == 0 || a.VrNetoTitulo <= VrNetoTituloFinal) &&
                                                    (TipoTitulo == null || a.TipoTitulo == TipoTitulo)).Include(x => x.Pagador).Include(x => x.EstadosTitulos).Include(x => x.Tercero).ToListAsync();
        }

        public async Task<List<Identity.Titulos>> ObtTitulosPorFiltrosAdm(string idPagador,
                                                              string FecRecepcionInicial,
                                                              string FecRecepcionFinal,
                                                              string FecFinalInicial,
                                                              string FecFinalFinal,
                                                              decimal VrNetoTituloInicial,
                                                              decimal VrNetoTituloFinal,
                                                              int? TipoTitulo,
                                                              int numeroTercero = 0)
        {

            return await _tituloRepositorio.GetAll().Where(a => (a.NumeroTercero == numeroTercero || numeroTercero == 0) &&
                                                    (idPagador == "" || a.Pagador.IdTercero == idPagador) &&
                                                    (FecRecepcionInicial == "" || a.FecInicial >= Convert.ToDateTime(FecRecepcionInicial)) &&
                                                    (FecRecepcionFinal == "" || a.FecInicial <= Convert.ToDateTime(FecRecepcionFinal)) &&
                                                    (FecFinalInicial == "" || a.FecFinal >= Convert.ToDateTime(FecFinalInicial)) &&
                                                    (FecFinalFinal == "" || a.FecFinal <= Convert.ToDateTime(FecFinalFinal)) &&
                                                    (VrNetoTituloInicial == 0 || a.VrNetoTitulo >= VrNetoTituloInicial) &&
                                                    (VrNetoTituloFinal == 0 || a.VrNetoTitulo <= VrNetoTituloFinal) &&
                                                    (TipoTitulo == null || a.TipoTitulo == TipoTitulo)).Include(x => x.Pagador).Include(x => x.EstadosTitulos).Include(x => x.Tercero).ToListAsync();
        }

        public async Task<Identity.Titulos> ObtenerTituloPorId(int numeroTercero, string idTitulo, int tipoTitulo)
        {
            return await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == numeroTercero
                                                       && a.IdTitulo == idTitulo
                                                       && a.TipoTitulo == tipoTitulo)
                                            .Include(x => x.EstadosTitulos)
                                            .Include(x => x.Tercero)
                                            .Include(x => x.Pagador)
                                            .FirstOrDefaultAsync();
        }


        public async Task<Identity.Titulos> ObtenerTituloPorCufe(string CUFE)
        {
            return await _tituloRepositorio.GetAll().Where(a => a.CufeTitulo == CUFE)
                                            .FirstOrDefaultAsync();
        }

        public async Task<List<Identity.DetalleSolicitudes>> ObtTitulosEndosado(int numeroTercero, int idOficina, string tipoDocumento, int producto)
        {
            var oficinas = await _oficinaManager.ObtOficinas(idOficina);
            var oficina = oficinas.FirstOrDefault();
            List<Identity.DetalleSolicitudes> result = null;

            if (tipoDocumento == EFactoringManagerConsts.gSolicitudSol)
            {
                var usados = await _tituloRepositorio.GetAll()
                             .Where(t =>
                                 ((producto == 1 && t.NumeroTercero == numeroTercero) ||
                                  (producto == 2 && t.NumeroPagador == numeroTercero)) &&
                                 t.DetalleSolicitudes.TipoDocumento == EFactoringManagerConsts.gSolicitudSol)
                             .Select(x => x.IdTitulo)
                             .ToListAsync();

                var query = _detalleSolicitudesRepositorio
                    .GetAll()
                    .Where(a =>
                        ((producto == 1 && a.NumeroTercero == numeroTercero) ||
                         (producto == 2 && a.NumeroPagador == numeroTercero)) &&
                        a.Titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gEndosada &&
                        (
                            (a.Titulo.EndosadoA == "" && a.IdOficina == idOficina) ||
                            (a.Titulo.EndosadoA != "" &&
                             a.Titulo.DetalleSolicitudes.SolicitudOperacion.Inversionista.IdTercero == oficina.NitOficina) ||
                            (a.Titulo.EndosadoA != "" &&
                             a.SolicitudOperacion.NumeroInversionista != a.SolicitudOperacion.NumeroTercero) ||
                            (idOficina == _appSettings.OficinaTodos)
                        ))
                    .Include(p => p.Pagador)
                    .Include(e => e.Tercero);

                // Traer a memoria, aplicar exclusión por IdTitulo y agrupar en memoria
                var listaFiltrada = query
                    .AsEnumerable() // forzar evaluación en cliente a partir de aquí
                    .Where(a => !usados.Contains(a.IdTitulo))
                    .GroupBy(x => x.IdTitulo)
                    .Select(g => g
                        .OrderByDescending(x => x.FecInicial)
                        .FirstOrDefault())
                    .ToList();

                result = listaFiltrada;
            }
            else
            {

                result = _detalleSolicitudesRepositorio.GetAll()
                        .Where(t => (producto == 1 && t.NumeroTercero == numeroTercero)
                                 || (producto == 2 && t.NumeroPagador == numeroTercero))
                        .Include(p => p.Pagador)
                        .Include(e => e.Tercero)
                        .GroupBy(x => x.IdTitulo)
                        .Select(g => g
                            .OrderByDescending(x => x.FecInicial)
                            .FirstOrDefault())
                        .ToList();
            }

            return result;
            //List<Identity.Titulos> result = null;

            //if (tipoDocumento == EFactoringManagerConsts.gSolicitudSol) {
            //    var usados = _tituloRepositorio.GetAll()
            //                .Where(t => ((producto == 1 && t.NumeroTercero == numeroTercero) || (producto == 2 && t.NumeroPagador == numeroTercero)) && t.DetalleSolicitudes.TipoDocumento == EFactoringManagerConsts.gSolicitudSol)
            //                .Select(x => x.IdTitulo)
            //                .ToList();

            //    result = _tituloRepositorio
            //                .GetAll()
            //                .Where(a => ((producto == 1 && a.NumeroTercero == numeroTercero) || (producto == 2 && a.NumeroPagador == numeroTercero)) &&
            //                            a.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gEndosada &&
            //                            (
            //                                (a.EndosadoA == "" && a.DetalleSolicitudes.IdOficina == idOficina) ||
            //                                (a.EndosadoA != "" && a.DetalleSolicitudes.SolicitudOperacion.NumeroInversionista != a.DetalleSolicitudes.SolicitudOperacion.NumeroTercero/*== oficina.NitOficina*/)
            //                            )
            //                            //&& a.DetalleSolicitudes.TipoDocumento != EFactoringManagerConsts.gSolicitudSol
            //                            && !usados.Contains(a.IdTitulo)
            //                        )
            //                .OrderByDescending(a => a.FecInicial)
            //                .Include(d => d.DetalleSolicitudes)
            //                .Include(p => p.Pagador)
            //                .Include(t => t.Tercero)
            //                .ToList();
            //}
            //else
            //{
            //    result = _tituloRepositorio.GetAll()
            //                .Where(t => (producto == 1 && t.NumeroTercero == numeroTercero) || (producto == 2 && t.NumeroPagador == numeroTercero))
            //                .OrderByDescending(a => a.FecInicial)
            //                .Include(d => d.DetalleSolicitudes)
            //                .Include(p => p.Pagador)
            //                .Include(t => t.Tercero)
            //                .ToList();
            //}


            //return result;
        }

        public async Task InsertTitulos(Identity.Titulos titulo)
        {
            await _tituloRepositorio.InsertAsync(titulo);
        }

        public async Task<bool> ActualizarEstadoTitulos(IList<Identity.Titulos> titulos, int estadoTitulo)
        {
            for (int i = 0; i < titulos.Count; i++)
            {
                var _titulo = await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == titulos[i].NumeroTercero
                                                            && a.IdTitulo == titulos[i].IdTitulo && a.TipoTitulo == titulos[i].TipoTitulo).FirstOrDefaultAsync();
                _titulo.EstadoTitulo = estadoTitulo;
                await _tituloRepositorio.UpdateAsync(_titulo);

            }
            return true;
        }

        public async Task<bool> ActualizarEstadoTitulo(Identity.Titulos titulo, int estadoTitulo, DateTime? FecProbable, Identity.SolicitudesOperaciones solicitud)
        {
            var _titulo = await _tituloRepositorio.GetAll()
                //.Include( x=> x.DetalleSolicitudes)
                .Where(a => a.NumeroTercero == titulo.NumeroTercero
                                                        && a.IdTitulo == titulo.IdTitulo /*&& a.TipoTitulo == titulo.TipoTitulo*/).FirstOrDefaultAsync();
            if (_titulo == null)
                return false;

            if (estadoTitulo == (int)EstadosTitulos.gRechazada)
            {
                _tituloRepositorio.EliminarTitulo(_titulo.IdTitulo, _titulo.NumeroTercero, _titulo.NumeroPagador.Value, solicitud.IdSolicitud, solicitud.IdOficina, solicitud.TipoDocumento);
                return true;
            }

            _titulo.EstadoTitulo = estadoTitulo;
            if (titulo.VrNetoTitulo > 0) _titulo.VrNetoTitulo = titulo.VrNetoTitulo;
            if (FecProbable != null) _titulo.FecProbable = titulo.FecProbable;

            //Contar titulos en el estado y guardar en el campo

            await _tituloRepositorio.UpdateAsync(_titulo);
            return true;
        }

        public async Task<bool> ActualizarFecProbableTitulo(string idTitulo, int numeroTercero, DateTime fechaProbable)
        {
            var _titulo = await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == numeroTercero
                            && a.IdTitulo == idTitulo /*&& a.TipoTitulo == titulo.TipoTitulo*/).FirstOrDefaultAsync();

            if (_titulo == null)
                return false;

            _titulo.FecProbable = fechaProbable;
            await _tituloRepositorio.UpdateAsync(_titulo);
            return true;

        }

        public async Task<Identity.Titulos> Get(string idTitulo)
        {
            return await _tituloRepositorio.GetAll().Where(t => t.IdTitulo == idTitulo).FirstOrDefaultAsync();

        }

        private bool ValidarTitulo(Identity.Titulos titulo)
        {

            //ojo revisar ARLEY se comenta idPagador
            if (titulo.NumeroTercero > 0 && titulo.IdTitulo != "" && /*titulo.IdPagador != "" &&*/ titulo.TipoMoneda != "" && titulo.VrBrutoTitulo != -1
                && titulo.VrNetoTitulo != -1 && titulo.PorcNegociacion != -1
                // && titulo.CufeTitulo!=""
                )
            {
                if (titulo.PorcReteIvaTitulo == -1 && titulo.VrReteIvaTitulo == -1 && titulo.PorcReteFuenteTitulo == -1 && titulo.VrReteFuenteTitulo == -1 && titulo.PorcReteIcaTitulo == -1 && titulo.VrReteIcaTitulo == -1)
                {
                    titulo.PorcReteIvaTitulo = 0;
                    titulo.VrReteIvaTitulo = 0;
                    titulo.PorcReteFuenteTitulo = 0;
                    titulo.VrReteFuenteTitulo = 0;
                    titulo.PorcReteIcaTitulo = 0;
                    titulo.VrReteIcaTitulo = 0;
                }
                //if (_tituloRepositorio.GetAll().Where(x => x.CufeTitulo == titulo.CufeTitulo).Count() > 0)
                //{
                //    return false;
                //}
                //else return true;
                return true;
            }
            else return false;


        }

        public async Task<bool> UpdateTitulo(Identity.Titulos titulo)
        {
            await _tituloRepositorio.UpdateAsync(titulo);
            return true;

        }

        public async Task ActualizarTitulo(Identity.Titulos titulo)
        {
            await _tituloRepositorio.UpdateAsync(titulo);
            //return true;

            //var detalle = _detalleSolicitudesRepositorio
            //    .GetAll()
            //    .Where(x => x.Titulo.CufeTitulo == titulo.CufeTitulo)
            //    .FirstOrDefault();

            //_detalleSolicitudesManager.Actualizar(detalle, (int)titulo.EstadoTitulo, 0);
        }

        public async Task<bool> ValidarModificacionTitulo(int numeroTercero)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);

            if (tercero == null || tercero.TerceroPadre == null)
                return false;

            var titulos = await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.IdTerceroPadre.Value && a.Actualiza == true).ToListAsync();
            if (titulos.Count > 0)
            {
                foreach (var titulo in titulos)
                {
                    titulo.Actualiza = false;
                    await _tituloRepositorio.UpdateAsync(titulo);
                }
                return true;
            }
            else return false;
        }

        public void SubirRepresentacionTitulos(Identity.Oficinas oficina, Identity.Titulos titulo, Identity.Terceros tercero, byte[] representacion, string fileName)
        {
            string rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ", "_") + "\\Facturas";
            //Identity.DetalleDocumentos detalleDocumentos = new Identity.DetalleDocumentos();
            if (!(Directory.Exists(rutaFichero)))
            {
                Directory.CreateDirectory(rutaFichero);
            }

            System.IO.File.WriteAllBytes(rutaFichero + "\\" + fileName, representacion);

            if (Path.GetExtension(fileName).ToLower() == ".xml")
                titulo.RepresentacionXml = rutaFichero + "\\" + fileName;
            else
                titulo.RepresentacionGrafica = rutaFichero + "\\" + fileName;

        }

        public async Task<bool> SubirRepresentacionTitulos(int numeroTercero, byte[] representacion, string fileName, string idTitulo, string idUsuario)
        {
            var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefaultAsync();
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);
            var _titulo = await _tituloRepositorio.GetAll().Where(a => a.NumeroTercero == numeroTercero && a.IdTitulo == idTitulo && a.TipoTitulo == (int)EFactoringManagerConsts.TiposTitulos.gFacturaElectronica).FirstOrDefaultAsync();
            if (_titulo == null)
                return false;


            SubirRepresentacionTitulos(oficina, _titulo, tercero, representacion, fileName);
            _titulo.FecModifica = DateTime.Now;
            _titulo.IdUsrMod = idUsuario;
            await _tituloRepositorio.UpdateAsync(_titulo);
            return true;


        }

        public byte[] ObtenerPdfTitulo(string dirDocumento)
        {
            return File.ReadAllBytes(dirDocumento);
        }


        public List<TitulosFileInfo> GetDocumentFromZip(byte[] zipFile)
        {

            byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }


            Stream data = new MemoryStream(zipFile); // The original data

            var result = new List<TitulosFileInfo>();

            ZipArchive archive = new ZipArchive(data);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {

                result.Add(new TitulosFileInfo(entry.FullName, ReadFully(entry.Open())));

            }
            return result;
        }

    }
}
