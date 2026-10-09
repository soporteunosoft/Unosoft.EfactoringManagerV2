using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{
    public class Body
    {
        [JsonProperty("InitialColumn")]
        public int InitialColumn { get; set; }
        [JsonProperty("FinalColumn")]
        public int FinalColumn { get; set; }
        [JsonProperty("InitialRow")]
        public int InitialRow { get; set; }
        [JsonProperty("HeaderColor")]
        public string HeaderColor { get; set; }
        [JsonProperty("Fields")]
        public List<BodyField> Fields { get; set; }
    }
}
