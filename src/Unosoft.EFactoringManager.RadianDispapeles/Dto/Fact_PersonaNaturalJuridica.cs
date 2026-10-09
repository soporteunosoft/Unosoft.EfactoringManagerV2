using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
   public class Fact_PersonaNaturalJuridica
    {
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }
        [JsonProperty("numeroDocumento")]
        public string NumeroDocumento { get; set; }
        [JsonProperty("nombreCompleto")]
        public string NombreCompleto { get; set; }
        [JsonProperty("tipoPersona")]
        public string TipoPersona { get; set; }
        [JsonProperty("tipoFactor")]
        public string TipoFactor { get; set; }
        [JsonProperty("descripcionFactor")]
        public string DescripcionFactor { get; set; }
        [JsonProperty("digitoVerificacion")]
        public string DigitoVerificacion { get; set; }
        [JsonProperty("monto")]
        public double Monto { get; set; }
        [JsonProperty("representanteLegal")]
        public List<Fact_RepresentanteLegal> RepresentanteLegal { get; set; }
    }
}
