using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.TiposTasaInteres
{
    public class TiposTasaInteresManager : IDomainService
    {
        #region Atributos
        private readonly ITiposTasaInteresRepositorio _tiposTasaInteresRepositorio;
        #endregion
        public TiposTasaInteresManager(ITiposTasaInteresRepositorio tiposTasaInteresRepositorio)
        {
            _tiposTasaInteresRepositorio = tiposTasaInteresRepositorio;
        }

        public async Task<Identity.TiposTasaInteres> ObtenerTiposTasaInteres(int idTasaInteres)
        {
            return await _tiposTasaInteresRepositorio.GetAll().Where(a => a.Id == idTasaInteres).FirstOrDefaultAsync();
        }
    }
}
