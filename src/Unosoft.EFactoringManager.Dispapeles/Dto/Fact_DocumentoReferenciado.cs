using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class Fact_DocumentoReferenciado
    {
        [JsonProperty("consecutivo")]
        public string Consecutivo { get; set; }
        [JsonProperty("cufeCude")]
        public string CufeCude { get; set; }
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }
        [JsonProperty("codigoTiempoMandato")]
        public string CodigoTiempoMandato { get; set; }
        [JsonProperty("descripcionEvento")]
        public string DescripcionEvento { get; set; }
        [JsonProperty("deudores")]
        public List<Fact_PersonaNaturalJuridica> Deudores { get; set; }
    }
}
