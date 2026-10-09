using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity
{
    public class spTitulos
    {
        public string IdTitulo { get; set; }
        public string IdTercero { get; set; }
        public string IdPagador { get; set; }
        public string NomTercero { get; set; }
        public string ApeTercero { get; set; }
        public decimal VrNetoTitulo { get; set; }
        public DateTime FecInicial { get; set; }
        public DateTime FecFinal { get; set; }
        public int Dias { get; set; }
        public int EstadoTitulo { get; set; }
        public string NomTipoTitulo { get; set; }
    }
}
