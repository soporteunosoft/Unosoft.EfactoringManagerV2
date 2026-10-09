using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Newtonsoft.Json;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("DocumentosGestion", Schema = "dbo")]
    public class DocumentosGestion : Entity<int>
    {
        [Key]
        [Column("IdDocumento")]
        public override int Id { get; set; }
        public int IdOficina { get; set; }
        public int IdGestion { get; set; }
        public string DirDocumento { get; set; }
        [ForeignKey("IdGestion")]
        public virtual Identity.GestionesTerceros GestionesTerceros { get; set; }
    }
}
