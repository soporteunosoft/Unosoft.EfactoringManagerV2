using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Titulos.Dto
{
    public class FechaProbableDto
    {
        public List<FechaProbableDetalleDto> Titulos { get; set; }
        public DateTime FechaProbable { get; set; }
        public int EstadoTitulo { get; set; }
    }


    public class FechaProbableDetalleDto
    {
        public int IdSolicitud { get; set; }
        public string IdTitulo { get; set; }
        public int NumeroTercero { get; set; }
    }
}
