using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Terceros.Dto
{
    [AutoMap(typeof(Identity.Terceros))]
    public class TerceroDto
    {

        public int Id { get; set; }
        public int IdTerceroPadre { get; set; }
        public string IdTercero { get; set; }
        public string RazonSocial { get; set; }
        public string DirOficina { get; set; }
        public string TelDomicilio { get; set; }
        public string TelOficina { get; set; }
        public string TelMovil { get; set; }
        public string Email { get; set; }
        public int Codigo { get; set; }
    }

}
