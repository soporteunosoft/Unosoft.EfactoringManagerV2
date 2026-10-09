using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Cupos.Dto
{
    [AutoMap(typeof(Identity.Cupos))]
    public class CuposDto
    {
        public int NumeroTercero { get; set; }        
        public int TipoMoneda { get; set; }
        public int TipoOperacion { get; set; }
        public DateTime? FecIniVigenciaCupo { get; set; }
        public int? PlazoVigencia { get; set; }
        public DateTime? FecFinVigenciaCupo { get; set; }
        public decimal? CupoOperaciones { get; set; }
        public bool? ValidarCupo { get; set; }

        public Identity.TiposOperaciones TiposOperaciones { get; set; }
    }
}
