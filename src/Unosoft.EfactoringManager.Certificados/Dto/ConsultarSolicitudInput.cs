using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto
{
    public class ConsultarSolicitudInput
    {
        int rasolicitud { get; set; }
        int tipoDoc { get; set; }
        string documento { get; set; }
    }
}
