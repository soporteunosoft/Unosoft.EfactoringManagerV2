using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;


namespace Unosoft.EFactoringManager.Identity
{    //interface TiposNotasRadian

    [Table("TiposNotasRadian", Schema = "dbo")]
    public class TiposNotasRadian : Entity<int>

    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public int TipoNota { get; set; }
        public string NomNota { get; set; }
        public ICollection<NotasEventosRadian> TiposEvento { get; set; }
    }
}
