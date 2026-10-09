using Abp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;



namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class ValidarEventosRadianRepositorio : EFactoringManagerRepositoryBase<Identity.ValidarEventosRadian, string>, IValidarEventosRadianRepositorio
    {
        public ValidarEventosRadianRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
