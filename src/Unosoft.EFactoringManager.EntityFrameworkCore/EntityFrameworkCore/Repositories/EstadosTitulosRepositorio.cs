using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;
using Unosoft.EFactoringManager.IRepositorios;
using Abp.EntityFrameworkCore;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class EstadosTitulosRepositorio : EFactoringManagerRepositoryBase<Identity.EstadosTitulos, int>, IEstadosTitulosRepositorio
    {
        public EstadosTitulosRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
