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
    [Table("FirmasDetalleDocumentos", Schema = "dbo")]
    public class FirmasDetalleDocumentos: Entity<int>
    {
        #region Overrides of Entity<long>
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }
        #endregion
        //public string IdTercero { get; set; }
        public int IdDetDoc { get; set; }
        public int NumeroTercero { get; set; }
        public int NumTerceroFirma { get; set; }
        public bool Firma { get; set; }
        public int OrdenFirma { get; set; }
        public string IdUsrMod { get; set; }
        
        [Column(TypeName = "smalldatetime")]
        public DateTime FecModifica { get; set; }
        public string HoraModifica { get; set; }
        
        [ForeignKey("IdDetDoc")]
        public virtual Identity.DetalleDocumentos DetalleDocumento { get; set; }

    }


}