using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.ValidadorOperaciones.Dto;

namespace Unosoft.EFactoringManager.ValidadorOperaciones
{
    public interface IValidadorOperacionesServices
    {
        List<ValidarOperacionResponseDto> Validar(ValidarOperacionRequestDto obj);
    }
}
