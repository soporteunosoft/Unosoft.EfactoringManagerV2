using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class EventosDianRequestDto : RadianDispapelesBaseDto
    {
        [JsonProperty("cufe")]
        public string Cufe { get; set; }

    }
}
