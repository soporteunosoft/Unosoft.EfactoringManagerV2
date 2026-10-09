using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.ValidadorOperaciones.Dto
{
    public class ValidarOperacionRequestDto    {
        
        public string IdterceroEmisor { get; set;}
        public string IdterceroPagador { get; set; }
        public decimal MontoOperacion { get; set; }
        public int TipoOperacion { get; set;}
    }
}
