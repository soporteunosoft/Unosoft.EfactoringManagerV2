using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Dispapeles.Dto
{
    public class ConsultaEventoDianResponseDto
    {
        [JsonProperty("codigoRespuesta")]
        public int CodigoRespuesta { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("tiempoProcesamiento")]
        public int TiempoProcesamiento { get; set; }

        [JsonProperty("validaciones")]
        public List<ValidacionDIAN> Validaciones { get; set; }

        [JsonProperty("eventos")]
        public List<EventosDIAN> Eventos { get; set; }
    }
}
