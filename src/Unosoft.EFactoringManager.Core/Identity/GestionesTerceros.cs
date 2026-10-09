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
    [Table("GestionesTerceros", Schema = "dbo")]
    public class GestionesTerceros : Entity<int>
    {
        [Key]
        [Column("IdGestion")]
        public override int Id { get; set; }
        public int IdOficina { get; set; }
        public DateTime FecGestion { get; set; }
        public int NumeroTercero { get; set; }
        public int Contacto { get; set; }
        public int TipoGestion { get; set; }
        public string DatoGestion { get; set; }
        public string Observacion { get; set; }
        public bool Activa { get; set; }
        public string IdUsrMod { get; set; }
        public DateTime FecModifica { get; set; }

        [ForeignKey("NumeroTercero")]
        public virtual Identity.Terceros Tercero { get; set; }
        
        public ICollection<Identity.DocumentosGestion> DocumentosGestion { get; set; }
    }
}