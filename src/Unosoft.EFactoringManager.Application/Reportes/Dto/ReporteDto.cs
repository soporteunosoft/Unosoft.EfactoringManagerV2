using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Reportes.Dto
{
    [AutoMap(typeof(Identity.Reportes))]
    public class ReporteDto
    {
        public int Id { get; set; }
        public string NomReporte { get; set; }
        public string TipoDocumento { get; set; }
        public int? TipoTercero { get; set; }
        public int? TipoOpcion { get; set; }
        public int? TipoFormato { get; set; }
        public int? ClaseDocumento { get; set; }
    }
}
