using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.DetalleSolicitudes.DTO
{
    public class InsertDetalleSolicitudes
    {
        public string TipoDocumento { get; set; }
        public int IdOficina { get; set; }
        public int IdSolicitud { get; set; }
        public string IdTitulo { get; set; }
        public Identity.Terceros Terceros { get; set; }
    }
}
