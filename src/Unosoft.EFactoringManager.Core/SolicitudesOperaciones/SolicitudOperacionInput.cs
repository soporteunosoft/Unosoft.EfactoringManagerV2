using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones
{
    public class SolicitudOperacionInput
    {
        public int NumeroTercero { get; set; }
        //public DateTime FecSolicitud { get; set; }
        public int Producto { get; set; }
        public int TipoTasaInteres { get; set; }
        public float? TasaInteresCliente { get; set; }
        public float? PorcNegociacion { get; set; }
        public int TipoEndoso { get; set; }
        public int IdOficina { get; set; }
        public string TipoDocumento { get; set; }
        public List<string> Roles { get; set; }
    }
}
