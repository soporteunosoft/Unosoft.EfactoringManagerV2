using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles.Dto
{
    public class CrearEventoRequestDto : RadianDispapelesBaseDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("codigoEvento")]
        public string CodigoEvento { get; set; }
        [JsonProperty("tipoOperacion")]
        public string TipoOperacion { get; set; }
        [JsonProperty("referenciaTipoOperacion")]
        public string ReferenciaTipoOperacion { get; set; }
        [JsonProperty("moneda")]
        public string Mondeda { get; set; }
        [JsonProperty("codigoCantidadDocumentos")]
        public string CodigoCantidadDocumentos { get; set; }
        [JsonProperty("archivosAdjuntos")]
        public List<Fact_ArchivoAdjunto> ArchivosAdjuntos { get; set; }
        [JsonProperty("facultades")]
        public string Facultades { get; set; }
        [JsonProperty("descripcionFacultades")]
        public string DescripcionFacultades { get; set; }
        [JsonProperty("notas")]
        public List<string> Notas { get; set; }
        [JsonProperty("fechaActuarMandatario")]
        public string FechaActuarMandatario { get; set; }
        [JsonProperty("documentosReferenciado")]
        public List<Fact_DocumentoReferenciado> DocumentosReferenciado { get; set; }
        [JsonProperty("generadorEvento")]
        public List<Fact_PersonaNaturalJuridica> GeneradorEvento { get; set; }
        [JsonProperty("mandante")]
        public List<Fact_PersonaNaturalJuridica> Mandante { get; set; }
        [JsonProperty("mandatario")]
        public List<Fact_PersonaNaturalJuridica> Mandatario { get; set; }
        [JsonProperty("informacionCampoValor")]
        public List<Fact_ObjetoComun> InformacionCampoValor { get; set; }
    }
}
