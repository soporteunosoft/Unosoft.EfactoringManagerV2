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
    public class ValidarOperacionRepositorio : EFactoringManagerRepositoryBase<Identity.ValidarOperacion, string>, IValidarOperacionRepositorio
    {

        public ValidarOperacionRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public List<Identity.ValidarOperacion> ValidarOperacion(string idTerceroEmisor, string idTerceroPagador, decimal montoOperacion, int tipoOperacion)
        {

            var parIdTerceroEmisor = new SqlParameter("@idTerceroEmisor", idTerceroEmisor);
            var parIdTerceroPagador = new SqlParameter("@idTerceroPagador", idTerceroPagador);
            var parMontoOperacion = new SqlParameter("@montoOperacion", montoOperacion);
            var parTipoOperacion = new SqlParameter("@tipoOperacion", tipoOperacion);

            return GetContext().ValidarOperaciones.FromSqlRaw("sp_ObtValidadorOperaciones @tipoOperacion, @idTerceroEmisor, @idTerceroPagador, @montoOperacion ",
                    parTipoOperacion,
                    parIdTerceroEmisor,
                    parIdTerceroPagador,
                    parMontoOperacion
                
                ).ToList();
            //"EXEC sp_ObtValidadorOperaciones 1,'830043332', '',100000"
        }
    }
}
