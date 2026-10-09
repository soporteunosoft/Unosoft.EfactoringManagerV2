using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Titulos.Dto;
using Unosoft.EFactoringManager.ModalidadesOperacion.Dto;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones.Dto
{
    [AutoMap()]
    public class InputSolicitudOperacion
    {
        public string TipoDocumento { get; set; }
        public byte IdOficina { get; set; }
        public int IdSolicitud { get; set; }
        public int NumeroInversionista { get; set; }
        public IList<TituloSolicitudDto> TitulosInputDto { get; set; }
            

    }
}
