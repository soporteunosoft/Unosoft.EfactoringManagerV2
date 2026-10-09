using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface ISolicitudesOperacionesRepositorio : IRepository<EFactoringManager.Identity.SolicitudesOperaciones, int>
    {
        List<Identity.CNSProyeccionDetalleSolicitudes> ObtenerProyeccionDetalleSolicitudes(string tipoDoc, int idOficina, int idSolicitud);
        List<Identity.PagadoresTitulosSol> ObtenerPagadoresTitulosPorSolicitud(string tipoDoc, int idOficina, int idSolicitud);
    }
}
