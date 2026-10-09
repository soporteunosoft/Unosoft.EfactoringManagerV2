using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.DetalleDocumentos.DTO
{
    public class DocumentosIniciales
    {
        public Identity.DetalleDocumentos Documento { get; set; }
        public bool SinFirma { get; set; }
    }
}
