using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;
using Unosoft.EFactoringManager.Terceros;

namespace Unosoft.EFactoringManager.FirmasDetalleDocumentos
{
    public class FirmasDetalleDocumentosManager : IDomainService
    {

        #region Atributos
        private readonly IFirmasDetalleDocumentosRepositorio _firmasDetalleDocumentosRepositorio;
        private readonly TercerosManager _tercerosManager;
        #endregion

        //    public TiposEventosRadianRepositorio(ITiposEventosRadianRepositorio tiposEventosRadianRepositorio)
        //    {
        //        _TiposEventosRadianRepositorio = tiposEventosRadianRepositorio;
        //    }

        public FirmasDetalleDocumentosManager(IFirmasDetalleDocumentosRepositorio firmasDetalleDocumentosRepositorio,
                                                TercerosManager tercerosManager)
        {
            _firmasDetalleDocumentosRepositorio = firmasDetalleDocumentosRepositorio;
            //_oficinaRepositorio = oficinaRepositorio;
            _tercerosManager = tercerosManager;
        }


        public async Task InsertarFirmasDetalleDocumentos(Identity.FirmasDetalleDocumentos FirmasDetalleDocumentos)
        {
            await _firmasDetalleDocumentosRepositorio.InsertAsync(FirmasDetalleDocumentos);
        }

        public async Task ActualizarFirmasDetalleDocumentos(Identity.FirmasDetalleDocumentos FirmasDetalleDocumentos)
        {
            await _firmasDetalleDocumentosRepositorio.UpdateAsync(FirmasDetalleDocumentos);
        }


        public async Task<bool> GuardarFirmasDetalleDocumentos(string idTercero, string idTerceroOrdenante, int IdDetDoc, bool firma)
        {
            //var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == 1).FirstOrDefault();
            //string rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\Vinculaciones\\" + idTercero;
            //Identity.FirmasDetalleDocumentos firmasDetalleDocumentos = new Identity.FirmasDetalleDocumentos();
            //if (!(Directory.Exists(rutaFichero)))
            //{
            //    Directory.CreateDirectory(rutaFichero);
            //}
            //if (Directory.Exists(rutaFichero))
            //{
            //    System.IO.File.WriteAllBytes(rutaFichero + "\\" + fileName, xml);
            //firmasDetalleDocumentos.IdDetDoc = IdDetDoc;
            //firmasDetalleDocumentos.IdTercero = idTercero;
            //firmasDetalleDocumentos.Firma = firma;
            //firmasDetalleDocumentos.IdUsrMod = idTercero;
            //firmasDetalleDocumentos.FecModifica = DateTime.Now;
            //firmasDetalleDocumentos.HoraModifica = DateTime.Now.ToString("HH:mm:ss");

            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            var firmasDetalleDocumentos = new Identity.FirmasDetalleDocumentos()
            {
                IdDetDoc = IdDetDoc,
                NumeroTercero = tercero.Id,
                NumTerceroFirma = 0,
                Firma = firma,
                IdUsrMod = idTercero,
                FecModifica = DateTime.Now,
                HoraModifica = DateTime.Now.ToString("HH:mm:ss")
            };
            await InsertarFirmasDetalleDocumentos(firmasDetalleDocumentos);
            return true;
        }

        public async Task<bool> ActualizarFirmasDetalleDocumentos(string idTercero, int numeroTercero, int IdDetDoc, bool firma)
        {
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            var firmasDetalleDocumentos = await _firmasDetalleDocumentosRepositorio.GetAll()
                            .Where(x => x.IdDetDoc == IdDetDoc && x.NumeroTercero == tercero.Id
                                           && x.NumeroTercero == numeroTercero).FirstOrDefaultAsync();

            if (firmasDetalleDocumentos == null)
                return false;

            firmasDetalleDocumentos.Firma = firma;
            firmasDetalleDocumentos.IdUsrMod = idTercero;
            firmasDetalleDocumentos.FecModifica = DateTime.Now;
            firmasDetalleDocumentos.HoraModifica = DateTime.Now.ToString("HH:mm:ss");

            await ActualizarFirmasDetalleDocumentos(firmasDetalleDocumentos);
            return true;
        }

        public async Task EliminarFirmasDetalleDocumentos(Identity.FirmasDetalleDocumentos FirmasDetalleDocumentos)
        {
            await _firmasDetalleDocumentosRepositorio.DeleteAsync(FirmasDetalleDocumentos);
        }
        public bool TieneDocumentosPendienteXFirma()
        {
            return true;
        }
    }

}

