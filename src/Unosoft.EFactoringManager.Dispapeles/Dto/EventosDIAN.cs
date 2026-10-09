using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Dispapeles.Dto
{
    public class EventosDIAN
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cude")]
        public string Cude { get; set; }

        [JsonProperty("fechaEvento")]
        public string FechaEvento { get; set; }

        [JsonProperty("descripcionEvento")]
        public string DescripcionEvento { get; set; }

        [JsonProperty("codigoEvento")]
        public string CodigoEvento { get; set; }
    }
}
