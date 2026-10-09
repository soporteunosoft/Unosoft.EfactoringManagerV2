using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto
{
    public class ConsultarCertInput
    {
        public int TipoDoc { get; set; }
        public string Documento { get; set; }
        public int TipoDocEmp { get; set; }
        public string DocumentoEmp { get; set; }
        public string mensaje { get; set; }
        public int NumeroSolicitud { get; set; }
    }
}
