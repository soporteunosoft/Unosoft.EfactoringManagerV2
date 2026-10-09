using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposOperaciones
{
    public class TiposOperacionesManager : IDomainService
    {
        #region Atributos
        private readonly ITiposOperacionesRepositorio _tiposOperacionesRepositorio;
        #endregion

        public TiposOperacionesManager(ITiposOperacionesRepositorio tiposOperacionesRepositorio)
        {
            _tiposOperacionesRepositorio = tiposOperacionesRepositorio;
        }

        public async Task<Identity.TiposOperaciones> ObtenerTipoOperacion(int tipoOperacion)
        {
            return await _tiposOperacionesRepositorio.GetAll().Where(x => x.TipoOperacion == tipoOperacion).FirstOrDefaultAsync();
        }

        public List<Identity.TiposOperaciones> ObtenerTipoOperacion()
        {
            return _tiposOperacionesRepositorio.GetList();
        }
    }
}
