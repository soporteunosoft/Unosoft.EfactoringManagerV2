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
	[Table("#CupoPagadores", Schema = "dbo")]
	public class HistorialCuposPagadores : Entity<int>
	{
		#region Overrides of Entity<long>
		[NotMapped]
		public override int Id { get; set; }
		#endregion

		public string IdPagador { get; set; }
		public string Pagador { get; set; }
		[Column(TypeName = "smalldatetime")]
		public DateTime FecActaAprobacion { get; set; }
		[Key]
		public int IdCupo { get; set; }
		public string NumActaAprobacion { get; set; }
		public string TipoActualiza { get; set; }
		[Column(TypeName = "money")]
		public decimal VrActualizacion { get; set; }
		[Column(TypeName = "smalldatetime")]
		public DateTime FecFinVigencia { get; set; }
		public string NotaActualizacion { get; set; }
		public string Bloqueado { get; set; }
	}
}