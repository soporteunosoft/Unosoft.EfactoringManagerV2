using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.Documentos
{
    public class TiposDocumentosManager : IDomainService
    {

        #region Atributos
        private readonly ITiposDocumentosRepositorio _tipoDocumentoRepositorio;
        #endregion

        public TiposDocumentosManager(ITiposDocumentosRepositorio tipoDocumentoRepositorio)
        {
            _tipoDocumentoRepositorio = tipoDocumentoRepositorio;

        }

        public async Task<List<Identity.TiposDocumentos>> GetTipoDocumentos(int TipoDoc)
        {
            var firma = TipoDoc == 1;
            return await _tipoDocumentoRepositorio.GetAll().Where(a => (a.Firma == firma || TipoDoc == 0)).OrderBy(a => a.NomDocumento).ToListAsync();
        }
    }
}
