using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Dto
{
    public class GetSingleRespuesta

    {
        public virtual DateTime fecha_actual_servidor { get; set; }

        public virtual int codigo_respuesta { get; set; }

        public virtual string mensaje_respuesta { get; set; }

        public GetSingleRespuesta(DateTime _fecha_actual_servidor, int _codigo_respuesta, string _mensaje_respuesta)
        {
            fecha_actual_servidor = _fecha_actual_servidor;
            codigo_respuesta = _codigo_respuesta;
            mensaje_respuesta = _mensaje_respuesta;
        }
    }

    public class GetSingleRespuestaExtend : GetSingleRespuesta
    {
        public virtual Object Response { get; set; }
        public GetSingleRespuestaExtend(DateTime _fecha_actual_servidor, int _codigo_respuesta, string _mensaje_respuesta, object response ) : base(_fecha_actual_servidor, _codigo_respuesta, _mensaje_respuesta)
        {
            Response = response;
        }
    }
}
