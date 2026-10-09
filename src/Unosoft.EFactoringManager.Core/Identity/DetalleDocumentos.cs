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
    [Table("DetalleDocumentos", Schema = "dbo")]
    public class DetalleDocumentos : Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        //[NotMapped]
        [Key]
        [Column("IdDetDoc")]
        public override int Id { get; set; }
        #endregion

        //[NotMapped]
        //public int IdDetDoc => Id;
        public int IdDocLinea { get; set; }
        public int NumeroTercero { get; set; }
        //public string IdTercero { get; set; }
        public int? TipoTercero { get; set; }
        [NotMapped]
        public string RazonSocial { get; set; }
        public int? IdDetalleTitulo { get; set; }
        public string TipoDocumento { get; set; }
        public int? IdOficina { get; set; }
        public int? IdDocumento { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecDocumento { get; set; }
        public string HoraDocumento { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecRenovacion { get; set; }
        public string DirDocumento { get; set; }
        public bool? Obligatorio { get; set; }
        public bool? Validar { get; set; }
        public bool? Firma { get; set; }
        public bool? FirmaAutorizada { get; set; }
        
        [StringLength(200)]
        public string AsuntoDocumento { get; set; }
        [StringLength(50)]
        public string IdFirmaElectronica { get; set; }
        
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FecModifica { get; set; }

        public string DirDocumentoFirmado { get; set; }

        /// <summary>
        /// Objeto Pais 
        /// </summary>
        [ForeignKey("IdDocLinea")]
        public virtual Identity.Documentos Documentos { get; set; }

        public ICollection<FirmasDetalleDocumentos> FirmasDetalleDocumentos { get; set; }

        [ForeignKey("NumeroTercero")]
        public Identity.Terceros Terceros { get; set; }

        public bool Activa { get; set; }
    }
}
