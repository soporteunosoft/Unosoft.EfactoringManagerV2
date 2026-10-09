using Abp.Application.Services;
using System.Collections.Generic;

namespace Unosoft.EFactoringManager.TiposOperacion
{
    public interface ITiposOperacionServices :  IApplicationService
    {
        List<ModalidadesOperacion.Dto.TipoOperacionDto> ObtenerTipoOperacion();
    }
}
