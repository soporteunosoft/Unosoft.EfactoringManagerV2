using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones.Dto
{
    public class AutorizarSolicitudDto
    {
        public SolicitudesOperacionesDto solicitudesOperacionesDto { get; set; }
        public int codigoAutorizacion { get; set; }
        public int estadoSolicitud { get; set; }
    }
}
