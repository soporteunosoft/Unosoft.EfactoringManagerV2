using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposNotasRadian
{
    public class TiposNotasRadianManager : IDomainService
    {
        #region Atributos
        private readonly ITiposNotasRadianRepositorio _tiposNotasRadianRepoisitorio;
        #endregion

        public TiposNotasRadianManager(ITiposNotasRadianRepositorio tiposNotasRadianRepositorio)
        {
            _tiposNotasRadianRepoisitorio = tiposNotasRadianRepositorio;
        }

    //    public Identity.TiposOperaciones ObtenerTipoOperacion(int tipoOperacion)
    //    {
    //        return _tiposOperacionesRepositorio.GetAll().Where(x => x.TipoOperacion == tipoOperacion).FirstOrDefault();
    //    }
    }
}
