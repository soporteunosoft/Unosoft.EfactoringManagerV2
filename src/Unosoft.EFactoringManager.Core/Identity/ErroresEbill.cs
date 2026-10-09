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
    [Table("ErroresEbill", Schema = "dbo")]
    public class ErroresEbill : Entity<int>
    {
        //#region Overrides of Entity<long>
        ///// <summary>
        ///// Unique identifier for this entity.
        ///// </summary>
        [Key]
        [Column("IdError")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        [StringLength(20)]
        public string CodigoError { get; set; }
        [StringLength(255)]
        public string DescripcionError { get; set; }
        [StringLength(255)]
        public string ProcesoError { get; set; }
     
    }
}
