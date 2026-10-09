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
    public class Menu : Entity<int>
    {

        #region Overrides of Entity<long>

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }

        #endregion
        [Key]
        public int IdMenu { get; set; }
        public int IdPadre { get; set; }
        public string NomMenu { get; set; }
        public byte ConMenu { get; set; }
        public string Url { get; set; }
        public bool Activa { get; set; }
        public string Icono { get; set; }
    }
}
