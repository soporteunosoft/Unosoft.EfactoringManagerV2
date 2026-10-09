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
    [Table("ModalidadesOperacion", Schema = "dbo")]
    public class ModalidadesOperacion : Entity<int>
    {
        #region Atributos 
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        public int TipoOperacion { get; set; }
        [Key]
        public int ModalidadOperacion { get; set; }

        public int TipoMoneda { get; set; }

        [Required]
        [StringLength(50)]
        public string NomModOperacion { get; set; }

        public int? IdTasaInteres { get; set; }

        public float TasaInteresMin { get; set; }

        public float TasaInteresMax { get; set; }

        public int? BaseInteres { get; set; }

        public short? BaseDias { get; set; }

        public int DiaPago { get; set; }

        public int? CausacionInteres { get; set; }

        public int? IdTasaMora { get; set; }

        public int? BaseMora { get; set; }

        public int DiasGraciaMora { get; set; }

        public float TasaDTFTA { get; set; }

        public float MargenTasaMin { get; set; }

        public float TasaMaxRemuneratoria { get; set; }

        public bool DescuentoComision { get; set; }

        public bool DescuentoAdmon { get; set; }

        public float PorcAdmon { get; set; }

        public bool GravamenBancario { get; set; }

        public bool ReteFuente { get; set; }

        public float PorcReteFuente { get; set; }

        public bool RecursoPropio { get; set; }

        public int? FactorPlazo { get; set; }

        public int? PlazoMinimo { get; set; }

        public int? ModeloFinanciero { get; set; }

        [StringLength(1)]
        public string IdFrecuenciaPagoCapital { get; set; }

        [StringLength(1)]
        public string IdFrecuenciaPagoInteres { get; set; }

        [StringLength(3)]
        public string IdConceptoComision { get; set; }

        [StringLength(3)]
        public string IdConceptoAdmon { get; set; }

        public bool? ComisionPagador { get; set; }

        public int TipoLiquidacion { get; set; }

        [StringLength(2)]
        public string AplicaPago1 { get; set; }

        [StringLength(2)]
        public string AplicaPago2 { get; set; }

        [StringLength(2)]
        public string AplicaPago3 { get; set; }

        [StringLength(2)]
        public string AplicaPago4 { get; set; }

        [StringLength(2)]
        public string AplicaPago5 { get; set; }

        [StringLength(2)]
        public string AplicaPago6 { get; set; }

        [StringLength(2)]
        public string AplicaPago7 { get; set; }

        [StringLength(50)]
        public string NomReporte { get; set; }

        public bool Activa { get; set; }

        [Required]
        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecModifica { get; set; }

        /// <summary>
        /// Objeto Pagador 
        /// </summary>
        [ForeignKey("IdTasaInteres")]
        public virtual Identity.TasasInteres TasaInteres { get; set; }
    }
}
