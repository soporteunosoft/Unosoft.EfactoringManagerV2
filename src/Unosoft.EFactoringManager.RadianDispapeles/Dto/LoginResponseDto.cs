using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class LoginResponseDto
    {
        [JsonProperty("codigoRespuesta")]
        public int CodigoRespuesta { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("dateStartToken")]
        public DateTime DateStartToken { get; set; }
        [JsonProperty("dateExpireToken")]
        public DateTime DateExpireToken { get; set; }

    }
}
