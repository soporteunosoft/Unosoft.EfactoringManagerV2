using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class ReportColumn : ReportStyle
    {
        [JsonProperty("Width")]
        public double Width { get; set; }

        [JsonProperty("Height")]
        public int Height { get; set; }



    }
}
