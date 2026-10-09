using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Terceros;
using UnoSoft.EFactoringManager.CrystalReportService;
using UnoSoft.EFactoringManager.CrystalReportService.Service;
using UnoSoft.EFactoringManager.ExcelReportService.BL;
using UnoSoft.EFactoringManager.ExcelReportService.Utils;
using UnoSoft.ExcelReportService.BL.Entities;

namespace Unosoft.EFactoringManager.Reportes
{
    public class ReportesManager : IDomainService
    {
        #region Atributos
        private readonly IReportesRepositorio _reportesRepositorio;
        private readonly TercerosManager _terceroManager;
        private readonly AppSettings _appSettings;
        private readonly IReportApiProxy _reportApiProxy;
        private readonly ILogger<ReportesManager> _logger;
        #endregion

        public ReportesManager(IReportesRepositorio reportesRepositorio,
                                TercerosManager terceroManager,
                                IOptions<AppSettings> appSettings,
                                ILogger<ReportesManager> logger,
                                IReportApiProxy reportApiProxy)
        {
            _reportesRepositorio = reportesRepositorio;
            _terceroManager = terceroManager;
            _appSettings = appSettings.Value;
            _logger = logger;
            _reportApiProxy = reportApiProxy;
        }

        public async Task<IList<Identity.Reportes>> ObtReportes(int tipoTercero)
        {
            return await _reportesRepositorio.GetAll().Where(a => (a.TipoTercero == tipoTercero || a.TipoTercero == 0 || _appSettings.Reportes.Contains(tipoTercero) /*tipoTercero == 5 || tipoTercero == 4 || tipoTercero == 8*/) && a.Activa).ToListAsync();
        }

        public async Task<Identity.Reportes> ObtReportePorId(int id)
        {
            return await _reportesRepositorio.GetAll().Where(a => a.Id == id).FirstOrDefaultAsync();
        }



        public async Task<byte[]> GenerarReportePDFXML(int id,
                                           string idTercero,
                                           string idTerceroInicio,
                                           int numeroTercero,
                                           int idDocumento,
                                           string tipoDocumento,
                                           int idOficina,
                                           DateTime? fechaInicial,
                                           DateTime? fechaFinal,
                                           bool esPdf)
        {

            var reporte = await _reportesRepositorio.GetAll().Where(a => a.Id == id && a.Activa).FirstOrDefaultAsync();

            if (reporte == null)
                return null;


            if (fechaInicial == null)
                fechaInicial = DateTime.Now;

            if (fechaFinal == null)
                fechaFinal = DateTime.Now;

            var DirectoryRoot = $"{AppDomain.CurrentDomain.BaseDirectory}ReportesCrystal";


            if (string.IsNullOrEmpty(DirectoryRoot))
                throw new Exception("No se encuentra configurado la ruta de la aplicación en el archivo de configuración");


            string reporteSinExtension = Path.GetFileNameWithoutExtension(reporte.DirReporte);
            string fullPath = Path.Combine(DirectoryRoot, reporteSinExtension + ".json");


            var consultaReporte = reporte.ConsultaReporte.Replace("IDOFICINA", idOficina.ToString())
                                        .Replace("IDDOCUMENTO", idDocumento.ToString())
                                        .Replace("TIPODOCUMENTO", tipoDocumento)
                                        .Replace("IDTERCEROINICIO", idTerceroInicio)
                                        .Replace("IDTERCERO", idTercero)
                                        .Replace("NUMEROTERCERO", numeroTercero.ToString())
                                        .Replace("OPCIONREPORTE", esPdf ? "1" : "2")
                                        .Replace("FECHAINICIAL", fechaInicial != null ? $"{fechaInicial:dd/MM/yyyy}" : "")
                                        .Replace("FECHAFINAL", fechaFinal != null ? $"{fechaFinal:dd/MM/yyyy}" : "");








            if (!esPdf && File.Exists(fullPath))
            {
                var data = _reportesRepositorio.GetDataReporte(consultaReporte);
                return GenerarReporteExcel(reporte, data, fullPath);
            }
            else
            {
                //fullPath = Path.Combine(DirectoryRoot, reporte.DirReporte);
                Dictionary<string, string> parametros = new Dictionary<string, string>
                {
                    { "FechaInicial", fechaInicial.Value.ToString("dd/MM/yyyy") },
                    { "FechaFinal", fechaFinal.Value.ToString("dd/MM/yyyy") },
                    { "IdOficina", idOficina.ToString() },
                    { "IdDocumento", idDocumento.ToString() },
                    { "TipoDocumento", tipoDocumento },
                    { "IdTerceroInicio", idTerceroInicio },
                    { "IdTercero", idTercero }
                };
                return await GenerarReportePdf(reporte, consultaReporte, parametros, esPdf);
            }



        }

        private async Task<byte[]> GenerarReportePdf(Identity.Reportes reporte, string query, Dictionary<string, string> parametros, bool esPdf)
        {
            /*
            ReportDocument rpt = new ReportDocument();

            rpt.Load(reportePath, OpenReportMethod.OpenReportByTempCopy);

            rpt.SetDataSource(data);

            CargarParametrosReporte(rpt, reporte, fechaInicial.Value, fechaFinal.Value, idOficina == 0 ? 1 : idOficina);

            System.IO.Stream stream = rpt.ExportToStream(esPdf ? ExportFormatType.PortableDocFormat : ExportFormatType.Excel);

            int datalen = (int)stream.Length;

            byte[] filebytes = new byte[datalen];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(filebytes, 0, datalen);

            stream.Close();


            return filebytes;*/

            ReportRequest reportRequest = new()
            {
                ReportName = reporte.DirReporte,
                Query = query,
                QueryParameters = reporte.ConsultaParametrosReporte,
                Format = esPdf ? ReportFormat.Pdf : ReportFormat.Excel,
                Parameters = parametros
            };

            return await _reportApiProxy.GenerateReportAsync(reportRequest);
        }

        private byte[] GenerarReporteExcel(Identity.Reportes reporte, DataSet data, string path)
        {
            string json = File.ReadAllText(path);
            Report report = JsonConvert.DeserializeObject<Report>(json);

            if (data.Tables.Count <= 0)
            {
                throw new Exception("La consulta no delvovió datos");
            }

            List<dynamic> dynamicData = DataSetConvert.ConvertToDynamicList(data.Tables[0]);
            ExcelReportGeneratorService excelReportGeneratorService = new ExcelReportGeneratorService();
            return excelReportGeneratorService.GenerateReport(report, dynamicData);
        }

        public async Task<List<Identity.RespuestaConsultaReporte>> ConsultarReporte(int id,
                                                                        string idTercero,
                                                                        int numeroTercero,
                                                                        int idOficina,
                                                                        DateTime? fechaCorte,
                                                                        DateTime? fechaInicial,
                                                                        DateTime? fechaFinal,
                                                                        string tipoDocumento)
        {
            var reporte = await _reportesRepositorio.GetAll().Where(a => a.Id == id).FirstOrDefaultAsync();

            if (reporte == null)
                return null;

            //var tercero = _terceroManager.ObtenerTerceroPorId(numeroTercero);

            //if (tercero == null)
            //    return null;


            if (fechaInicial == null)
                fechaInicial = DateTime.Now;

            if (fechaFinal == null)
                fechaFinal = DateTime.Now;




            var query = reporte.Consulta.Replace("IDOFICINA", idOficina.ToString())
                                        .Replace("TIPOTERCERO", reporte.TipoTercero?.ToString())
                                        .Replace("FECHACORTE", fechaCorte != null ? $"{fechaCorte:YYYYMMDD}" : "")
                                        .Replace("FECHAINICIAL", fechaInicial != null ? $"{fechaInicial:yyyyMMdd}" : "")
                                        .Replace("FECHAFINAL", fechaFinal != null ? $"{fechaFinal:yyyyMMdd}" : "")
                                        .Replace("NUMEROTERCERO", numeroTercero.ToString())
                                        .Replace("IDTERCERO", idTercero.ToString())
                                        .Replace("TIPODOCUMENTO", tipoDocumento != null ? tipoDocumento.ToString() : "");
            //.Replace("CLASEDOCUMENTO", (reporte.ClaseDocumento != null ? reporte.ClaseDocumento?.ToString() : "0"))


            var result = _reportesRepositorio.ConsultarReporte(query);

            return result;

        }

        /*
        private void CargarParametrosReporte(ReportDocument rpt, Identity.Reportes reporte, DateTime fechaInicial, DateTime fechaFinal, int idOficina)
        {
            if (!string.IsNullOrEmpty(reporte.ConsultaParametrosReporte))
            {
               
                var dataParametros = _reportesRepositorio.GetDataReporte(reporte.ConsultaParametrosReporte.Replace("IDOFICINA", idOficina.ToString()));

               
                Dictionary<string, string> parametros = new Dictionary<string, string>();

                //Se carga el diccionario con la iformacion de los parametros
               
                    if (dataParametros != null && dataParametros.Tables.Count > 0 && dataParametros.Tables[0].Rows.Count>0)
                    {

                        DataRow row = dataParametros.Tables[0].Rows[0];
                        for (int i = 0; i < row.Table.Columns.Count; i++)
                        {                            
                          parametros.Add(row.Table.Columns[i].ColumnName, row[row.Table.Columns[i].ColumnName].ToString());                            
                        }

                    }

                parametros.Add("FechaInicial", fechaInicial.ToString("dd/MM/yyyy"));
                parametros.Add("FechaFinal", fechaFinal.ToString("dd/MM/yyyy"));
                SetParameters(rpt, parametros);

            }

        }


        private void SetParameters(ReportDocument rpt, Dictionary<string, string> parametros)
        {
            /*
            foreach (ParameterFieldDefinition parameter in rpt.DataDefinition.ParameterFields)
            {
                try
                {

                    // Now get the current value for the parameter
                    CrystalDecisions.Shared.ParameterValues currentValues = parameter.CurrentValues;
                    //currentValues.Clear();

                    // Create a value object for Crystal reports and assign the specified value.
                    CrystalDecisions.Shared.ParameterDiscreteValue newValue = new CrystalDecisions.Shared.ParameterDiscreteValue();

                    if (parametros.ContainsKey(parameter.Name))
                    {
                        currentValues.Clear();
                        newValue.Value = parametros[parameter.Name];


                        // Now add the new value to the values collection and apply the 
                        // collection to the report.
                        currentValues.Add(newValue);
                        // parameter.CurrentValues = currentValues;
                        parameter.ApplyCurrentValues(currentValues); //OJO ARLEY
                    }
                }
                catch
                {
                    // Ignore any errors
                }
            }
            
        }*/


    }
}
