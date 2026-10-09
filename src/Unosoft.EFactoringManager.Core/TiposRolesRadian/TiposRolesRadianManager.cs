using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposRolesRadian
{
    public class TiposRolesRadianManager : IDomainService
    {
        #region Atributos
        private readonly ITiposRolesRadianRepositorio _tiposRolesRadianRepositorio;
        #endregion

        public TiposRolesRadianManager(ITiposRolesRadianRepositorio tiposRolesRadianRepositorio)
        {
            _tiposRolesRadianRepositorio = tiposRolesRadianRepositorio;
        }

  
    }
}
