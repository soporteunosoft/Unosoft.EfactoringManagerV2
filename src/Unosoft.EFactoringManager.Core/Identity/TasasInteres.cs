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
    [Table("TasasInteres", Schema = "dbo")]
    public class TasasInteres : Entity<int>
    {
        [Key]
        [Column("IdTasaInteres")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public string NomTasaInteres { get; set; }
        public int TipoTasaInteres { get; set; }
        public int ModalidadInteres { get; set; }
        public float? TasaInteres { get; set; }
        public bool? TasaVencida { get; set; }
        public bool? Predeterminado { get; set; }
        public bool? Activa { get; set; }
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }
    }
}
