using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Unosoft.EFactoringManager.TiposOperaciones;

namespace Unosoft.EFactoringManager.TiposOperacion
{
    public class TiposOperacionServices : ApplicationService, ITiposOperacionServices
    {

        #region Atributos
        TiposOperacionesManager _tiposOperacionesManager;
        #endregion

        public TiposOperacionServices(TiposOperacionesManager tiposOperacionesManager)
        {
            _tiposOperacionesManager = tiposOperacionesManager;
        }

        [HttpGet]
        public List<ModalidadesOperacion.Dto.TipoOperacionDto> ObtenerTipoOperacion()
        {
            try
            {
                var tiposOperacionList = _tiposOperacionesManager.ObtenerTipoOperacion();
                return ObjectMapper.Map<List<ModalidadesOperacion.Dto.TipoOperacionDto>>(tiposOperacionList);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
