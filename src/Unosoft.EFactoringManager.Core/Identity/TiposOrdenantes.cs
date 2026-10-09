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
    [Table("TiposOrdenantes", Schema = "dbo")]
    public class TiposOrdenantes : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public int TipoOrdenante { get; set; }
        public string NomTipoOrdenante { get; set; }
        public int? TipoCertificado { get; set; }
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        ICollection<OrdenantesTiposCertificados> OrdenantesTiposCertificados { get; set; }
    }
}
