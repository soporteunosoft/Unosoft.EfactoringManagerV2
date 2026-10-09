using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Reportes.Dto
{
    public class ReporteFiltro
    {
        public int Id { get; set; }
        public int NumeroTercero { get; set; }
        public string IdTercero { get; set; }
        public int NumeroTerceroInicio { get; set; }
        public string IdTerceroInicio { get; set; }
        public int IdOficina { get; set; }
        public DateTime? FechaCorte { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string TipoDocumento { get; set; }
    }
}
