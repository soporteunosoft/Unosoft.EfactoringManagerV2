using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones.Dto
{
    public class EndosoSolicitudDto
    {
        public Identity.Terceros Pagador { get; set; }
        public List<Identity.DetalleSolicitudes> DetalleSolicitudes { get; set; }
        public string listTitulos { get; set; }
        public bool Endoso { get; set; }
    }
}
