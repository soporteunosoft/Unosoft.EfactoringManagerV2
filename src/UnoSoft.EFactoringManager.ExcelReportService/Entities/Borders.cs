using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class Borders
    {
        [JsonProperty("Left")]
        public bool Left { get; set; }

        [JsonProperty("Right")]
        public bool Right { get; set; }

        [JsonProperty("Bottom")]
        public bool Bottom { get; set; }

        [JsonProperty("Top")]
        public bool Top { get; set; }
    }
}
