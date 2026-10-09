using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class Report
    {
        [JsonProperty("Name")]
        public string Name { get; set; } 
        
        [JsonProperty("Font")]
        public Font Font { get; set; }

        [JsonProperty("Body")]
        public Body Body { get; set; }

        [JsonProperty("Header")]
        public ReportSection Header { get; set; }
        [JsonProperty("Footer")]
        public ReportSection Footer { get; set; }

        [JsonProperty("Data")]
        public List<Object> Data  { get; set; }            

    }
}
