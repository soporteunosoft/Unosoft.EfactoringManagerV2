using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Terceros.Dto
{
    [AutoMap(typeof(Identity.SeguridadRespuestasTerceros))]
    public class RespuestasTerceros
    {
        public string IdTercero { get; set; }
        public int IdPregunta { get; set; }
        public string Respuesta { get; set; }
    }
}
