using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class Fact_RepresentanteLegal
    {
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }
        [JsonProperty("numeroDocumento")]
        public string NumeroDocumento { get; set; }
        [JsonProperty("nombres")]
        public string Nombres { get; set; }
        [JsonProperty("apellidos")]
        public string Apellidos { get; set; }
        [JsonProperty("tipoSujeto")]
        public string TipoSujeto { get; set; }
        [JsonProperty("digitoVerificacion")]
        public string DigitoVerificacion { get; set; }
        [JsonProperty("cargo")]
        public string Cargo { get; set; }
        [JsonProperty("area")]
        public string AreaOrdCliente { get; set; }
        [JsonProperty("nacionalidad")]
        public string Nacionalidad { get; set; }
    }
}
