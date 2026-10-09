using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.ModalidadesOperacion.Dto
{
    [AutoMap(typeof(Identity.ModalidadesOperacion))]
    public class ModalidadOperacionDto
    {
        public int ModalidadOperacion { get; set; }
        public int TipoMoneda { get; set; }
        public string NomModOperacion { get; set; }
        public Identity.TasasInteres TasaInteres { get; set; }

    }
}
