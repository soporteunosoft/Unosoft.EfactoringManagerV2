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
    //interface TiposEventosRadian

    [Table("TiposEventosRadian", Schema = "dbo")]
    public class TiposEventosRadian : Entity<string>

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
        [StringLength(1000)]
        public string NomEvento { get; set; }
        public bool Obligatorio { get; set; }
        public bool Rechazo { get; set; }
        public ICollection<NotasEventosRadian> Notas { get; set; }
        public ICollection<ValidarEventosRadian> CamposObligatorios { get; set; }


    }
}
