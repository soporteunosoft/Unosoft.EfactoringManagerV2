using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface INotificacionesRepositorio :  IRepository<Identity.Notificacion, int>
    {

        void Create(Identity.Notificacion notificacion);

    }
}
