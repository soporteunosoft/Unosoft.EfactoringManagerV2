using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;


namespace Unosoft.EFactoringManager.Identity
{    //interface NotasEventosRadian

    [Table("NotasEventosRadian", Schema = "dbo")]
    public class NotasEventosRadian : Entity<string>

    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override string Id { get; set; }
        #endregion

      
        [StringLength(3)]
        public string TipoEvento { get; set; }

        public int TipoNota { get; set; }

        [ForeignKey(nameof(NotasEventosRadian.TipoEvento))]
        public TiposEventosRadian TipoEventoRadian{ get; set; }
        [ForeignKey(nameof(NotasEventosRadian.TipoNota))]
        public TiposNotasRadian TiposNotasRadian { get; set; }

    }
}
