using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Reportes.Dto;

namespace Unosoft.EFactoringManager.Reportes
{
    public interface IReportesServices : IApplicationService
    {
        Task<IList<Dto.ReporteDto>> ObtReportes(int tipoTercero);
        Task<Dto.ReporteDto> ObtReportePorId(int id);
        [HttpPost]
        Task<byte[]> GetReporteFromPDFXML(ReporteConsultaPDFXLS filtros);
    }
}
