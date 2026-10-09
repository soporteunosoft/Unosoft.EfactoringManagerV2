using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Dispapeles.Dto
{
    public class ValidacionDIAN
    {
        [JsonProperty("tipoValidacion")]
        public string TipoValidacion { get; set; }
        [JsonProperty("codigoValidacion")]
        public string CodigoValidacion { get; set; }
        [JsonProperty("descripcionValidacion")]
        public string DescripcionValidacion { get; set; }
       
    }
}
