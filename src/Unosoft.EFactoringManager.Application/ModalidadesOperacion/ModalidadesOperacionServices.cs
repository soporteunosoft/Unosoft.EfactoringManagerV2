using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Dto;
using System.Globalization;
using Abp.Authorization;

namespace Unosoft.EFactoringManager.ModalidadesOperacion
{
    public class ModalidadesOperacionServices : ApplicationService, IModalidadesOperacionServices
    {

        #region Atributos
        ModalidadesOperacionManager _modalidadesOperacionManager;
        #endregion

        public ModalidadesOperacionServices(ModalidadesOperacionManager modalidadesOperacionManager)
        {
            _modalidadesOperacionManager = modalidadesOperacionManager;
        }

        public List<ModalidadesOperacion.Dto.ModalidadOperacionDto> ObtenerModalidadPorTipoOpe(ModalidadesOperacion.Dto.TipoOperacionDto tipoOperacion)
        {
            try
            {
                var modalidadesOpe = _modalidadesOperacionManager.ObtenerModalidadesOpePorTipoOpe(tipoOperacion.TipoOperacion);
                return ObjectMapper.Map<List<ModalidadesOperacion.Dto.ModalidadOperacionDto>>(modalidadesOpe);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
