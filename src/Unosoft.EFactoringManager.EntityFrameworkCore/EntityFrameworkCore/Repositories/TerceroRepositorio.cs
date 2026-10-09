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
    public class TerceroRepositorio : EFactoringManagerRepositoryBase<Identity.Terceros, int>, ITerceroRepositorio
    {

        public TerceroRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
                    
        }

        public Identity.Terceros GetbyId(string idTercero)
        {
            return GetAll()
                .Include(t => t.TerceroPadre)
                .Where(x => x.IdTercero == idTercero).FirstOrDefault();
        }

        public Identity.Terceros GetbyId(int id)
        {
            return GetAll()
                .Include(t => t.TerceroPadre)
                .Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Identity.TercerosComerciantes> ObtenerTercerosComerciante(string idtercero, int tipoTecero)
        {         
            
                var prmIdTercero = new SqlParameter("@IdTercero", idtercero);
                var prmTipoTecero = new SqlParameter("@TipoTercero", tipoTecero);

                return GetContext().TercerosComerciantes.FromSqlRaw("sp_ObtTercerosComerciales @TipoTercero, @IdTercero",
                        prmTipoTecero,
                        prmIdTercero
                    ).ToList();            
        }


        public bool InsertarNotificacionCodigoTercero(string idTercero, int tipoTercero, int codigoTercero)
        {


            var idTercero_ = new SqlParameter("@IdTercero", idTercero);
            var tipoTercero_ = new SqlParameter("@TipoTercero", tipoTercero);
            var codigoTercero_ = new SqlParameter("@CodigoTercero", codigoTercero);

            var resultado = GetContext().Database.ExecuteSqlRaw("sp_InsNotificacionCodigoTercero @IdTercero, @TipoTercero, @CodigoTercero",
                    idTercero_,
                    tipoTercero_,
                    codigoTercero_
                );

            return true;

        }
        public List<Identity.Terceros> ObtenerTercerosUsuarios(bool esUsuario)
        {
            
            var esUsuario_ = new SqlParameter("@EsUsuario", esUsuario);

            return GetContext().Terceros.FromSqlRaw("sp_ObtTercerosUsuarios @EsUsuario",
                    esUsuario_

                ).ToList();

        }

        public AbpUsers ObtIdUsuario(string idTercero)
        {          
                var idTercero_ = new SqlParameter("@IdTercero", idTercero);               

                return GetContext().AbpUsers.FromSqlRaw("sp_ObtIdUsuario @IdTercero",
                      idTercero_

                  ).FirstOrDefault();
            
        }
    }
}
