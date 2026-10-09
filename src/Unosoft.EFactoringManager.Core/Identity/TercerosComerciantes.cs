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
    public class TercerosComerciantes : Entity<string>
    {

        [Key]
        [Column("IdTercero")]
        public override string Id { get; set; }
        public string Tercero { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Email { get; set; }

    }
}
