using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Menu.Dto;

namespace Unosoft.EFactoringManager.Menu
{
    public interface IMenuServices : IApplicationService
    {

        Task<List<Identity.MenuItem>> ObtenerMenu(TerceroMenuDto TerceroDto);
    }
}
