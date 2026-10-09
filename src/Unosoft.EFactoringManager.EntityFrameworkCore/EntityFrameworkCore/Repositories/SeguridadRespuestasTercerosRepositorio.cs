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
    public class SeguridadRespuestasTercerosRepositorio : EFactoringManagerRepositoryBase<Identity.SeguridadRespuestasTerceros, int>, ISeguridadRespuestasTercerosRepositorio
    {
        public SeguridadRespuestasTercerosRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }

}
