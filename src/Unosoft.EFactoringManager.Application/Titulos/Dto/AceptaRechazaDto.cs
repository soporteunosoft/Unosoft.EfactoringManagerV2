using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Titulos.Dto
{
    public class AceptaRechazaDto
    {
        public List<AceptaRechazaDetalleDto> Titulos { get; set; }
        public int EstadoTitulo { get; set; }
        public int VrNetoPagar { get; set; }
        public DateTime FecProbable { get; set; }
    }

    public class AceptaRechazaDetalleDto
    {
        public int IdSolicitud { get; set; }
        public int IdOficina { get; set; }
        public string TipoDocumento { get; set; }
        public string IdTitulo { get; set; }
        public int NumeroTercero { get; set; }
    }
}
