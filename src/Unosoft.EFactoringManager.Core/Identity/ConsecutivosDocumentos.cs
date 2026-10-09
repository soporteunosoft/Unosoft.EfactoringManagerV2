using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("ConsecutivosDocumentos", Schema = "dbo")]
    public class ConsecutivosDocumentos : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string TipoDocumento { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte IdOficina { get; set; }

        public int ConsDocumento { get; set; }

        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

    }
}
