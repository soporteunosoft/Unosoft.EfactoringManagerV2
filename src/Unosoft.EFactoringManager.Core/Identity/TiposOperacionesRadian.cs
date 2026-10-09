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
    //interface TiposOperacionesRadian

    [Table("TiposOperacionesRadian", Schema = "dbo")]
    public class TiposOperacionesRadian : Entity<string>

    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override string Id { get; set; }
        #endregion

        [Key]
        [StringLength(3)]
        public string TipoOperacion { get; set; }
        [StringLength(1000)]
        public string NomOperacion { get; set; }
        [StringLength(3)]
        public string TipoEvento { get; set; }

    }
}
 
