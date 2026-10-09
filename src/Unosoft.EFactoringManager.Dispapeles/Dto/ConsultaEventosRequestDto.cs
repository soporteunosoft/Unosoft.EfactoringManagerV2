using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class ConsultaEventosRequestDto : RadianDispapelesBaseDto
    {
        [JsonProperty("idEvento")]
        public string IdEvento { get; set; }
        [JsonProperty("cudeEvento")]
        public string CudeEvento { get; set; }
        [JsonProperty("documentoGeneradorEvento")]
        public string DocumentoGeneradorEvento { get; set; }
        [JsonProperty("tipoDocGeneradorEvento")]
        public string TipoDocGeneradorEvento { get; set; }
        [JsonProperty("consecutivoDocRef")]
        public string ConsecutivoDocRef { get; set; }
        [JsonProperty("cudeDocRef")]
        public string CudeDocRef { get; set; }
        [JsonProperty("tipoDocRef")]
        public string TipoDocRef { get; set; }
        [JsonProperty("codigoEvento")]
        public string CodigoEvento { get; set; }
        [JsonProperty("tipoOperacion")]
        public string TipoOperacion { get; set; }
        [JsonProperty("estado")]
        public string Estado { get; set; }
        [JsonProperty("origen")]
        public string Origen { get; set; }
        [JsonProperty("base64")]
        public string Base64 { get; set; }


    }
}
