using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.Menu
{
    public class MenuManager : IDomainService
    {
        #region Atributos
        private readonly IMenuRepositorio _menuRepositorio;
        private List<Identity.Menu> _menu;
        private List<Identity.MenuItem> arbolMenu = new List<Identity.MenuItem>();
        #endregion

        public MenuManager(IMenuRepositorio menuRepositorio)
        {
            _menuRepositorio = menuRepositorio;
        }

        private void GenerarMenuItem(int idPadre, Identity.MenuItem menuItemPadre)
        {


            try
            {
                foreach (Identity.Menu menu in _menu.ToList().FindAll(a => a.IdPadre == idPadre))
                {
                    Identity.MenuItem menuItem = new Identity.MenuItem();
                    menuItem.name = menu.NomMenu;
                    menuItem.icon = menu.Icono;
                    menuItem.route = menu.Url;
                    menuItem.permissionName = "";


                    if (_menu.ToList().Count(a => a.IdPadre == menu.IdMenu) > 0)
                    {
                        menuItem.Items = new List<Identity.MenuItem>();
                        GenerarMenuItem(menu.IdMenu, menuItem);
                        arbolMenu.Add(menuItem);
                    }
                    else
                    {
                        if (menuItemPadre == null)
                        {
                            arbolMenu.Add(menuItem);
                        }
                        else
                        {

                            menuItemPadre.Items.Add(menuItem);
                        }

                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string LoadTreeMenu(int IdFather, string TreeMenu)
        {


            try
            {

                foreach (Identity.Menu menu in _menu.ToList().FindAll(a => a.IdPadre == IdFather))
                {
                    TreeMenu = TreeMenu + "<li>" + "<a href=\"" + menu.Url + "\" class=\"dropdown-toggle\" data-hover=\"dropdown\" data-toggle=\"dropdown\">" + "<span style=\"font-size:16px;\" class=\"pull-right hidden-xs showopacity " + menu.Icono + "\"></span>" + menu.NomMenu + "</a>";
                    if (_menu.ToList().Count(a => a.IdPadre == menu.IdMenu) > 0)
                    {
                        TreeMenu = TreeMenu + "<ul class=\"dropdown-menu forAnimate\" role =\"menu\" >";
                        TreeMenu = LoadTreeMenu(menu.IdMenu, TreeMenu);
                        TreeMenu = TreeMenu + "</ul></li>";
                    }
                    else
                    {
                        TreeMenu = TreeMenu + "</li>";
                    }


                }
                return TreeMenu;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<List<Identity.MenuItem>> ObtenerMenu(string IdTercero, int IdOficina, string Usuario)
        {
            _menu = await _menuRepositorio.ObtenerMenuTerceroAsync(IdTercero, IdOficina, Usuario);
            GenerarMenuItem(0, null);
            return arbolMenu;
        }


    }
}
