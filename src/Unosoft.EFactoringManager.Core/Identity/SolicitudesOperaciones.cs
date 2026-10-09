using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using System.Text.Json.Serialization;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("SolicitudesOperaciones", Schema = "dbo")]
    public class SolicitudesOperaciones : Entity<int>
    {

        #region Overrides of Entity<long>

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }

        #endregion


        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string TipoDocumento { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte IdOficina { get; set; }

        [Key]
        public  int IdSolicitud { get; set; }
      
        public int NumeroTercero { get; set; }

        public int? TipoTercero { get; set; }

        public int? NumeroPagador { get; set; }

        //[StringLength(20)]
        //public string IdPagador { get; set; }

        //[StringLength(29)]
        //public string IdInversionista { get; set; }

        [StringLength(3)]
        public string IdConceptoSol { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecDocumento { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecSolicitud { get; set; }

        public int? Producto { get; set; }

        public int? TipoOperacion { get; set; }

        public int? ModalidadOperacion { get; set; }

        public int TipoTasaInteres { get; set; }

        public int? ModalidadInteres { get; set; }

        public float TasaInteresCliente { get; set; }

        public float TasaPuntos { get; set; }

        public int? PeriodosPago { get; set; }

        public short BaseDias { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecOperacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecPrimerPago { get; set; }

        [Column(TypeName = "money")]
        public decimal CapitalFinanciado { get; set; }

        public int CuotasPactadas { get; set; }

        [Column(TypeName = "money")]
        public decimal VrCuota { get; set; }

        [Column(TypeName = "money")]
        public decimal VrCuotaManejo { get; set; }

        [StringLength(1)]
        public string IdFrecuenciaPagoCapital { get; set; }

        [StringLength(1)]
        public string IdFrecuenciaPagoInteres { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecPrimerPagoCapital { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecPrimerPagoInteres { get; set; }


        public int? TipoDesembolso { get; set; }

        public int? TipoLiquidacion { get; set; }

        [StringLength(255)]
        public string NotaConcepto { get; set; }

        [StringLength(255)]

        public string NotaSolicitud { get; set; }

        [Column(TypeName = "money")]
        public decimal? CupoAprobado { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecAprobacion { get; set; }

        public int? TipoAprobacion { get; set; }

        public int? IdOperacion { get; set; }

        public int? IdEtapaSolicitud { get; set; }

        public int? IdEtapaSolicitudAnt { get; set; }

        public int TotalAsegurados { get; set; }

        public bool DescuentoComision { get; set; }

        public bool DescuentoAdmon { get; set; }

        public bool GravamenBancario { get; set; }

        public float PorcAdmon { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVrAdmon { get; set; }

        public float PorcGravBancario { get; set; }

        [Column(TypeName = "money")]
        public decimal GravamenOtros { get; set; }


        [Column(TypeName = "money")]
        public decimal TotalVrFuturo { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVrEntregarBruto { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVrEntregarNeto { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVrIntereses { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalReteFuenteInteres { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVrNetoRecibir { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalVrComisionBruto { get; set; }

        [Column(TypeName = "money")]
        public decimal IvaComision { get; set; }

        [Column(TypeName = "money")]
        public decimal ReteFuenteComision { get; set; }

        [Column(TypeName = "money")]
        public decimal? ReteIcaComision { get; set; }

        [Column(TypeName = "money")]
        public decimal? ReteIvaComision { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalVrComisionNeto { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalVrNetoTitulo { get; set; }

        [StringLength(3)]
        public string IdConceptoComision { get; set; }

        [StringLength(3)]
        public string IdConceptoEstudio { get; set; }

        [StringLength(3)]
        public string IdConceptoAdmon { get; set; }

        [StringLength(3)]
        public string IdConceptoGravamen { get; set; }

        [StringLength(20)]
        public string NumCuenta { get; set; }

        [StringLength(255)]
        public string NomArchivoC { get; set; }

        [StringLength(255)]
        public string NomArchivoF { get; set; }

        public int? TipoEndoso { get; set; }

        public float? PorcNegociacion { get; set; }

        public int EstadoSolicitud { get; set; }

        [Required]
        [StringLength(50)]
        public string RadicadoPor { get; set; }

        public bool Actualiza { get; set; }

       // [Required]
        [StringLength(20)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        [StringLength(8)]
        public string HoraModifica { get; set; }

        /// <summary>
        /// Objeto Tercero 
        /// </summary>
        [ForeignKey("NumeroTercero")]
        public virtual Identity.Terceros Tercero { get; set; }

        /// <summary>
        /// Objeto Tercero 
        /// </summary>
        [ForeignKey("EstadoSolicitud,TipoDocumento")]
        [JsonIgnore]
        public virtual Identity.EstadosSolicitudes EstadoSol { get; set; }

        [ForeignKey("TipoTasaInteres")]
        public virtual Identity.TiposTasaInteres TipoTasa { get; set; }

        public ICollection<DetalleSolicitudes> DetalleSolicitudes { get; set; }


        public int? NumeroInversionista { get; set; }

        [ForeignKey("NumeroInversionista")]
        public virtual Identity.Terceros Inversionista { get; set; }

        public int TotalFacturas { get; set; }
        public int TotalAceptadas { get; set; }
        public int TotalRechazadas { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecAceptacion { get; set; }

    }
}
