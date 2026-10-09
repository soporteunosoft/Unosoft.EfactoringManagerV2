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
    [Table("Ciudades", Schema = "dbo")]
    public class Ciudades : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public int IdCiudad { get; set; }
        public string NomCiudad { get; set; }
        public int IdPais { get; set; }
        public int IdDepartamento { get; set; }
        public int? IdProvincia { get; set; }
        public string CodDaneCiudad { get; set; }
        public float PorcReteIca { get; set; }
        public bool Activa { get; set; }
        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        /// <summary>
        /// Objeto Pais 
        /// </summary>
        [ForeignKey("IdPais")]
        public virtual Identity.Paises Pais { get; set; }

        /// <summary>
        /// Objeto Departamento 
        /// </summary>
        [ForeignKey("IdDepartamento")]
        public virtual Identity.Departamentos Departamento { get; set; }
    }
}
