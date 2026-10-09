using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IProcesoEndosoBillyRepositorio :  IRepository<EFactoringManager.Identity.ProcesoEndosoBilly, int>
    {

        List<Identity.ProcesoEndosoBilly> GetList(int estado, DateTime? fechaProceso);


    }
}
