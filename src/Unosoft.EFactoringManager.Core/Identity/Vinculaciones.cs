using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("Vinculaciones", Schema = "dbo")]
    public class Vinculaciones : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public string Identificacion { get; set; }
        public string RazonSocial { get; set; }
        public string DirOficina { get; set; }
        public int IdCiudad { get; set; }
        public string TelOficina { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string NomContacto { get; set; }
        public string MovilContacto { get; set; }
        public string CargoCon { get; set; }
        public bool? GestionVinculacion { get; set; }
        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecVinculacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecGestion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        [StringLength(8)]
        public string HoraModifica { get; set; }

    }
}
