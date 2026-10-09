using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Certificado.Dto
{
    public class DocumentoFirmarInput
    {
        public string IdTercero { get; set; }
        public string IdTerceroPadre { get; set; }
        public int IdDetDoc { get; set; }
        public string PinFirma { get; set; }
        public int Formatoentrada { get; set; }
        public int Formatosalida { get; set; }
      

    }
}
