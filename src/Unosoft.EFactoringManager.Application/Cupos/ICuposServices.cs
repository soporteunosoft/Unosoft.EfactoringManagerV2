using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Cupos
{
    public interface ICuposServices : IApplicationService
    {
        List<Cupos.Dto.CuposDto> ObtenerCuposPorTercero(Terceros.Dto.TerceroDto tercero);
    
    }
}
