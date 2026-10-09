using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposTitulo
{
    public class TiposTituloManager : IDomainService
    {
        #region Atributos
        private readonly ITiposTituloRepositorio _tiposTitulosRepositorio;
        #endregion

        public TiposTituloManager(ITiposTituloRepositorio tiposTitulosRepositorio)
        {
            _tiposTitulosRepositorio = tiposTitulosRepositorio;
        }

        public async Task<List<Identity.TiposTitulo>> ObtenerTiposTitulo()
        {
            return await _tiposTitulosRepositorio.GetAll().ToListAsync();
        }
    }
}
