using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IUsuarioRepositorio : IRepository<AbpUsers, long>
    {
    }
}
