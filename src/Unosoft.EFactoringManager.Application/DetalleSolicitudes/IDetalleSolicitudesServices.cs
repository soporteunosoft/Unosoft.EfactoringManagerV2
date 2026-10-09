using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.DetalleSolicitudes.DTO;

namespace Unosoft.EFactoringManager.DetalleSolicitudes
{
    public interface IDetalleSolicitudesServices : IApplicationService
    {
        Task<List<DetalleSolicitudDTO>> ObtDetalleSolicitudes(ObtDetalleSolicitudesRequest request);
        Task<bool> InsertDetalleSolicitudes(InsertDetalleSolicitudes request);
    }
}
