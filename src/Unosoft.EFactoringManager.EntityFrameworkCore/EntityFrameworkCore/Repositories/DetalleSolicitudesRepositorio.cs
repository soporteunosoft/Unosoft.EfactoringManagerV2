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
    public class DetalleSolicitudesRepositorio : EFactoringManagerRepositoryBase<Identity.DetalleSolicitudes, int>, IDetalleSolicitudesRepositorio
    {

        public DetalleSolicitudesRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public List<Identity.DetalleSolicitudes> ObtenerDetalleSolicitudesPagador(string tipoDocumento, int idOficina, int idSolicitud)
        {

            var TipoDocumento_ = new SqlParameter("@TipoDocumento", tipoDocumento);
            var IdOficina_ = new SqlParameter("@IdOficina", idOficina);
            var IdSolicitud_ = new SqlParameter("@IdSolicitud", idSolicitud);

            return GetContext().DetalleSolicitudes.FromSqlRaw("Select IdPagador from DetalleSolicitudes where TipoDocumento= @TipoDocumento and IdOficina = @IdOficina and  IdSolicitud = @IdSolicitud group by IdPagador",
                    TipoDocumento_,
                    IdOficina_,
                    IdSolicitud_
                ).ToList();

        }

        public void DeleteObj(Identity.DetalleSolicitudes obj)
        {
            var entry = GetContext().Entry(obj);

            if (entry.State == EntityState.Detached)
            {
                // La entidad no está siendo rastreada, hay que adjuntarla
               GetContext().Set<Identity.DetalleSolicitudes>().Attach(obj);
            }

            GetContext().Set<Identity.DetalleSolicitudes>().Remove(obj);
        }
    }
}
