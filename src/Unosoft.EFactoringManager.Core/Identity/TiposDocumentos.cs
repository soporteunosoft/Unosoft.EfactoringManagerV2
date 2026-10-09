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
    [Table("TiposDocumentos", Schema = "dbo")]
    public class TiposDocumentos : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public int TipoDoc { get; set; }
        public string NomDocumento { get; set; }
        public string NotaDocumento { get; set; }
        public bool Firma { get; set; }
        [StringLength(8)]
        public string IdUsrMod { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

    }
}
