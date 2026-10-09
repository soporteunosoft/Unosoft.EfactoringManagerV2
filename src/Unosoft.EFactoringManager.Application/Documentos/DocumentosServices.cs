using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dispapeles;
using Unosoft.EFactoringManager.Documentos.Dto;
using Unosoft.EFactoringManager.Titulos;

namespace Unosoft.EFactoringManager.Documentos
{
    public class DocumentosServices : ApplicationService, IDocumentosServices
    {
        #region Atributos
        readonly DocumentosManager _documentosManager;
        readonly DispapelesManager _dispapelesManager;
        TitulosManager _tituloManager;
        #endregion

        public DocumentosServices(DocumentosManager documentosManager, DispapelesManager dispapelesManager, TitulosManager tituloManager)
        {
            _documentosManager = documentosManager;
            _dispapelesManager = dispapelesManager;
            _tituloManager = tituloManager;
        }

        public async Task<bool> InsertarDocumento(DocumentoNewDto documento)
        {
            await _documentosManager.Insertar(new Identity.Documentos()
            {
                TipoLinea = documento.TipoLinea,
                TipoDoc = documento.TipoDocumento,
                TipoFirma = documento.TipoFirma,
                TipoFirmaElectronica = documento.TipoFirmaElectronica,
                Obligatorio = documento.Obligatorio,
                OrigenDoc = documento.OrigenDoc,
                FirmaPagina = 1,
                FecModifica = DateTime.Now

            }, documento.IdTercero,
            documento.NumeroTercero,
            documento.PlazoDias,
            AbpSession.UserId.Value);

            return true;
        }

        [HttpGet]
        public async Task<List<Documentos.Dto.DocumentosDto>> ObtenerDocumentos(int documentoOrigen)
        {
            try
            {
                var documentos = await _documentosManager.ObtenerDocumentosPorOrigen(documentoOrigen);
                return ObjectMapper.Map<List<Documentos.Dto.DocumentosDto>>(documentos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<bool> EliminarDocumento(int IdDetDoc)
        {
            try
            {
                return await _documentosManager.Eliminar(IdDetDoc); 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
