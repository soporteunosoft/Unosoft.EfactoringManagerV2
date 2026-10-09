using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity
{
    public class Notificacion : Entity<int>    {
        public string IdCorreo { get; set; }
        public int IdOficina { get; set; }
        public string TipoDocumento { get; set; }
        public int IdDocumento { get; set; }        
       
    }
}
