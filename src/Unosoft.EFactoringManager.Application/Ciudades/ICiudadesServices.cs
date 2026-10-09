using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Ciudades.Dto;

namespace Unosoft.EFactoringManager.Ciudades
{
    public interface ICiudadesServices : IApplicationService
    {
        Task<List<CiudadesDto>> ObtenerCiudades(string nomCiudad);
    }
}
