using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.ValidadorOperaciones.Dto
{
    [AutoMap(typeof(Identity.ValidarOperacion))]
    public  class ValidarOperacionResponseDto
    {
        public string IdCliente { get; set; }
        public string Cliente { get; set; }
        public decimal TotalCupo { get; set; }
        public decimal TotalDisponible { get; set; }
        public DateTime FecMaxVigencia { get; set; }
        public decimal TotalVigentes { get; set; }
        public int MaxMoraReal { get; set; }
        public string Pagare { get; set; }
        public string Aceptacion { get; set; }
        public int MaximoPlazo { get; set; }
        public string CodigoBloqueante { get; set; }
        public string NotaBloqueante { get; set; }
        public int TipoTercero { get; set; }
        public decimal VigentesEmisorPagador { get; set; }
        public string SinLimite { get; set; }
        public string CalificacionRiesgo { get; set; }
    }
}
