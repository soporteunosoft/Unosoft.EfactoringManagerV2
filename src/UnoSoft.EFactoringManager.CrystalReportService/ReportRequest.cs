using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.CrystalReportService
{

    public enum ReportFormat
    {
        Pdf,
        Excel
    }

    public class ReportRequest
    {
        public string ReportName { get; set; }

        public string Query { get; set; }
        public string QueryParameters { get; set; }
        public string ReportParameters { get; set; }

        public string OutputFileName { get; set; }

        public ReportFormat Format { get; set; } = ReportFormat.Pdf;

        public string Data { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
            = new Dictionary<string, string>();

    }
}
