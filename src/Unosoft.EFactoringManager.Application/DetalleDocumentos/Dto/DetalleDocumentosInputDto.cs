using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.DetalleDocumentos.Dto
{
    public class DetalleDocumentosInputDto
    {
        public string IdTercero { get; set; }
        public int NumeroTercero { get; set; }
        public int IdDocOrigen { get; set; }
        public string TipoDocumento { get; set; }
        public int? IdOficina { get; set; }
        public int? IdDocumento { get; set; }
    }
}
