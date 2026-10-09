using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Reportes.Dto
{
    public class ReporteConsultaPDFXLS
    {
        public int Id { get; set; }
        public int NumeroTercero { get; set; }
        public int NumeroTerceroInicio { get; set; }
        public string IdTercero { get; set; }
        public string IdTerceroInicio { get; set; }
        public int IdOficina { get; set; }
        public string TipoDocumento { get; set; }
        public int IdDocumento { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool EsPdf { get; set; }
    }
}
