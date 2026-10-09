using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Authorization.Roles;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.SeguridadPreguntasTerceros;
using Unosoft.EFactoringManager.SeguridadRespuestasTerceros;
using Unosoft.EFactoringManager.SolicitudesOperaciones.Dto;
using Unosoft.EFactoringManager.Terceros.Dto;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.Terceros
{

    public class TercerosServices : ApplicationService, ITercerosServices
    {

        #region Atributos
        TercerosManager _tercerosManager;
        SeguridadPreguntasTerceros.SeguridadPreguntasTercerosManager _seguridadPreguntasTercerosManager;
        SeguridadRespuestasTerceros.SeguridadRespuestasTercerosManager _seguridadRespuestasTercerosManager;
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;
        #endregion

        public TercerosServices(TercerosManager tercerosManager,
                                SeguridadPreguntasTercerosManager seguridadPreguntasTercerosManager,
                                SeguridadRespuestasTercerosManager seguridadRespuestasTercerosManager,
                                RoleManager roleManager,
                                UserManager userManager,
                                IAbpSession abpSession)
        {
            _tercerosManager = tercerosManager;
            _seguridadPreguntasTercerosManager = seguridadPreguntasTercerosManager;
            _seguridadRespuestasTercerosManager = seguridadRespuestasTercerosManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpSession = abpSession;
        }

        public async Task Insertar(Identity.Terceros tercero)
        {
            try
            {
                await _tercerosManager.Insertar(tercero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<List<Identity.Terceros>> ObtenerTerceros(string idTercero = null)
        {
            try
            {
                if (idTercero != null)
                {
                    return await _tercerosManager.ObtenerTerceroId(idTercero);
                }
                else
                {
                    return await _tercerosManager.ObtenerTerceros();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<PagedResultDto<TerceroPerfilDto>> ObtenerTercerosConFirma(string idTercero = null,
                                                             Boolean cargarHermanos = true,
                                                             int tipoTercero = 1,
                                                             int skip = 0,
                                                             int limit = 20,
                                                             string sortField = "",
                                                             int sortOrder = 1,
                                                             string filter = "")
        {
            try
            {
                var terceros = new List<TerceroPerfilDto>();

                var result = await _tercerosManager.ObtenerTercerosConFirma(idTercero,
                                                                        cargarHermanos,
                                                                        tipoTercero,
                                                                        skip,
                                                                        limit,
                                                                        sortField,
                                                                        sortOrder,
                                                                        filter);
                foreach (var tercero in result.Data)
                {
                    terceros.Add(new TerceroPerfilDto()
                    {
                        Id = tercero.Id,
                        IdTercero = tercero.IdTercero,
                        IdTerceroPadre = tercero.TerceroPadre?.IdTercero,
                        RazonSocial = tercero.RazonSocial,
                        RazonSocialPadre = tercero.TerceroPadre?.RazonSocial,
                        NomCorto = tercero.TiposIdentificacion.NomCorto,
                        IdOficina = tercero.IdOficina,
                        DirOficina = tercero.DirOficina,
                        Email = tercero.Email,
                        TelMovil = tercero.TelMovil,
                        TelOficina = tercero.TelOficina,
                        Firma = tercero.Firma,
                        NomTipoOrdenante = tercero.TiposOrdenantes?.NomTipoOrdenante,
                        TipoCertificado = tercero.TiposOrdenantes?.TipoCertificado,
                        CertificadoVigente = tercero.FecVigenciaCert > DateTime.Now,
                        TasaInteres = tercero.TasaInteres,
                        PorcNegociacion = tercero.PorcNegociacion
                    });
                }

                return new PagedResultDto<TerceroPerfilDto>(
                   result.Total,
                   terceros
               );
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<List<TerceroPerfilDto>> ObtenerOrdenantes(string idTerceroPadre)
        {
            try
            {
                List<TerceroPerfilDto> result = new List<TerceroPerfilDto>();

                var ordenantes = await _tercerosManager.ObtenerOrdenantes(idTerceroPadre);
                var empleados = await _tercerosManager.ObtenerEmpleadosConFirma();
                var terceros = ordenantes.Concat(empleados).ToList();

                foreach (var tercero in terceros)
                {
                    result.Add(new TerceroPerfilDto()
                    {
                        Id = tercero.Id,
                        IdTercero = tercero.IdTercero,
                        IdTerceroPadre = tercero.TerceroPadre?.IdTercero,
                        RazonSocial = tercero.RazonSocial,
                        RazonSocialPadre = tercero.TerceroPadre?.RazonSocial,
                        NomCorto = tercero.TiposIdentificacion.NomCorto,
                        IdOficina = tercero.IdOficina,
                        DirOficina = tercero.DirOficina,
                        Email = tercero.Email,
                        TelMovil = tercero.TelMovil,
                        TelOficina = tercero.TelOficina,
                        Firma = tercero.Firma,
                        NomTipoOrdenante = tercero.TiposOrdenantes?.NomTipoOrdenante,
                        TipoCertificado = tercero.TiposOrdenantes?.TipoCertificado,
                        IndicativoPais = tercero.Ciudades?.Pais.IndicativoPais,
                        Pais = tercero.Ciudades.Pais.NomPais,
                        CertificadoVigente = tercero.FecVigenciaCert > DateTime.Now
                    });
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public List<TerceroPerfilDto> ObtenerTercerosComerciante(string idTercero, int tipoTercero)
        {
            try
            {
                List<TerceroPerfilDto> result = new List<TerceroPerfilDto>();

                var terceros = _tercerosManager.ObtenerTercerosComerciante(idTercero, tipoTercero);

                foreach (var tercero in terceros)
                {
                    result.Add(new TerceroPerfilDto()
                    {
                        IdTercero = tercero.Id,
                        RazonSocial = tercero.Tercero,
                        NomCorto = tercero.TipoIdentificacion,
                        Email = tercero.Email
                    });
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<List<TerceroPerfilDto>> ObtenerInversionistas()
        {
            try
            {
                List<TerceroPerfilDto> result = new List<TerceroPerfilDto>();

                var terceros = await _tercerosManager.ObtenerInversionistas();

                foreach (var tercero in terceros)
                {
                    result.Add(new TerceroPerfilDto(tercero));
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<TerceroDto> ObtenerTerceroPorId(string idTercero)
        {
            try
            {
                var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);
                return ObjectMapper.Map<TerceroDto>(tercero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapToEntity(TerceroDto input, Identity.Terceros tercero)
        {
            ObjectMapper.Map(input, tercero);
        }

        [HttpPut]
        public async Task<GetSingleRespuesta> ActualizarTercero(TerceroDto TerceroDto)
        {
            Identity.Terceros Tercero = ObjectMapper.Map<Identity.Terceros>(TerceroDto);

            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _tercerosManager.ActualizarTercero(Tercero))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Tercero Actualizado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Tercero no actualizado");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetSingleRespuesta> InsertarCodigoTercero(SolicitudesOperacionesDto solicitudOperacion)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _tercerosManager.GenerarCodigoTercero(solicitudOperacion.Tercero.IdTercero, solicitudOperacion.TipoTercero))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Codigo Enviado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Problema al Generar Codigo");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<PagedResultDto<Identity.Terceros>> ObtenerTercerosUsuarios(bool esUsuario = false)
        {
            try
            {
                var result = await _tercerosManager.ObtenerTercerosUsuarios(esUsuario, false, 0, 0, "", 1, "");

                return new PagedResultDto<Identity.Terceros>(
                  result.Total,
                  result.Data
              );
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<PagedResultDto<TerceroPerfilDto>> ObtenerTercerosXUsuarios(bool esUsuario = false,
                                                                 int skip = 0,
                                                                 int limit = 20,
                                                                 string sortField = "",
                                                                 int sortOrder = 1,
                                                                 string filter = "")
        {
            try
            {
                List<TerceroPerfilDto> terceros = new List<TerceroPerfilDto>();

                var result = await _tercerosManager.ObtenerTercerosUsuarios(esUsuario, true, skip, limit, sortField, sortOrder, filter);
                foreach (var tercero in result.Data.Where(x => x.IdTerceroPadre > 0).ToList())
                {
                    terceros.Add(new TerceroPerfilDto()
                    {
                        IdTercero = tercero.IdTercero,
                        IdTerceroPadre = tercero.TerceroPadre?.IdTercero,
                        RazonSocial = tercero.RazonSocial,
                        RazonSocialPadre = tercero.TerceroPadre?.RazonSocial,
                        NomCorto = tercero.TiposIdentificacion.NomCorto,
                        IdOficina = tercero.IdOficina,
                        DirOficina = tercero.DirOficina,
                        Email = tercero.Email,
                        TelMovil = tercero.TelMovil,
                        TelOficina = tercero.TelOficina,
                        Firma = tercero.Firma,
                        NomTipoOrdenante = tercero.TiposOrdenantes?.NomTipoOrdenante,
                        TipoCertificado = tercero.TiposOrdenantes?.TipoCertificado
                    });
                }

                return new PagedResultDto<TerceroPerfilDto>(
                 result.Total,
                 terceros
             );
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<List<PreguntasTerceros>> ObtenerPreguntasTerceros()
        {
            try
            {
                var preguntas = await _seguridadPreguntasTercerosManager.ObtenerPreguntasTercero();
                return ObjectMapper.Map<List<PreguntasTerceros>>(preguntas);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<List<PreguntasTerceros>> ObtenerPreguntasTercerosAleatorio()
        {
            try
            {
                var preguntas = await _seguridadPreguntasTercerosManager.ObtenerPreguntasTerceroAleatorio();
                return ObjectMapper.Map<List<PreguntasTerceros>>(preguntas);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<bool> VerificarRespuestasTercero(List<RespuestasTerceros> respuestasTerceros)
        {
            try
            {
                bool verificacion = true;
                foreach (var respuesta in respuestasTerceros)
                {
                    verificacion = await _seguridadRespuestasTercerosManager.verificarRespuestasTercero(ObjectMapper.Map<Identity.SeguridadRespuestasTerceros>(respuesta));
                    if (!verificacion)
                    {
                        return verificacion;
                    }
                }

                return verificacion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<bool> InsertarRespuestasTercero(List<RespuestasTerceros> respuestasTerceros)
        {
            try
            {
                foreach (var respuesta in respuestasTerceros)
                {
                    var respuestaTercero = ObjectMapper.Map<Identity.SeguridadRespuestasTerceros>(respuesta);
                    respuestaTercero.IdUsrMod = respuesta.IdTercero;
                    respuestaTercero.FecModifica = DateTime.Now;
                    await _seguridadRespuestasTercerosManager.InsRespuestasTercero(respuestaTercero);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task GenerarCodigoTercero(string idTercero, int? tipoTercero)
        {
            try
            {
                await _tercerosManager.GenerarCodigoTercero(idTercero, tipoTercero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<bool> ValidarCodigo(TerceroDto tercero)
        {
            try
            {
                return await _tercerosManager.VerificarCodigoTercero(tercero.IdTercero, tercero.Codigo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
