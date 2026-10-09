using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Unosoft.EFactoringManager.DocumentosJuridicos.Dto;

namespace Unosoft.EFactoringManager.DocumentosJuridicos
{
    public class DocumentosJuridicosServices : ApplicationService, IDocumentosJuridicosServices
    {
        #region Atributos
        DocumentosJuridicosManager _documentosJuridicosManager;
        #endregion

        public DocumentosJuridicosServices(DocumentosJuridicosManager documentosJuridicosManager)
        {
            _documentosJuridicosManager = documentosJuridicosManager;
        }

        [HttpGet]
        public DocumentosJuridicosDto ObtenerDocumentoJuridico(int idDocJuridico)
        {
            try
            {
                var documentoJuridico = _documentosJuridicosManager.ObtenerDocumentoJuridico(idDocJuridico);
                return ObjectMapper.Map<DocumentosJuridicosDto>(documentoJuridico);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
