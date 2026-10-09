using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.CrystalReportService.Service
{
    public interface IReportApiProxy
    {
        Task<byte[]> GenerateReportAsync(ReportRequest request, CancellationToken cancellationToken = default);
        Task<byte[]> GenerateReportAsync<T>(string reportName, IEnumerable<T> data);
    }
}
