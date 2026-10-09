using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.DocumentosJuridicos
{
    public class DocumentosJuridicosManager : IDomainService
    {
        #region Atributos
        private readonly IDocumentosJuridicosRepositorio _documentosJuridicosRepositorio;
        #endregion

        public DocumentosJuridicosManager(IDocumentosJuridicosRepositorio documentosJuridicosRepositorio)
        {
            _documentosJuridicosRepositorio = documentosJuridicosRepositorio;
        }

        public async Task<Identity.DocumentosJuridicos> ObtenerDocumentoJuridico(int docJuridico)
        {
            return await _documentosJuridicosRepositorio.GetAll().Where(a => a.IdDocJuridico == docJuridico).FirstOrDefaultAsync();
        }

    }
}
