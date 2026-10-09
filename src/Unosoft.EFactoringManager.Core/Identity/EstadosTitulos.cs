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
    [Table("EstadosTitulos", Schema = "dbo")]
    public class EstadosTitulos : Entity<int>
    {
        //#region Overrides of Entity<long>
        ///// <summary>
        ///// Unique identifier for this entity.
        ///// </summary>
        [Key]
        [Column("IdEstadoTitulo")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public string NomEstadoTitulo { get; set; }
        public string DesEstadoTitulo { get; set; }
        [StringLength(20)]
        public string IdUsrMod { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }

    }
}
