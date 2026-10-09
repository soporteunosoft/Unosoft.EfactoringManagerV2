using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Pronus.Dto;

namespace Unosoft.EFactoringManager.Pronus
{
    public interface IPronusServices : IApplicationService
    {
        List<Identity.ConsolidadoOperaciones> ObtConsolidadoOperacionesPeriodo(ResumenVigentesDto ResumenVigentesDto);

        List<Identity.HistorialCuposPagadores> ObtHistorialCuposPagadores(ResumenVigentesDto ResumenVigentesDto);

        List<Identity.ResumenRecaudos> ObtResumenRecaudosClientes(ResumenVigentesDto ResumenVigentesDto);

        List<Identity.DetalleRecaudos> ObtDetalleRecaudosClientes(ResumenVigentesDto ResumenVigentesDto);
    }
}
