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
    [Table("DetalleSolicitudes", Schema = "dbo")]
    public class DetalleSolicitudes : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion

        [Key]
        [Required]
        [StringLength(3)]
        public string TipoDocumento { get; set; }

        [Key]
        public byte IdOficina { get; set; }

        [Key]
        public int IdSolicitud { get; set; }

        [Key]
        [Required]      
        public int NumeroTercero { get; set; }

        [Key]
        [StringLength(20)]
        public string IdTitulo { get; set; }

        [Key]
        public int TipoTitulo { get; set; }

        public int? IdCuota { get; set; }       

        
        public int NumeroPagador { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecInicial { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecFinal { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecEmision { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecProbable { get; set; }

        public short DiasOperacion { get; set; }

        [Column(TypeName = "money")]
        public decimal VrPresente { get; set; }

        [Column(TypeName = "money")]
        public decimal VrFuturo { get; set; }

        [Column(TypeName = "money")]
        public decimal GravamenBancario { get; set; }

        [Column(TypeName = "money")]
        public decimal VrIntereses { get; set; }

        [Column(TypeName = "money")]
        public decimal ReteFuenteIntereses { get; set; }

        [Column(TypeName = "money")]
        public decimal VrAdmon { get; set; }

        [Column(TypeName = "money")]
        public decimal IvaAdmon { get; set; }

        [Column(TypeName = "money")]
        public decimal VrNetoRecibir { get; set; }

        [Column(TypeName = "money")]
        public decimal BaseCapitalInteres { get; set; }

        [Column(TypeName = "money")]
        public decimal VrCuota { get; set; }

        public float PorcNegociacion { get; set; }

        [Column(TypeName = "money")]
        public decimal VrBrutoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrDescuentoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrAnticipoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal PorcIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrSubTotalTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal PorcReteIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrReteIvaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal PorcReteFuenteTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrReteFuenteTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal PorcReteIcaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrReteIcaTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrNetoTitulo { get; set; }

        [Column(TypeName = "money")]
        public decimal VrNegociacion { get; set; }

        [Column(TypeName = "money")]
        public decimal VrReserva { get; set; }

        [Column(TypeName = "money")]
        public decimal VrComision { get; set; }

        [Column(TypeName = "money")]
        public decimal VrEntregarCliente { get; set; }

        [StringLength(255)]
        public string ConceptoDetalleSolicitud { get; set; }

        [Required]
        [StringLength(20)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecModifica { get; set; }
        /// <summary>
        /// Objeto Pagador 
        /// </summary>
       [ForeignKey("NumeroPagador")]
        public virtual Identity.Terceros Pagador { get; set; }

        [ForeignKey("NumeroTercero,IdTitulo,TipoTitulo")]
        public virtual Identity.Titulos Titulo { get; set; }

        public virtual Identity.SolicitudesOperaciones SolicitudOperacion { get; set; }

        [ForeignKey("NumeroTercero")]
        public virtual Identity.Terceros Tercero { get; set; }


    }
}
