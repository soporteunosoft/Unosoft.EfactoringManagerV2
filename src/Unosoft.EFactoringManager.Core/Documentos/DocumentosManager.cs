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
using Unosoft.EfactoringManager.RadianDispapeles;
using Unosoft.EFactoringManager.DetalleDocumentos;
using Unosoft.EFactoringManager.Authorization.Users;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.Documentos
{
    public class DocumentosManager : IDomainService
    {
        #region Atributos
        private readonly IDocumentosRepositorio _documentosRepositorio;
        private readonly DetalleDocumentosManager _detalleDocumentosManager;
        private readonly UserManager _userManager;
        private readonly Terceros.TercerosManager _tercerosManager;
        #endregion

        public DocumentosManager(IDocumentosRepositorio documentosRepositorio,
                                DetalleDocumentosManager detalleDocumentosManager,
                                UserManager userManager,
                                Terceros.TercerosManager tercerosManager)
        {
            _documentosRepositorio = documentosRepositorio;
            _detalleDocumentosManager = detalleDocumentosManager;
            _userManager = userManager;
            _tercerosManager = tercerosManager;

        }

        public async Task<List<Identity.Documentos>> ObtenerDocumentosPorOrigen(int IdDocOrigen)
        {
            return await _documentosRepositorio.GetAll().Where(a => a.OrigenDoc == IdDocOrigen).Include(x => x.TipoDocumento).ToListAsync();
        }


        public async Task<Identity.Documentos> Get(int idDocLinea)
        {
            return await _documentosRepositorio.GetAll().Where(a => a.IdDocLinea == idDocLinea).FirstOrDefaultAsync();
        }

        public async Task Insertar(Identity.Documentos documento, string idTercero, int numeroTercero, int plazoDias, long idUsuario)
        {
            User user = await _userManager.GetUserByIdAsync(idUsuario);

            Identity.Terceros tercero = null;

            if (numeroTercero > 0 )
                tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);
            else
                tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

            int idDocLinea = 0;
            #region Si no existe el documento en la tabla lo inserta

            Identity.Documentos currentDocument = null;
            if (documento.OrigenDoc <= (int)OrigenDocumento.Evaluacion)
                currentDocument = _documentosRepositorio.GetAll().FirstOrDefault(x => x.TipoDoc == documento.TipoDoc && x.OrigenDoc <= documento.OrigenDoc);
            else
                currentDocument = _documentosRepositorio.GetAll().FirstOrDefault(x => x.TipoDoc == documento.TipoDoc && x.OrigenDoc == documento.OrigenDoc);


            if (currentDocument == null)
            {
                documento.IdUsrMod = user.UserName;
                idDocLinea = _documentosRepositorio.InsertAndGetId(documento);
                //currentDocument = _documentosRepositorio.GetAll().FirstOrDefault(x => x.TipoDoc == documento.TipoDoc && x.OrigenDoc == documento.OrigenDoc);


            }
            else
                idDocLinea = currentDocument.Id;

            #endregion

            await _detalleDocumentosManager.InsertarRenovar(new Identity.DetalleDocumentos()
            {
                NumeroTercero = tercero.Id,
                //IdTercero = idTercero,
                TipoTercero = 2,
                IdDocLinea = idDocLinea,
                Obligatorio = documento.Obligatorio,
                Activa = true,
                FecModifica = DateTime.Now,
                FecRenovacion = DateTime.Now.AddDays(plazoDias),
                IdUsrMod = user.UserName

            },
            user.UserName);

        }

        public async Task<bool> Eliminar(int idDetDoc)
        {
            var detDocumento = await _detalleDocumentosManager.ObtenerDetalleDocumentoId(idDetDoc);

            if (detDocumento == null)
                return false;

            var documento = detDocumento.Documentos;

            var listaDetalleDocumento = await _detalleDocumentosManager.ObtenerAllDetalleDocumentos(detDocumento.NumeroTercero, detDocumento.IdDocLinea);

            if(documento.Obligatorio)
                return false;


            if (listaDetalleDocumento.Count == 1)
            {
                await _detalleDocumentosManager.DelDetalleDocumento(detDocumento);
                if (!await _detalleDocumentosManager.DocumentoConDetalle(documento.IdDocLinea))
                    _documentosRepositorio.Delete(documento);
            }
            else
            {
                var newDetDocumento = listaDetalleDocumento[1];
                newDetDocumento.Activa = true;
                newDetDocumento.FecModifica = DateTime.Now;

                await _detalleDocumentosManager.Actualizar(newDetDocumento);

                await _detalleDocumentosManager.DelDetalleDocumento(detDocumento);
            }

            return true;
        }
    }
}
