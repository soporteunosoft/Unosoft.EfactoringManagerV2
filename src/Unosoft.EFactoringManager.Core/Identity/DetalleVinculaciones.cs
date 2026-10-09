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
    [Table("DetalleVinculaciones", Schema = "dbo")]
    public class DetalleVinculaciones : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public string TipoDocumento { get; set; }
        [Key]
        public string Identificacion { get; set; }
        [Key]
        public string IdentificacionTer { get; set; }
        public string NomTercero { get; set; }
        public int IdCiudad { get; set; }
        public decimal VrVentasMensuales { get; set; }
        public decimal VrPromedioFactura { get; set; }
        public int Plazo { get; set; }
        public int PlazoRealPago { get; set; }
        public float? PorcCompras { get; set; }
    }
}
