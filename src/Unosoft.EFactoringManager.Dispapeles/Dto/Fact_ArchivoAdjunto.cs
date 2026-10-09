using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class Fact_ArchivoAdjunto
    {
        [JsonProperty("numeroDocumento")]
        public string NumeroDocumento { get; set; }
        [JsonProperty("archivoBase64")]
        public string ArchivoBase64 { get; set; }
        [JsonProperty("nombreArchivo")]
        public string NombreArchivo { get; set; }
        [JsonProperty("nombreDocumento")]
        public string NombreDocumento { get; set; }
        [JsonProperty("descripcionDocumento")]
        public string DescripcionDocumento { get; set; }
        [JsonProperty("fecha")]
        public string Fecha { get; set; }
        [JsonProperty("notarios")]
        public List<Fact_RepresentanteLegal> Notarios { get; set; }
    }
}
