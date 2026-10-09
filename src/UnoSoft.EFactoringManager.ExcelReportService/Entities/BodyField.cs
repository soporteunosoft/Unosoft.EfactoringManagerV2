using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class BodyField : ReportColumn
    {
        [JsonProperty("Caption")]
        public string Caption { get; set; }
        [JsonProperty("HeaderStyle")]
        public ReportStyle HeaderStyle { get; set; }
        [JsonProperty("HeaderColor")]
        public string HeaderColor { get; set; }
        [JsonProperty("HeaderHeight")]
        public int HeaderHeight { get; set; }

        [JsonProperty("Type")]
        public FieldType Type { get; set; }
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [JsonProperty("Format")]
        public string Format { get; set; }


    }
}
