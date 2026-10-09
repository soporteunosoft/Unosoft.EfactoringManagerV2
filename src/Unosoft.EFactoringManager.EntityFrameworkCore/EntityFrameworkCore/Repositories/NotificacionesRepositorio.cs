using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class NotificacionesRepositorio : EFactoringManagerRepositoryBase<Identity.Notificacion, int>, INotificacionesRepositorio
    {
        public NotificacionesRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public void Create(Identity.Notificacion notificacion)
        {
            var parIdCorreo = new SqlParameter("@idCorreo", notificacion.IdCorreo);
            var parIdOficina = new SqlParameter("@idOficina", notificacion.IdOficina);
            var parTipoDocumento = new SqlParameter("@tipoDocumento", notificacion.TipoDocumento);
            var parId = new SqlParameter("@id", notificacion.IdDocumento);

            GetContext().Database.ExecuteSqlRaw(
                "sp_GenerarNotificacion @idCorreo, @idOficina, @tipoDocumento, @id",
                parIdCorreo,
                parIdOficina,
                parTipoDocumento,
                parId
            );
        }
    }
}
