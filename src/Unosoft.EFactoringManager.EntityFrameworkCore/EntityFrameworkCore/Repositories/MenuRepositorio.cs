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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.Web;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class MenuRepositorio : EFactoringManagerRepositoryBase<Identity.Menu, int>, IMenuRepositorio
    {

        public MenuRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public List<Identity.Menu> ObtenerMenuTercero(string IdTercero, int IdOficina, string Usuario)
        {
            var IdTercero_ = new SqlParameter("@IdTercero", IdTercero);
            var IdOficina_ = new SqlParameter("@IdOficina", IdOficina);
            var Usuario_ = new SqlParameter("@Usuario", Usuario);

            return GetContext().Menus.FromSqlRaw("sp_ObtMenusTerceros @IdTercero, @IdOficina, @Usuario",
                    IdTercero_, IdOficina_, Usuario_).ToList();
        }

        public async Task<List<Identity.Menu>> ObtenerMenuTerceroAsync(string IdTercero, int IdOficina, string Usuario)
        {
            var IdTercero_ = new SqlParameter("@IdTercero", IdTercero);
            var IdOficina_ = new SqlParameter("@IdOficina", IdOficina);
            var Usuario_ = new SqlParameter("@Usuario", Usuario);

            return await GetContext().Menus.FromSqlRaw("sp_ObtMenusTerceros @IdTercero, @IdOficina, @Usuario",
                    IdTercero_, IdOficina_, Usuario_).ToListAsync();
        }
    }
}
