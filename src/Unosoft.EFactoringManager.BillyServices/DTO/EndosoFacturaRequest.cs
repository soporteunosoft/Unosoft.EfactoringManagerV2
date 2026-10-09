using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BillyServices.DTO
{
    public class EndosoFacturaRequest
    {
        [JsonProperty("cufe")]
        public string Cufe { get; set; }
        [JsonProperty("market")]
        public string Market { get; set; }
        [JsonProperty("annualPercentageRate")]
        public double AnnualPercentageRate { get; set; }
    }
}
