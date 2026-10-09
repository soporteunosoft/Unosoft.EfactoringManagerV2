using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.DetalleSolicitudes.DTO
{
    public class DetalleSolicitudDTO
    {
        public string TipoDocumento { get; set; }
        public int IdSolicitud { get; set; }
        public string IdTitulo { get; set; }
        public int NumeroTercero { get; set; }
        public DateTime? FecSolicitud { get; set; }
        public DateTime FecInicial { get; set; }
        public DateTime? FecFinal { get; set; }
        public DateTime FecEmision { get; set; }
        public DateTime? FecProbable { get; set; }
        public int Dias { get; set; }
        public decimal VrNetoTitulo { get; set; }
        public string EstadoTitulo { get; set; }
        public string Emisor { get; set; }
        public string Pagador { get; set; }
        public string RepresentacionGrafica { get; set; }
        public string RepresentacionXml { get; set; }
    }
}
