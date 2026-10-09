using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class Font
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Size")]
        public double Size { get; set; }

        [JsonProperty("Bold")]
        public bool Bold { get; set; }
    }
}
