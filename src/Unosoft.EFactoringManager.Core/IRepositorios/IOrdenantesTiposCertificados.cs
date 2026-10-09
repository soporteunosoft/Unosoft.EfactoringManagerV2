using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IOrdenantesTiposCertificados : IRepository<EFactoringManager.Identity.OrdenantesTiposCertificados, int>
    {
    }
}
