using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Documentos.Dto
{
    [AutoMap(typeof(Identity.TiposDocumentos))]
    public class TipoDocumentoDto
    {

        public int TipoDoc { get; set; }
        public string NomDocumento { get; set; }

        //public int OrigenDoc { get; set; }
    }
}
