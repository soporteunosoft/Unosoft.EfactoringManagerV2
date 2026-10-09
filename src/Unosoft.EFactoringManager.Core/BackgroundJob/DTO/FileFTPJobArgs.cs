
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BackgroundJob.DTO
{
    public class FileFTPJobArgs
    {
       
        public string TipoDocumento { get; set; }      
        public int IdOficina { get; set; }      
        public int IdSolicitud { get; set; }
        public int IdInversionista { get; set; }
    }
}
