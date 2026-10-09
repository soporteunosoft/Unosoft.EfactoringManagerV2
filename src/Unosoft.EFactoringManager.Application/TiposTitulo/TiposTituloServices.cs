using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Abp.Authorization;

namespace Unosoft.EFactoringManager.TiposTitulo
{
    [AbpAuthorize]
    public class TiposTituloServices : ApplicationService, ITiposTituloServices
    {
        #region Atributos
        TiposTituloManager _tiposTituloManager;
        #endregion

        public TiposTituloServices(TiposTituloManager tiposTituloManager)
        {
            _tiposTituloManager = tiposTituloManager;
        }

        [HttpGet]
        public async Task<List<Identity.TiposTitulo>> ObtenerTiposTitulo()
        {
            try
            {
                return await _tiposTituloManager.ObtenerTiposTitulo();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
