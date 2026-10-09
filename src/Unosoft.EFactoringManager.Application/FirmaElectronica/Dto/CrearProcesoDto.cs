using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.FirmaElectronica.Dto
{
  
    public class CrearProcesoDto
    {
        public List<int> Documentos { get; set; }
        public List<int> Terceros { get; set; }
        public int TipoFirma { get; set; }
    }
}
