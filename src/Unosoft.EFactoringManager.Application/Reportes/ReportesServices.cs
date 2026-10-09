using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Dto;
using System.Globalization;
using Abp.Authorization;
using Unosoft.EFactoringManager.Reportes.Dto;

namespace Unosoft.EFactoringManager.Reportes
{
    public class ReportesServices : ApplicationService, IReportesServices
    {
        #region Atributos
        ReportesManager _reportesManager;
        #endregion

        public ReportesServices(ReportesManager reportesManager)
        {
            _reportesManager = reportesManager;
        }

        [HttpGet]
        public async Task<IList<Dto.ReporteDto>> ObtReportes(int tipoTercero)
        {
            try
            {
                var reportes = await _reportesManager.ObtReportes(tipoTercero);
                return ObjectMapper.Map<List<Dto.ReporteDto>>(reportes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<Dto.ReporteDto> ObtReportePorId(int id)
        {
            try
            {
                var reporte = await _reportesManager.ObtReportePorId(id);
                return ObjectMapper.Map<Dto.ReporteDto>(reporte);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<List<Identity.RespuestaConsultaReporte>> ConsultarReporte(ReporteFiltro filtros)
        {
            try
            {
                return await _reportesManager.ConsultarReporte(filtros.Id,
                                                         filtros.IdTercero,
                                                         filtros.NumeroTercero,
                                                         filtros.IdOficina,
                                                         filtros.FechaCorte,
                                                         filtros.FechaInicial,
                                                         filtros.FechaFinal,
                                                         "COP");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<byte[]> GetReporteFromPDFXML(ReporteConsultaPDFXLS filtros)
        {
            try
            {
                return await _reportesManager.GenerarReportePDFXML(filtros.Id,
                                                            filtros.IdTercero,
                                                            filtros.IdTerceroInicio,
                                                            filtros.NumeroTercero,
                                                            filtros.IdDocumento,
                                                            filtros.TipoDocumento,
                                                            filtros.IdOficina,
                                                            filtros.FechaInicial,
                                                            filtros.FechaFinal,
                                                            filtros.EsPdf);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
