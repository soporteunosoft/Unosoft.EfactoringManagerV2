using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.FirmaElectronica.Dto
{

    public class NotificacionDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }


}
