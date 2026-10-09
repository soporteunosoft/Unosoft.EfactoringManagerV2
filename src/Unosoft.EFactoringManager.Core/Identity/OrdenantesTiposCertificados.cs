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
    [Table("OrdenantesTiposCertificados", Schema = "dbo")]
    public class OrdenantesTiposCertificados: Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion
  
        public int TipoDoc { get; set; }
        public int TipoOrdenante { get; set; }
        public bool IdentificaPersona { get; set; }
        public string IdUsrMod { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }
        [ForeignKey("TipoOrdenante")]
        public TiposOrdenantes TiposOrdenantes { get; set; }

    }
}