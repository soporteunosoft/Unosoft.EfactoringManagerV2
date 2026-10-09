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
	[Table("cnsDetIngresosClientes2", Schema = "dbo")]
	public class DetalleRecaudos : Entity<int>
	{
		#region Overrides of Entity<long>
		[NotMapped]
		public override int Id { get; set; }
		#endregion
		[Key]
		public string oficina { get; set; }
		[Key]
		public int recibo { get; set; }
		[Column(TypeName = "smalldatetime")]
		public DateTime fec_pago { get; set; }
		[Key]
		public int no_operacion { get; set; }
		[Key]
		public string no_titulo { get; set; }
		[Key]
		public string no_id_emisor { get; set; }
		public string razon_social_emisor { get; set; }
		public string no_id_pagador { get; set; }
		public string razon_social_pagador { get; set; }
		public decimal factor_cliente { get; set; }
		[Column(TypeName = "smalldatetime")]
		public DateTime fec_inicia_op { get; set; }
		[Column(TypeName = "smalldatetime")]
		public DateTime fec_final_op { get; set; }
		public int dias_pago { get; set; }
		public decimal saldo_capital { get; set; }
		public decimal capital_pagado { get; set; }
		public decimal valor_descuento { get; set; }
		[Column(TypeName = "real")]
		public Single tasa_mora { get; set; }
		public decimal recaudo_mora { get; set; }
		public decimal devolucion_valor_descuento { get; set; }
		public decimal recaudo_neto { get; set; }
	}
}
