using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Unosoft.EFactoringManager.IRepositorios;
using Abp.EntityFrameworkCore;
using Unosoft.EFactoringManager.Identity;
using Microsoft.EntityFrameworkCore;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class TiposOperacionesRepositorio : EFactoringManagerRepositoryBase<Identity.TiposOperaciones, int>, ITiposOperacionesRepositorio
    {
        public TiposOperacionesRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public List<Identity.TiposOperaciones> GetList()
        {
            return GetContext().TiposOperaciones.FromSqlRaw("sp_ObtTiposOperaciones").ToList();
        }
    }
}
