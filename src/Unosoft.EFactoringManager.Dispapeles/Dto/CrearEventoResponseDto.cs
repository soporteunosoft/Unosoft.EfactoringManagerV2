using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Dispapeles.Dto
{
    public class CrearEventoResponseDto
    {
        [JsonProperty("codigoRespuesta")]
        public string CodigoRespuesta { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("tiempoProcesamiento")]
        public string TiempoProcesamiento { get; set; }
        [JsonProperty("tipoObjeto")]
        public string TipoObjeto { get; set; }
        [JsonProperty("objeto")]
        public string Objeto { get; set; }
        [JsonProperty("validaciones")]
        public List<ValidacionDIAN> Validaciones { get; set; }
      
    }
}
