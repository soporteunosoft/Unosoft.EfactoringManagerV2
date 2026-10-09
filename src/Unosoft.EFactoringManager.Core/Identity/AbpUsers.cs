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
    [Table("AbpUsers", Schema = "dbo")]
    public class AbpUsers : Entity<long>
    {
        public override long Id { get; set; }
        public string UserName { get; set; }
    }
}
