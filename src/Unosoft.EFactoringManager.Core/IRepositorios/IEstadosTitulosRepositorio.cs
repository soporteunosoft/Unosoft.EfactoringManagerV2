using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IEstadosTitulosRepositorio :IRepository<EFactoringManager.Identity.EstadosTitulos, int>
    {
    }
}
