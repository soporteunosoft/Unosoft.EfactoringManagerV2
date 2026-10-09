using Abp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
 
    public class TiposEventosRadianRepositorio : EFactoringManagerRepositoryBase<Identity.TiposEventosRadian, string>, ITiposEventosRadianRepositorio
    {
        public TiposEventosRadianRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
