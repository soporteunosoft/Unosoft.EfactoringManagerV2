using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;
using Unosoft.EFactoringManager.Menu;
using Unosoft.EFactoringManager.Menu.Dto;


namespace Unosoft.EFactoringManager.Menu
{
    public class MenuServices : ApplicationService, IMenuServices
    {
        #region Atributos
        MenuManager _menuManager;
        #endregion

        public MenuServices(MenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        [HttpPost]
        public async Task<List<Identity.MenuItem>> ObtenerMenu(TerceroMenuDto TerceroDto)
        {
            return await _menuManager.ObtenerMenu(TerceroDto.IdTercero, TerceroDto.IdOficina, TerceroDto.Usuario);
        }

        
    }
}
