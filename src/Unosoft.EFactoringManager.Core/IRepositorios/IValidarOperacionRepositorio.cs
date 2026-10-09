using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public  interface IValidarOperacionRepositorio : IRepository<Identity.ValidarOperacion, string>
    {
        List<Identity.ValidarOperacion> ValidarOperacion(string idTerceroEmisor, string idTerceroPagador, decimal montoOperacion, int tipoOperacion);
    }
}
