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

    [Table("TiposOperaciones", Schema = "dbo")]
    public class TiposOperaciones : Entity<int>
    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }

        #endregion
        [Key]
        public int TipoOperacion { get; set; }

        [Required]
        [StringLength(50)]
        public string NomOperacion { get; set; }

        public int TipoLinea { get; set; }

        public int? TipoTitulo { get; set; }

        [StringLength(20)]
        public string IdCuentaContable { get; set; }

        public bool? Activa { get; set; }

        [Required]
        [StringLength(8)]
        public string IdUsrMod { get; set; }

        public int? DiasMaxMora { get; set; }

        public bool Pagador { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecModifica { get; set; }
    }
}
