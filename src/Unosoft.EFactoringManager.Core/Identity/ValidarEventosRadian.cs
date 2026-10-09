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

    [Table("ValidarEventosRadian", Schema = "dbo")]
    public class ValidarEventosRadian : Entity<string>

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
        [StringLength(50)]
        public string TipoObjeto { get; set; }
        [StringLength(50)]
        public string NombreAtributo { get; set; }
        [ForeignKey(nameof(ValidarEventosRadian.TipoEvento))]
        public TiposEventosRadian TipoEventoRadian { get; set; }

    }
}

