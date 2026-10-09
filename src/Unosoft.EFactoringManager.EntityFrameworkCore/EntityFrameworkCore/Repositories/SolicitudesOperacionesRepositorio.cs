using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.Web;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class SolicitudesOperacionesRepositorio : EFactoringManagerRepositoryBase<Identity.SolicitudesOperaciones, int>, ISolicitudesOperacionesRepositorio
    {
        public SolicitudesOperacionesRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public List<Identity.CNSProyeccionDetalleSolicitudes> ObtenerProyeccionDetalleSolicitudes(string tipoDoc, int idOficina, int idSolicitud)
        {

            var tipoDoc_ = new SqlParameter("@TipoDoc", tipoDoc);
            var idOficina_ = new SqlParameter("@IdOficina", idOficina);
            var idSolicitud_ = new SqlParameter("@idSolicitud", idSolicitud);

            return GetContext().CNSProyeccionDetalleSolicitudes.FromSqlRaw("sp_ObtProyeccionDetalleSolicitudes @TipoDoc,  @IdOficina, @IdSolicitud",
                    tipoDoc_,
                    idOficina_,
                    idSolicitud_
                ).ToList();

        }

        public List<Identity.PagadoresTitulosSol> ObtenerPagadoresTitulosPorSolicitud(string tipoDoc, int idOficina, int idSolicitud)
        {
            
                var tipoDoc_ = new SqlParameter("@TipoDoc", tipoDoc);
                var idOficina_ = new SqlParameter("@IdOficina", idOficina);
                var idSolicitud_ = new SqlParameter("@idSolicitud", idSolicitud);

                return GetContext().PagadoresTitulosSol.FromSqlRaw("sp_ObtPagadoresPorSolicitud @TipoDoc,  @IdOficina, @IdSolicitud",
                        tipoDoc_,
                        idOficina_,
                        idSolicitud_
                    ).ToList();
          
        }
    }
}
