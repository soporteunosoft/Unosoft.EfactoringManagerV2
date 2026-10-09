using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.Vinculaciones.Dto;

namespace Unosoft.EFactoringManager.Vinculaciones
{
    public interface IVinculacionesServices : IApplicationService
    {
        Task<GetSingleRespuesta> IngresarVinculacion(Identity.Vinculaciones vinculaciones);
        Task<GetSingleRespuesta> ActualizarVinculacion(Identity.Vinculaciones vinculaciones);
        Task<GetSingleRespuesta> ActualizarEstadoVinculacion(VinculacionesDto vinculacionesDto);
        Task<PagedResultDto<VinculacionesDto>> ObtenerVinculaciones(int skip = 0,
                                                    int limit = 20,
                                                    string sortField = "",
                                                    int sortOrder = 1,
                                                    string filter = "");
    }
}
