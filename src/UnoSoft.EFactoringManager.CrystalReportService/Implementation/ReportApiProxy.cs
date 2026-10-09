using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnoSoft.EFactoringManager.CrystalReportService.Service;


namespace UnoSoft.EFactoringManager.CrystalReportService.Implementation
{
    
    public sealed class ReportApiProxy : IReportApiProxy
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReportApiProxy> _logger;
        

        public ReportApiProxy(HttpClient httpClient, ILogger<ReportApiProxy> logger   )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<byte[]> GenerateReportAsync(
            ReportRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/reports",
                request,
                new JsonSerializerOptions(JsonSerializerDefaults.Web),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Error calling report API: {Error}", error);

                throw new HttpRequestException(
                    $"Report API error ({response.StatusCode}): {error}");
            }

            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
               

        public async Task<byte[]> GenerateReportAsync<T>(string reportName, IEnumerable<T> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var jsonData = JsonConvert.SerializeObject(data);

            ReportRequest reportRequest = new()
            {
                ReportName = reportName,
                Data = jsonData,
                Format = ReportFormat.Pdf
            };

            return await GenerateReportAsync(reportRequest);
        }

    }
}
