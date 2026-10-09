using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Documentos
{
    public interface ITipoDocumentoAppServices : IApplicationService
    {
        List<Documentos.Dto.TipoDocumentoDto> GetAll(int TipoDoc);
    }
}
