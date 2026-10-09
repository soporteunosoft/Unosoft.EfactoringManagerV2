using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IDetalleSolicitudesRepositorio : IRepository<EFactoringManager.Identity.DetalleSolicitudes, int>
    {
        List<Identity.DetalleSolicitudes> ObtenerDetalleSolicitudesPagador(string tipoDocumento, int idOficina, int idSolicitud);

       void DeleteObj(Identity.DetalleSolicitudes obj);
    }

}
