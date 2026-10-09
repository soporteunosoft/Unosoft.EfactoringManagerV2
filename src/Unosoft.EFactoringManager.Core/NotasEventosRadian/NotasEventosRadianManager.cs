using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.NotasEventosRadian
{
    public class NotasEventosRadianMaanger : IDomainService
    {
        #region Atributos
        private readonly INotasEventosRadianRepositorio _notasEventosRadianRepositorio;
        #endregion

        public NotasEventosRadianMaanger(INotasEventosRadianRepositorio notasEventosRadianRepositorio)
        {
            _notasEventosRadianRepositorio = notasEventosRadianRepositorio;
        }

        //public Identity.TiposOperaciones ObtenerTipoOperacion(int tipoOperacion)
        //{
        //    return _tiposOperacionesRepositorio.GetAll().Where(x => x.TipoOperacion == tipoOperacion).FirstOrDefault();
        //}
    }
}
