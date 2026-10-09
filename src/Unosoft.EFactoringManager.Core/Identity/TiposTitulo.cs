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
    [Table("TiposTitulo", Schema = "dbo")]
    public class TiposTitulo : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TipoTitulo { get; set; }

        [Required]
        [StringLength(50)]
        public string NomTipoTitulo { get; set; }

        [Required]
        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecModifica { get; set; }
    }
}
