using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("ProcesoEndosoBilly", Schema = "dbo")]
    public class ProcesoEndosoBilly : Entity<int>
    {
        #region Atributos   
        public string TipoDocumento { get; set; }
        public int IdOficina { get; set; }
        public int IdSolicitud { get; set; }
        public string Cufe { get; set; }
        public DateTime FechaProceso { get; set; } 
        public int Estado { get; set; }
        public string JsonRequest { get; set; }
        public string JsoResponse { get; set; }

        #endregion
    }
}
