using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("cnsDetIngresosClientes", Schema = "dbo")]
    public class ResumenRecaudos : Entity<int>
	{
		#region Overrides of Entity<long>
		[NotMapped]
		public override int Id { get; set; }
		#endregion
		[Key]
		public string oficina { get; set; }
		[Key]
		public int no_operacion { get; set; }
		[Key]
		public int recibo { get; set; }
		[Key]
		public string no_id_emisor { get; set; }
		public string razon_social_emisor { get; set; }
		public string no_id_pagador { get; set; }
		public string razon_social_pagador { get; set; }
		public decimal recaudo_mora { get; set; }
		public decimal devolucion_valor_descuento { get; set; }
		public decimal vr_otros_ajustes { get; set; }
		public decimal gmf { get; set; }
		public decimal reintegro_garantia { get; set; }
    }
}
