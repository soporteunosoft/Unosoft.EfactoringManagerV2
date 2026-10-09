using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.GestionesTerceros.Dto
{
    [AutoMap(typeof(Identity.DocumentosGestion))]
    public class DocumentosGestionDto
    {
        public int Id { get; set; }
        public int IdGestion { get; set; }
        public int IdOficina { get; set; }
        public string DirDocumento { get; set; }
    }
}
