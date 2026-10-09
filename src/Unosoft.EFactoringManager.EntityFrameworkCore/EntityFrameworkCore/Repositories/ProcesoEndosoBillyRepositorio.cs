using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public  class ProcesoEndosoBillyRepositorio : EFactoringManagerRepositoryBase<Identity.ProcesoEndosoBilly, int>, IProcesoEndosoBillyRepositorio
    {
        public ProcesoEndosoBillyRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public List<Identity.ProcesoEndosoBilly> GetList(int estado, DateTime? fechaProceso)
        {

            var parEstado = new SqlParameter("@estado", estado);
            var parFechaProceso = new SqlParameter("@fechaProceso", fechaProceso);


            return GetContext().ProcesoEndosoBilly.FromSqlRaw("sp_ObtProcesoEndosoBilly @estado, @fechaProceso ",
                    parEstado,
                    parFechaProceso                

                ).ToList();            
        }

    }
}
