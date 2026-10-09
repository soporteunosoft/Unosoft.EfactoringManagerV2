using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BillyServices.DTO
{
    public class CargaFacturaRequest
    {
        [JsonProperty("cufe")]
        public string Cufe { get; set; }
    }
}
