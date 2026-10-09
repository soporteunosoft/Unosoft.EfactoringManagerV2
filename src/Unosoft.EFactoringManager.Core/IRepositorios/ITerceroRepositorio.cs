using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface ITerceroRepositorio : IRepository<EFactoringManager.Identity.Terceros, int>
    {

        Identity.Terceros GetbyId(string idTercero);
        Identity.Terceros GetbyId(int id);
        List<Identity.Terceros> ObtenerTercerosUsuarios(bool esUsuario);
        bool InsertarNotificacionCodigoTercero(string idTercero, int tipoTercero, int codigoTercero);
        Identity.AbpUsers ObtIdUsuario(string idTercero);
        List<Identity.TercerosComerciantes> ObtenerTercerosComerciante(string idTercero, int tipoTecero);
    }
   
}
