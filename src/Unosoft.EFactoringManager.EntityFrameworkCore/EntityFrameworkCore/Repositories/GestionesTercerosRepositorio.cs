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
    public class GestionesTercerosRepositorio : EFactoringManagerRepositoryBase<Identity.GestionesTerceros, int>, IGestionesTercerosRepositorio
    {
        public GestionesTercerosRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public Identity.GestionesTerceros GetbyId(int numTercero)
        {
            return GetAll()
                .Include(t => t.NumeroTercero)
                .Where(x => x.NumeroTercero == numTercero).FirstOrDefault();
        }
    }
}
