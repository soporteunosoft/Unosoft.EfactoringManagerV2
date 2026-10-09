using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Menu.Dto
{
    [AutoMap()]
    public class TerceroMenuDto
    {
        public string IdTercero { get; set; }
        public int IdOficina { get; set; }
        public string Usuario { get; set; }
    }
}
