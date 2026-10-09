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
    [Table("TiposTasaInteres", Schema = "dbo")]
    public class TiposTasaInteres : Entity<int>
    {
        [Key]
        [Column("TipoTasaInteres")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public string NomTipoTasaInteres { get; set; }
        public int TipoTasaInteresEBill { get; set; }
        public string IdUsrMod { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }
    }
}
