using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.DetalleSolicitudes.DTO
{
    public class DetalleTituloDTO
    {
        public int IdSolicitud { get; set; }
        public int NumeroTercero { get; set; }
        public string TipoDocumento { get; set; }
        public string IdTitulo { get; set; }
        public int TipoTitulo { get; set; }
        public int NumeroPagador { get; set; }
        public decimal? VrNetoTitulo { get; set; }
        public DateTime FecInicial { get; set; }
        public DateTime? FecFinal { get; set; }
        public DateTime? FecProbable { get; set; }
        public int? DiasOperacion { get; set; }
        public Identity.Terceros Pagador { get; set; }
        public Identity.Terceros Tercero { get; set; }
        public string EstadoTitulo { get; set; }
    }
}
