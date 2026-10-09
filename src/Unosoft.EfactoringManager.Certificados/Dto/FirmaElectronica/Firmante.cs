using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto.FirmaDigital
{
    public class Firmante
    {
        public int NumeroTercero { get; set; }
        public int Contador { get; set; }
        public string Email { get; set; }
        public int Indicativo { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Documento { get; set; }
        public string TipoDocumento { get; set; }
    }
}
