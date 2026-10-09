using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("ImpDocumentosOperativos", Schema = "dbo")]
    public class ImpDocumentosOperativos : Entity<int>
    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public string TipoDocumento { get; set; }
        [Key]
        public int IdOficina { get; set; }
        [Key]
        public string IdConcepto { get; set; }
        [Key]
        public string IdImpuesto { get; set; }
        public string Concepto { get; set; }
        public float PorcImpuesto { get; set; }
        public decimal VrBaseImpuesto { get; set; }
        public string IdCuenta { get; set; }
        public float PorcImpuestoCon { get; set; }
        public string PorcImpuestoBase { get; set; }
        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }
    }
}
