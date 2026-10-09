using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.EstadosTitulos.Dto;

namespace Unosoft.EFactoringManager.EstadosTitulos
{
    public interface IEstadoTitulosAppService
    {

        List<EstadosTitulosDto> ObtEstados();
    }
}
