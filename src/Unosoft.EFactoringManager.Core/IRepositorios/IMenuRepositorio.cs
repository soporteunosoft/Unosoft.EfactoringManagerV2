using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IMenuRepositorio : IRepository<EFactoringManager.Identity.Menu, int>
    {
       List<Identity.Menu> ObtenerMenuTercero(string IdTercero, int IdOficina, string Usuario);
       Task<List<Identity.Menu>> ObtenerMenuTerceroAsync(string IdTercero, int IdOficina, string Usuario);

    }
}
