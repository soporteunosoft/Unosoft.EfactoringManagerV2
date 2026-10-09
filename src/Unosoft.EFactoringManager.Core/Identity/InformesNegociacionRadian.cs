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
    //interface InformesNegociacionRadian

    [Table("InformesNegociacionRadian", Schema = "dbo")]
    public class InformesNegociacionRadian : Entity<string>

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
        public string TipoEvento { get; set; }
        [StringLength(50)]
        public string NombreAtributo { get; set; }
        public string ValorAtributo { get; set; }
     
    }
}
