using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;
using Unosoft.EFactoringManager.DetalleDocumentos.DTO;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.DetalleDocumentos
{
    public class DetalleDocumentosManager : IDomainService
    {
        #region Atributos
        private readonly IDetalleDocumentosRepositorio _detalleDocumentosRepositorio;
        private readonly IOficinasRepositorio _oficinaRepositorio;
        private readonly IFirmasDetalleDocumentosRepositorio _firmaDocumentosRepositorio;
        private readonly IDocumentosRepositorio _documentoRepositorio;

        private readonly Terceros.TercerosManager _tercerosManager;
        //private readonly Documentos.DocumentosManager _documentosManager;
        #endregion

        public DetalleDocumentosManager(IDetalleDocumentosRepositorio detalleDocumentosRepositorio,
                                        IOficinasRepositorio oficinaRepositorio,
                                        IFirmasDetalleDocumentosRepositorio firmaDocumentosRepositorio,
                                        Terceros.TercerosManager tercerosManager,
                                        IDocumentosRepositorio documentoRepositorio
                                        )
        {
            _detalleDocumentosRepositorio = detalleDocumentosRepositorio;
            _oficinaRepositorio = oficinaRepositorio;
            _firmaDocumentosRepositorio = firmaDocumentosRepositorio;
            _tercerosManager = tercerosManager;
            _documentoRepositorio = documentoRepositorio;
        }

        public async Task<List<Identity.DetalleDocumentos>> ObtenerDetalleDocumentos(int numeroTercero, int IdDocOrigen, string tipoDocumento)
        {
            bool firma = false;
            if (tipoDocumento == "1") firma = true;

            return await _detalleDocumentosRepositorio.GetAll()
                .Where(a => a.NumeroTercero == numeroTercero
                         && a.Documentos.OrigenDoc == IdDocOrigen
                         && a.Activa
                         && (a.Documentos.TipoDocumento.Firma == firma || tipoDocumento == null))
                .Include(x => x.Documentos.TipoDocumento)
                .OrderByDescending(a => a.FecModifica)
                .ThenByDescending(d => d.Documentos.Obligatorio).ToListAsync();
        }

        public async Task<List<Identity.DetalleDocumentos>> ObtenerAllDetalleDocumentos(int numeroTercero, int idDocLinea)
        {
            return await _detalleDocumentosRepositorio.GetAll()
                .Where(a => a.NumeroTercero == numeroTercero && a.IdDocLinea == idDocLinea)
                .OrderByDescending(x => x.Id).ToListAsync();
        }


        public async Task<Tuple<List<Identity.DetalleDocumentos>, List<Terceros.ResumenTercero>>> ObtenerDetalleDocumentosFirmar(string idTercero, int IdDocOrigen)
        {
           
            var listIdsTercero = await _tercerosManager.ObtenerIdsTercero(idTercero);
            var idsTerceros = listIdsTercero.Select(t => t.Id).ToList();
            var idsTercerosPadres = listIdsTercero
                .Where(t => t.IdTerceroPadre.HasValue)
                .Select(t => t.IdTerceroPadre.Value)
                .ToList();

            var firmasQuery = _firmaDocumentosRepositorio.GetAll()
                                .Where(f => idsTerceros.Contains(f.NumTerceroFirma) && !f.Firma);

            var firmasIds = await firmasQuery
                    .Select(f => new { f.IdDetDoc, f.NumeroTercero })
                    .ToListAsync(); // se ejecuta en memoria

            var result = _detalleDocumentosRepositorio.GetAll()
                .Include(x => x.Documentos.TipoDocumento)
                .AsEnumerable() // a partir de aquí en memoria
                .Where(dd => dd.Documentos.TipoFirma == (int)TipoFirma.Digital
                             && idsTercerosPadres.Contains(dd.NumeroTercero)
                             && dd.Activa
                             && dd.Documentos.OrigenDoc == IdDocOrigen
                             && firmasIds.Any(f => f.IdDetDoc == dd.Id && f.NumeroTercero == dd.NumeroTercero))
                .Distinct()
                .ToList();

            return new Tuple<List<Identity.DetalleDocumentos>, List<Terceros.ResumenTercero>>(
                result,
                listIdsTercero
            );
        }

        public async Task<List<Identity.DetalleDocumentos>> ObtenerDetalleDocumentosFirmarSol(string idTercero, int IdDocOrigen, string TipoDocumento, int IdOficina, int IdDocumento)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            //OJO REVISAR (Consuta de documentos, validar si debe ser con numtercero
            //ademas validar que exista el documento dirdocumento != null
            //WILLIAN LONDONO 02-09-2022
            return await _detalleDocumentosRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.TerceroPadre.Id
                                                                && a.TipoDocumento == TipoDocumento
                                                                && a.IdOficina == IdOficina
                                                                && a.IdDocumento == IdDocumento
                                                                && a.Documentos.OrigenDoc == IdDocOrigen
                                                                && (a.Firma == false || a.FecRenovacion < DateTime.Now || a.Firma == null))
                                                                    .Include(x => x.Documentos.TipoDocumento)
                                                                    .ToListAsync();
        }

        public async Task<List<Identity.DetalleDocumentos>> ObtenerDetalleDocumentosFirmadosSol(string idTercero, int IdDocOrigen, string TipoDocumento, int IdOficina, int IdDocumento)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            return await _detalleDocumentosRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.Id && a.TipoDocumento == TipoDocumento
                                                                && a.IdOficina == IdOficina && a.IdDocumento == IdDocumento &&
                                                                a.Documentos.OrigenDoc == IdDocOrigen && a.Activa).Include(x => x.Documentos.TipoDocumento).ToListAsync();
        }

        public async Task<Identity.DetalleDocumentos> ObtenerDetalleDocumentoPorTipo(string idTercero, int tipoDoc)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            return await _detalleDocumentosRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.Id && a.Documentos.TipoDoc == tipoDoc && a.Activa).Include(x => x.Documentos.TipoDocumento).FirstOrDefaultAsync();
        }

        public async Task<List<DocumentosIniciales>> ObtenerDetalleDocumentosPorTercero(string idTercero)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            if (tercero == null)
                return new List<DocumentosIniciales>();

            var listIdsTercero = await _tercerosManager.ObtenerIdsTercero(idTercero);
            var idsTerceros = listIdsTercero.Select(t => t.Id).ToList();

            var detalle = from det in _detalleDocumentosRepositorio.GetAll()
                                                                   .Include(x => x.Documentos)
                                                                   .Include(x => x.Documentos.TipoDocumento)
                                                                   .Where(dd => dd.Documentos.TipoFirma == (int)TipoFirma.Digital && dd.NumeroTercero == tercero.Id && dd.Activa
                                                                           && dd.Documentos.OrigenDoc == (int)EFactoringManagerConsts.DocumentoOrigen.gJuridicos)


                          join f in _firmaDocumentosRepositorio.GetAll().Where(x => idsTerceros.Contains(x.NumTerceroFirma))
                            on new { det.Id, det.NumeroTercero } equals new { f.Id, f.NumeroTercero } into firmad
                          from firmas in firmad //.DefaultIfEmpty()
                          select new DocumentosIniciales
                          {
                              Documento = det,
                              SinFirma = (firmas.Firma == false)
                          }
                         ;

            return await detalle.ToListAsync();




        }

        public async Task<List<Identity.DetalleDocumentos>> GetDetalleDocumentoPorTipo(string idTercero, List<int> tipoDoc)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            return await _detalleDocumentosRepositorio.GetAll()
                    .Include(x => x.Documentos.TipoDocumento)
                    .Where(a => a.NumeroTercero == tercero.Id /*&& a.DirDocumento != null*/ && tipoDoc.Contains(a.Documentos.TipoDoc) && a.Activa).ToListAsync();
        }

        public async Task<List<Identity.DetalleDocumentos>> GetDetalleDocumentoPorTipo(int numeroTercero, List<int> tipoDoc)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);

            return await _detalleDocumentosRepositorio.GetAll()
                    .Include(x => x.Documentos.TipoDocumento)
                    .Where(a => a.NumeroTercero == tercero.Id /*&& a.DirDocumento != null*/ && tipoDoc.Contains(a.Documentos.TipoDoc) && a.Activa).ToListAsync();
        }

        public async Task<Identity.DetalleDocumentos> ObtenerDetalleDocumentoId(int idDetDoc)
        {
            return await _detalleDocumentosRepositorio.GetAll().Where(a => a.Id == idDetDoc)
                        .Include(x => x.Documentos)
                        .ThenInclude(x => x.TipoDocumento)
                        .Include(x => x.FirmasDetalleDocumentos)
                        .FirstOrDefaultAsync();
        }

        public async Task<Identity.DetalleDocumentos> ObtenerDetalleDocumentoPorTipoSol(string idTercero, int tipoDoc, string tipoDocumento, int idOficina, int idDocumento)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            return await _detalleDocumentosRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.Id && a.Documentos.TipoDoc == tipoDoc
                                                                    && a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina
                                                                    && a.IdDocumento == idDocumento && a.Activa).Include(x => x.Documentos.TipoDocumento).FirstOrDefaultAsync();
        }

        public async Task InsertarDetalleDocumentos(Identity.DetalleDocumentos detalleDocumentos)
        {
            await _detalleDocumentosRepositorio.InsertAsync(detalleDocumentos);
        }

        public async Task<bool> DelDetalleDocumento(Identity.DetalleDocumentos detalleDocumentos)
        {
            var detalleDoc = await _detalleDocumentosRepositorio.GetAll().Where(a => a.Id == detalleDocumentos.Id).FirstOrDefaultAsync();
            if (detalleDoc == null)
            {
                return false;
            }
            else
            {

                await _detalleDocumentosRepositorio.DeleteAsync(detalleDoc);
                return true;
            }

        }

        public async Task<bool> ActualizarEstadosDetalleDocumentos(Identity.DetalleDocumentos detalleDocumentos)
        {
            var detalleDoc = await _detalleDocumentosRepositorio.GetAll().Where(a => a.Id == detalleDocumentos.Id).FirstOrDefaultAsync();
            if (detalleDoc == null)
            {
                return false;
            }
            else
            {

                detalleDoc.Validar = detalleDocumentos.Validar;
                detalleDoc.Obligatorio = detalleDocumentos.Obligatorio;
                detalleDoc.FecDocumento = detalleDocumentos.FecDocumento;
                detalleDoc.FecRenovacion = detalleDocumentos.FecRenovacion;
                detalleDoc.IdUsrMod = detalleDocumentos.IdUsrMod;
                detalleDoc.FecModifica = DateTime.Now;
                await _detalleDocumentosRepositorio.UpdateAsync(detalleDoc);
                return true;
            }
        }

        public async Task<bool> ActualizarFechaRenovacionDetalleDocumentos(Identity.DetalleDocumentos detalleDocumentos)
        {
            var detalleDoc = await _detalleDocumentosRepositorio.GetAll().Where(a => a.Id == detalleDocumentos.Id).FirstOrDefaultAsync();
            if (detalleDoc == null)
            {
                return false;
            }

            if (detalleDocumentos.FecRenovacion == null)
            {
                detalleDoc.FecRenovacion = detalleDocumentos.FecRenovacion;
            }
            else
            {
                detalleDoc.FecRenovacion = Convert.ToDateTime((Convert.ToDateTime(detalleDocumentos.FecRenovacion.ToString())).ToString("yyyy-MM-dd"));
            }



            detalleDoc.IdUsrMod = detalleDocumentos.IdUsrMod;
            detalleDoc.FecModifica = DateTime.Now;
            await _detalleDocumentosRepositorio.UpdateAsync(detalleDoc);
            return true;

        }

        public async Task Actualizar(Identity.DetalleDocumentos detalleDocumento)
        {
            await _detalleDocumentosRepositorio.UpdateAsync(detalleDocumento);
        }

        public async Task<bool> ActualizarDirDocDetalleDocumentos(Identity.DetalleDocumentos detalleDocumentos)
        {

            var detalleDoc = await _detalleDocumentosRepositorio.GetAll()
                                                           .Include(x => x.Documentos)
                                                           .Include(y => y.Documentos.TipoDocumento)
                                                           .Where(a => a.Id == detalleDocumentos.Id)
                                                           .FirstOrDefaultAsync();

            var diasAdicionales = detalleDoc.Documentos.DiasRenovacion ?? 60;

            if (detalleDoc == null)
                return false;


            detalleDoc.DirDocumento = detalleDocumentos.DirDocumento;
            detalleDoc.FecDocumento = detalleDocumentos.FecDocumento;

            detalleDoc.FirmaAutorizada = false;
            detalleDoc.IdUsrMod = detalleDocumentos.IdUsrMod;
            detalleDoc.FecModifica = detalleDocumentos.FecModifica;




            //if (detalleDoc.Documentos.TipoDocumento.TipoDoc == (int)EFactoringManagerConsts.DocumentoSoporte.gPagare || detalleDoc.Documentos.TipoDocumento.TipoDoc == (int)EFactoringManagerConsts.DocumentoSoporte.gContratoFactoring ||
            //   detalleDoc.Documentos.TipoDocumento.TipoDoc == (int)EFactoringManagerConsts.DocumentoSoporte.gConvenioRecaudo || detalleDoc.Documentos.TipoDocumento.TipoDoc == (int)EFactoringManagerConsts.DocumentoSoporte.gCartaInstrucciones)
            //{
            //FOR DO:REVISAR CON ARLEY PARAMETROS FECRENOVACION PARA NOTIFICAR AL CLIENTE ACTUALIZACION
            //detalleDocumentos.FecRenovacion = DateTime.Now.AddDays(60);//ADICIONA 60 DIAS;

            detalleDoc.FecRenovacion = detalleDocumentos.FecDocumento.Value.AddDays(diasAdicionales);
            //}
            await _detalleDocumentosRepositorio.UpdateAsync(detalleDoc);
            return true;

        }
        public async Task<bool> ActualizarFirmaDetalleDocumento(string idTercero, int idDetDoc, bool firma)
        {
            var detalleDoc = await _detalleDocumentosRepositorio.GetAll().Where(a => a.Id == idDetDoc).Include(x => x.Documentos).Include(y => y.Documentos.TipoDocumento).FirstOrDefaultAsync();
            if (detalleDoc == null)
                return false;


            detalleDoc.Firma = firma;
            detalleDoc.IdUsrMod = idTercero;
            detalleDoc.FecModifica = DateTime.Now;
            await _detalleDocumentosRepositorio.UpdateAsync(detalleDoc);
            return true;

        }

        public async Task<bool> ActualizarFirmaDetalleDocumentoAutoriza(string idTercero, int idDetDoc, bool firma)
        {
            var detalleDoc = await _detalleDocumentosRepositorio.GetAll().Where(a => a.Id == idDetDoc)
                                                           .Include(x => x.Documentos)
                                                           .Include(y => y.Documentos.TipoDocumento).FirstOrDefaultAsync();
            if (detalleDoc == null)
                return false;

            detalleDoc.FirmaAutorizada = firma;
            detalleDoc.IdUsrMod = idTercero;
            detalleDoc.FecModifica = DateTime.Now;
            await _detalleDocumentosRepositorio.UpdateAsync(detalleDoc);
            return true;

        }

        public byte[] ObtenerPdfDocumento(string dirDocumento)
        {
            return System.IO.File.ReadAllBytes(dirDocumento);
        }

        public async Task<bool> GuardarDocumentosVinculacion(string idTercero, byte[] xml, string fileName, int idDocLinea)
        {
            var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefaultAsync();
            string rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\Vinculaciones\\" + idTercero;

            Identity.DetalleDocumentos detalleDocumentos = new Identity.DetalleDocumentos();
            var documento = await _documentoRepositorio.GetAll().FirstOrDefaultAsync(x => x.IdDocLinea == idDocLinea);

            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            if (documento == null)
                return false;

            if (!Directory.Exists(rutaFichero))
            {
                Directory.CreateDirectory(rutaFichero);
            }

            System.IO.File.WriteAllBytes(rutaFichero + "\\" + fileName, xml);
            detalleDocumentos.IdDocLinea = idDocLinea;
            detalleDocumentos.NumeroTercero = tercero.Id;
            detalleDocumentos.DirDocumento = rutaFichero + "\\" + fileName;
            detalleDocumentos.Validar = false;
            detalleDocumentos.IdUsrMod = idTercero;
            detalleDocumentos.FecDocumento = DateTime.Now;
            detalleDocumentos.Activa = true;
            //detalleDocumentos.FecRenovacion = DateTime.Now;

            //WIGELO:FOR DO 13/07/2022 SE DEBE LEE LOS DIAS DE RENOVACION DE DOCUMENTOS

            detalleDocumentos.FecRenovacion = DateTime.Now.AddDays(documento.DiasRenovacion ?? 60);//ADICIONA 60 DIAS;
            detalleDocumentos.FecModifica = DateTime.Now;
            await InsertarDetalleDocumentos(detalleDocumentos);
            return true;

        }

        public async Task<bool> ActualizarDocumentos(int numeroTercero, byte[] xml, string fileName, int idDetDoc, string idUsuarioMod)
        {
            //09/02/2022:WIGLE:Mejora en los nombres de la carpeta
            var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefaultAsync();
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);
            var detalleDoc = await ObtenerDetalleDocumentoId(idDetDoc);


            string rutaFichero;


            if (detalleDoc.Documentos.OrigenDoc == 2)
            {
                //rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ","_")+ "\\Documentos_Jurídicos";
                rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ", "_").Replace("\r", "").Replace("\n", "") + "\\Documentos_Juridicos";
            }
            else
            {
                rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ", "_").Replace("\r","").Replace("\n","") + "\\Documentos_Renovacion_Cupos";
            }

            Identity.DetalleDocumentos detalleDocumentos = new Identity.DetalleDocumentos();
            fileName = string.Format(fileName + "_{0:ddMMyyyyHHmm}.pdf", DateTime.Now);
            //return true;
            //rutaFichero = @"D:\WEB";
            if (!(Directory.Exists(rutaFichero)))
            {
                Directory.CreateDirectory(rutaFichero);
            }
            if (Directory.Exists(rutaFichero))
            {
                System.IO.File.WriteAllBytes(rutaFichero + "\\" + fileName, xml);
                detalleDocumentos.Id = idDetDoc;
                detalleDocumentos.DirDocumento = rutaFichero + "\\" + fileName;
                detalleDocumentos.IdUsrMod = idUsuarioMod;
                detalleDocumentos.FecDocumento = DateTime.Now;
                detalleDocumentos.FecModifica = DateTime.Now;
                detalleDocumentos.Activa = true;
                return await ActualizarDirDocDetalleDocumentos(detalleDocumentos);

            }
            else return false;

        }

        public async Task<string> ValidarDocumentosAutorizarSolicitud(string idTercero)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            var documentos = await _detalleDocumentosRepositorio.GetAll().Where(a => a.NumeroTercero == tercero.Id && a.Documentos.OrigenDoc != (int)EFactoringManagerConsts.DocumentoOrigen.gDocSolicitudes && a.Obligatorio == true && a.Activa).Include(x => x.Documentos.TipoDocumento).ToListAsync();

            string documentosNoValidos = "";
            bool validacion = true;

            foreach (Identity.DetalleDocumentos detalleDoc in documentos)
            {
                if (detalleDoc.FecRenovacion < DateTime.Now || detalleDoc.Validar == false)
                {
                    documentosNoValidos = documentosNoValidos + " " + detalleDoc.Documentos.TipoDocumento.NomDocumento + ",";
                    validacion = false;
                }
            }

            if (!validacion)
            {
                documentosNoValidos = "Falta validar los siguientes documentos obligatorios para autorizar la solicitud: " + documentosNoValidos;
            }


            return documentosNoValidos;
        }

        public async Task<Identity.DetalleDocumentos> ObtenerDetalleDocumentoByIdFirmaElectronica(string idFirmaElectronica)
        {
            return await _detalleDocumentosRepositorio.GetAll()
                        .Include(x => x.FirmasDetalleDocumentos)
                        .Where(a => a.IdFirmaElectronica == idFirmaElectronica && a.Activa)
                        .FirstOrDefaultAsync();
        }

        public async Task<bool> InsertarRenovar(Identity.DetalleDocumentos detalleDocNew, string idUsuarioMod)
        {
            var detalleDoc = await _detalleDocumentosRepositorio.GetAll()
                                                          .Where(a => a.IdDocLinea == detalleDocNew.IdDocLinea
                                                                    && a.NumeroTercero == detalleDocNew.NumeroTercero && a.Activa).FirstOrDefaultAsync();

            if (detalleDoc == null)
            {
                detalleDoc = detalleDocNew;
            }
            else
            {
                detalleDoc.Activa = false;
                await _detalleDocumentosRepositorio.UpdateAsync(detalleDoc);
            }


            var newDetalleDoc = new Identity.DetalleDocumentos()
            {
                IdDocLinea = detalleDoc.IdDocLinea,
                IdDocumento = detalleDoc.IdDocumento,
                IdOficina = detalleDoc.IdOficina,
                NumeroTercero = detalleDoc.NumeroTercero,
                Obligatorio = detalleDoc.Obligatorio,
                FecRenovacion = detalleDoc.FecRenovacion,
                Activa = true,
                Validar = false,
                TipoDocumento = detalleDoc.TipoDocumento,
                IdUsrMod = idUsuarioMod,
                FecModifica = DateTime.Now
            };
            await _detalleDocumentosRepositorio.InsertAsync(newDetalleDoc);
            return true;

        }


        public async Task<bool> DocumentoConDetalle(int idDocLinea)
        {
            return await _detalleDocumentosRepositorio.GetAll().AnyAsync(x => x.IdDocLinea == idDocLinea);
        }

    }
}
