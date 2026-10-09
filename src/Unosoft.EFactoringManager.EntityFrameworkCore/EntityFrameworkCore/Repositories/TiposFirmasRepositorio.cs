using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class TiposFirmasRepositorio : EFactoringManagerRepositoryBase<Identity.TiposFirmas, int>, ITiposFirmasRepositorio
    {
        public TiposFirmasRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }



}
