using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class ReportSection
    { 

        [JsonProperty("BackgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("Fields")]
        public List<SectionField> Fields { get; set; }
    }
}
