using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Documentos.Dto
{
    public class DocumentoNewDto
    {
        public int NumeroTercero { get; set; }
        public string IdTercero { get; set; }
        public int TipoDocumento { get; set; }
        public int TipoLinea { get; set; }
        public int? TipoFirma { get; set; }
        public int? TipoFirmaElectronica { get; set; }
        public int PlazoDias { get; set; }
        public int OrigenDoc { get; set; }
        public bool Obligatorio { get; set; }
    }
}
