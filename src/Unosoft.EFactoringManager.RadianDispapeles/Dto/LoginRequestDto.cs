using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class LoginRequestDto
    {
        [JsonProperty("idEmpresa")]
        public int IdEmpresa { get; set; }
        [JsonProperty("usuario")]
        public string Usuario { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
