using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Documentos.Dto;

namespace Unosoft.EFactoringManager.Documentos
{
    public class TipoDocumentoAppServices : ApplicationService, ITipoDocumentoAppServices
    {

        private readonly TiposDocumentosManager _tipoDocumentoManager;
        public TipoDocumentoAppServices(TiposDocumentosManager tipoDocumentoManager)
        {
            _tipoDocumentoManager = tipoDocumentoManager;
        }
        public List<TipoDocumentoDto> GetAll(int TipoDoc)
        {
            var tiposDocumentos = _tipoDocumentoManager.GetTipoDocumentos(TipoDoc);

            return ObjectMapper.Map<List<TipoDocumentoDto>>(tiposDocumentos);
            
        }
    }
}
