using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.Identity;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Vinculaciones.Dto;

namespace Unosoft.EFactoringManager.Vinculaciones
{
    public class VinculacionesServices : ApplicationService, IVinculacionesServices
    {
        #region Atributos
        VinculacionesManager _vinculacionesManager;
        #endregion

        public VinculacionesServices(VinculacionesManager vinculacionesManager)
        {
            _vinculacionesManager = vinculacionesManager;
        }
        [HttpPost]
        public async Task<GetSingleRespuesta> IngresarVinculacion(Identity.Vinculaciones vinculaciones)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _vinculacionesManager.InsertarViculacion(vinculaciones))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Vinculacion registrada");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Vinculacion No registrada");
                    return _respuesta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<GetSingleRespuesta> ActualizarVinculacion(Identity.Vinculaciones vinculaciones)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;

                if (await _vinculacionesManager.ActualizarVinculacion(vinculaciones))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Vinculacion No registrado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Vinculacion registrada");
                    return _respuesta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<GetSingleRespuesta> ActualizarEstadoVinculacion(VinculacionesDto vinculacionesDto)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _vinculacionesManager.ActualizarEstadoVinculacion(ObjectMapper.Map<Identity.Vinculaciones>(vinculacionesDto)))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Vinculacion Actualizada");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Vinculacion no Actualizada");
                    return _respuesta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<PagedResultDto<VinculacionesDto>> ObtenerVinculaciones(int skip = 0,
                                                             int limit = 20,
                                                             string sortField = "",
                                                             int sortOrder = 1,
                                                             string filter = "")
        {
            try
            {
                var result = await _vinculacionesManager.ObtenerVinculaciones(skip,
                                                                        limit,
                                                                        sortField,
                                                                        sortOrder,
                                                                        filter);

                var vinculaciones = ObjectMapper.Map<List<VinculacionesDto>>(result.Data);

                return new PagedResultDto<VinculacionesDto>(
                   result.Total,
                   vinculaciones
               );
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
