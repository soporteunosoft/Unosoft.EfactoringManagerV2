using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public abstract class ReportStyle
    {

        [JsonProperty("Alignment")]
        public ReportAlignment Alignment { get; set; }

        [JsonProperty("Borders")]
        public Borders Borders { get; set; }

        [JsonProperty("Font")]
        public Font Font { get; set; }
        [JsonProperty("CellColor")]
        public string CellColor { get; set; }
    }


    public enum ReportAlignment
    {
        Left = 1,
        Center = 2,
        Right = 3
    }
}
