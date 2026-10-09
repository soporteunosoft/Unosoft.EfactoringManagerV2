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
    [Table("TiposIdentificacion", Schema = "dbo")]
    public class TiposIdentificacion : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public int TipoIdentificacion { get; set; }
        public string NomCorto { get; set; }
        public string NomIdentificacion { get; set; }
        public int? TipoIdentificacionAndes { get; set; }
        public string TipoIdentificacionRadian { get; set; }
        public int? TipoIdentificacionEBill { get; set; }



    }
}
