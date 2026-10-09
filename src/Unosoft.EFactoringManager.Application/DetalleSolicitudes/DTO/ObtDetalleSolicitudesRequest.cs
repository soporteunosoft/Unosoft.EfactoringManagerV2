using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.DetalleSolicitudes.DTO
{
    public class ObtDetalleSolicitudesRequest
    {
        public string TipoDocumento { get; set; }
        public int Producto { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int EstadoTitulo { get; set; }
        public int IdSolicitud { get; set; }
        public string IdTitulo { get; set; }
    }
}
