using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.TiposOperaciones;
using Unosoft.EFactoringManager.ValidadorOperaciones.Dto;
using Unosoft.EFactoringManager.ValidarOperaciones;

namespace Unosoft.EFactoringManager.ValidadorOperaciones
{
    public class ValidadorOperacionesServices : ApplicationService, IValidadorOperacionesServices
    {
        #region Atributos
        ValidarOperacionesManager _validarOperacionesManager;
        #endregion

        public ValidadorOperacionesServices(ValidarOperacionesManager validarOperacionesManager)
        {
            _validarOperacionesManager = validarOperacionesManager;
        }

        [HttpPost]
        public List<ValidarOperacionResponseDto> Validar(ValidarOperacionRequestDto obj)
        {
            var validarOperacionList = _validarOperacionesManager.Validar(obj.IdterceroEmisor ?? "", 
                                                                      obj.IdterceroPagador ?? "", 
                                                                      obj.MontoOperacion, 
                                                                      obj.TipoOperacion);
            return ObjectMapper.Map<List<ValidarOperacionResponseDto>>(validarOperacionList);
        }
    }
}
