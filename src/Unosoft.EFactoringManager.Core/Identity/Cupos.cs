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

    [Table("Cupos", Schema = "dbo")]
    public class Cupos : Entity<int>
    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion
        [Key]
        public int NumeroTercero { get; set; }
        [Key]
        public int TipoMoneda { get; set; }
        [Key]
        public int TipoOperacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecIniVigenciaCupo { get; set; }
        public int? PlazoVigencia { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecFinVigenciaCupo { get; set; }

        [Column(TypeName = "money")]
        public decimal? CupoOperaciones { get; set; }
        public bool? ValidarCupo { get; set; }

        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        /// <summary>
        /// Objeto TipoOperacion 
        /// </summary>
        [ForeignKey("TipoOperacion")]
        public virtual Identity.TiposOperaciones TiposOperaciones { get; set; }

    }
}
