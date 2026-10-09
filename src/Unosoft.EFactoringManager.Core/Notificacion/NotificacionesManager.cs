using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.Notificacion
{
    public  class NotificacionesManager : IDomainService
    {
        private readonly INotificacionesRepositorio  _notificacionesRepositorio;

        public NotificacionesManager(INotificacionesRepositorio notificacionesRepositorio)
        {
            _notificacionesRepositorio = notificacionesRepositorio;
        }

        public void Notificar(Identity.Notificacion notificacion)
        {
            _notificacionesRepositorio.Create(notificacion);
        }
    }
}
