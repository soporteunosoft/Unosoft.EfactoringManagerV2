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
    [Table("Documentos", Schema = "dbo")]
    public class Documentos : Entity<int>
    {

        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [Key]
        [Column("IdDocLinea")]
        public override int Id { get; set; }
        #endregion

        
        [NotMapped]
        public int IdDocLinea => Id;
        public int TipoLinea { get; set; }
        public int TipoDoc { get; set; }
        //public string NomDocumento { get; set; }
        public bool Obligatorio { get; set; }
        public int OrigenDoc { get; set; }
        public string DirDescarga { get; set; }
        //public string NotaDocumento { get; set; }
         public int? FirmaPagina { get; set; }
        public int? TipoFirma { get; set; }
        public int? TipoFirmaElectronica { get; set; }
        public int? DiasRenovacion { get; set; }
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

        /// <summary>
        /// Objeto Pais 
        /// </summary>
        [ForeignKey("TipoDoc")]
        public virtual Identity.TiposDocumentos TipoDocumento { get; set; }

      
    }
}
