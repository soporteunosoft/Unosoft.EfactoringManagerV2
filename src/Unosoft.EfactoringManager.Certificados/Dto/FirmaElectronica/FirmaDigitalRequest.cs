using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto.FirmaDigital
{
    public class FirmaDigitalRequest
    {
        public string Documentoid { get; set; }
        public List<Firmante> Firmantes { get; set; }
        public string NombreCreador { get; set; }
        public string Modificar { get; set; }
        public string Notificacion { get; set; }
        public string Callback { get; set; }
        public string DiasVence { get; set; }
    }
}
