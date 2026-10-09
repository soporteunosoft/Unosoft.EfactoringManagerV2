using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class Fact_ObjetoComun
    {
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("valor")]
        public string Valor { get; set; }
    }
}
