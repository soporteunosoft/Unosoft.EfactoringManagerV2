using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;


namespace Unosoft.EFactoringManager.Terceros.Dto
{
    [AutoMap(typeof(Identity.SeguridadPreguntasTerceros))]
    public class PreguntasTerceros
    {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
    }
}
