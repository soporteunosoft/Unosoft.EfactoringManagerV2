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
    
    [Table("Titulos", Schema = "dbo")]
    public class Titulos : Entity<int>
    {
        #region Overrides of Entity<long>

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }

        #endregion
        //[Key]
        //[Required]
        //[StringLength(20)]
        //public int IdCliente { get; set; }

        [Key]
        [Required]
        [StringLength(20)]
        public string IdTitulo { get; set; }
        [Key]
        public int TipoTitulo { get; set; }

        [Key]
        [Required]       
        public int NumeroTercero { get; set; }
        //WIGELO:29/08/2022 NO SE REQUIERE
        //public int TipoTercero { get; set; }

        public String TipoMoneda { get; set; }

        //WIGELO:29/08/2022 NO SE REQUIERE
        //public int? IdPagador { get; set; }
        //NumPagador se reemplaza por IdPagador
        public int? NumeroPagador { get; set; }

        [StringLength(20)]
        //WIGELO:29/08/2022 NO SE REQUIERE
        //public string IdInversionista { get; set; }

        public float? TasaInversionista { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrBrutoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrDescuentoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrAnticipoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? PorcIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrSubTotalTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? PorcReteIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrReteIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? PorcReteFuenteTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrReteFuenteTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? PorcReteIcaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrReteIcaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrNetoTitulo { get; set; }

        public float? PorcNegociacion { get; set; }
                
        public decimal? VrNegociacion { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrEntregarTercero { get; set; }

        public float? PorcGravBancario { get; set; }

        public float? VrGravBancario { get; set; }

        [Column(TypeName = "money")]
        public decimal? VrPresente;

        //[Column(TypeName = "money")]
        public decimal VrReserva { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecRecepcion { get; set; }

        [StringLength(8)]
        public string HoraRecepcion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecInicial { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecEmision { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecFinal { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecProbable { get; set; }

        public int? Dias { get; set; }

        [StringLength(20)]
        public string Garantia { get; set; }

        [StringLength(255)]
        public string ConceptoTitulo { get; set; }

        [StringLength(100)]
        public string CufeTitulo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecCufeTitulo { get; set; }

        public int? EstadoTitulo { get; set; }

        public string RepresentacionGrafica { get; set; }

        public string RepresentacionXml { get; set; }

        public bool Actualiza { get; set; }

        [StringLength(8)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        [StringLength(8)]
        public string HoraModifica { get; set; }

        [StringLength(20)]

        //C-VALIDACIONES
        public bool Vencida { get; set; }

        [StringLength(500)]
        public string DescripcionEstadoCUFE { get; set; }
        public bool AcuseRecibo { get; set; }
        public bool FecEntregaValidoNegociar { get; set; }
        public bool AceptacionTacita { get; set; }
        public bool AceptacionExpresa { get; set; }
        public bool InscritaFEcomoTV { get; set; }
        public bool Endoso { get; set; }
        [StringLength(200)]
        public string EndosadoA { get; set; }
        public bool FechaEntregaProductoServicio { get; set; }
      

        //D-ENDOSO
        [StringLength(500)]
        public string CodigoError { get; set; }
        
        public string MensajeProcesamiento { get; set; }       
        public bool FacturaVencida { get; set; }
        public string CudeTitulo { get; set; }

        [StringLength(20)]
        public string NumeroFactura { get; set; }
        [StringLength(20)]
        public string PrefijoFactura { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? FecCudeTitulo { get; set; }

        /// <summary>
        /// Objeto Pagador 
        /// </summary>
        [ForeignKey("NumeroPagador")] // OJO TEMPORAL DEBSE SER DIFERENTE
        public virtual Identity.Terceros Pagador { get; set; }

        /// <summary>
        /// Objeto Tercero 
        /// </summary>
        [ForeignKey("NumeroTercero")]
        public virtual Identity.Terceros Tercero { get; set; }

        /// <summary>
        /// Objeto Tercero 
        /// </summary>
        [ForeignKey("EstadoTitulo")]
        public virtual Identity.EstadosTitulos EstadosTitulos { get; set; }

        
        public virtual Identity.DetalleSolicitudes DetalleSolicitudes { get; set; }




    }
}
