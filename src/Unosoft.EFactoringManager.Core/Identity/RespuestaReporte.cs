using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity
{
    public class RespuestaConsultaReporte : Entity<int>
    {
      
        public string TipoDocumento { get; set; }
        public int IdOficina { get; set; }
        public int IdDocumento { get; set; }
        public DateTime FecDocumento { get; set; }  
        public decimal Valor { get; set; }
        public string Concepto { get; set; }
    }

    public class RequestConsultaReporte : RespuestaConsultaReporte
    {
        public new int Id { get; set; }
        public int NumeroDocumento { get; set; }
    }
}
