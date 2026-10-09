using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Documentos.Dto
{
    [AutoMap(typeof(Identity.Documentos))]
    public class DocumentosDto
    {
        public int IdDocLinea { get; set; }
        public int TipoLinea { get; set; }
        public int TipoDoc { get; set; }
        public bool Obligatorio { get; set; }
        public string DirDescarga { get; set; }
        //public string NotaDocumento { get; set; }
        public Identity.TiposDocumentos TipoDocumento { get; set; }
    }
}
