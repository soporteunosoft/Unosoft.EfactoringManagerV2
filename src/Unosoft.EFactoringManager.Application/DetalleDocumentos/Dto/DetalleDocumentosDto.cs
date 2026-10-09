using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Newtonsoft.Json;

namespace Unosoft.EFactoringManager.DetalleDocumentos.Dto
{
    [AutoMap(typeof(Identity.DetalleDocumentos))]
    public class DetalleDocumentosDto
    {
        [JsonProperty("idDetDoc")]
        public int Id { get; set; }
        //public int NumeroTercero { get; set; }
        public int IdDocLinea { get; set; }
        public int NumeroTercero { get; set; }
        public string RazonSocial { get; set; }
        public int? TipoTercero { get; set; }
        public int? IdDetalleTitulo { get; set; }
        public string TipoDocumento { get; set; }
        public int? IdOficina { get; set; }
        public int? IdDocumento { get; set; }
        public DateTime? FecDocumento { get; set; }
        public string HoraDocumento { get; set; }
        public DateTime? FecRenovacion { get; set; }
        public string DirDocumento { get; set; }
        public string DirDocumentoFirmado { get; set; }
        public bool? Obligatorio { get; set; }
        public bool? Validar { get; set; }
        public bool? Firma { get; set; }
        public string IdUsrMod { get; set; }
        public Identity.Documentos Documentos { get; set; }
    }
}
