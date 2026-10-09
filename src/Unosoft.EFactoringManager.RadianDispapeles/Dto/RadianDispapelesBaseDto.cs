using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public abstract class RadianDispapelesBaseDto
    {
        [JsonProperty("idEmpresa")]
        public int IdEmpresa { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
