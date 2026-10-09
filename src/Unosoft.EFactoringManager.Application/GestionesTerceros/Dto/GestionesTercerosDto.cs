using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.GestionesTerceros.Dto
{
    [AutoMap(typeof(Identity.GestionesTerceros))]
    public class GestionesTercerosDto
    {
        public int? Id { get; set; }
        public int IdOficina { get; set; }
        public DateTime FecGestion { get; set; }
        public int NumeroTercero { get; set; }
        public int Contacto { get; set; }
        public int TipoGestion { get; set; }
        public string DatoGestion { get; set; }
        public string Observacion { get; set; }
        public Identity.Terceros Tercero { get; set; }
    }
}