using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.TiposTitulo
{
    public interface ITiposTituloServices : IApplicationService
    {
        Task<List<Identity.TiposTitulo>> ObtenerTiposTitulo();
    }
}
