using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.DocumentosJuridicos.Dto
{
    [AutoMap(typeof(Identity.DocumentosJuridicos))]
    public class DocumentosJuridicosDto
    {
        public int IdDocJuridico { get; set; }
        public string NomDocumento { get; set; }
        public string Texto { get; set; }
    }
}
