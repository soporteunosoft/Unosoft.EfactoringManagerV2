using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using iText.Commons.Bouncycastle.Asn1.X509;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.BackgroundJob;
using Unosoft.EFactoringManager.BackgroundJob.DTO;
using Unosoft.EFactoringManager.DetalleSolicitudes;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.SolicitudesOperaciones.Dto;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Titulos.Dto;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones
{
    //[AbpAuthorize]
    public class SolicitudesOperacionesServices : ApplicationService, ISolicitudesOperacionesServices
    {
        #region Atributos
        SolicitudesOperacionesManager _solicitudesOperacionesManager;
        DetalleSolicitudesManager _detalleSolicitudesManager;
        Titulos.TitulosManager _titulosManager;
        //FOR DO: REVISAR CON ARLEY
        Terceros.TercerosManager _tercerosManager;
        private readonly UserManager _userManager;
        #endregion

        public SolicitudesOperacionesServices(SolicitudesOperacionesManager solicitudesOperacionesManager,
                                              DetalleSolicitudesManager detalleSolicitudesManager,
                                                Titulos.TitulosManager titulosManager,
                                                Terceros.TercerosManager tercerosManager,
                                                 UserManager userManager)
        {
            _solicitudesOperacionesManager = solicitudesOperacionesManager;
            _titulosManager = titulosManager;
            _tercerosManager = tercerosManager;
            _detalleSolicitudesManager = detalleSolicitudesManager;
            _userManager = userManager;

        }



        public async Task<GetSingleRespuestaExtend> IngresarSolicitudOperacion(SolicitudOperacionInput inputSolicitudOperacion)
        {

            if (AbpSession.UserId == null)
                return null;

            long idUsuario = AbpSession.UserId.Value;
            User user = Task.Run(async () => await _userManager.GetUserByIdAsync(idUsuario)).Result;


            if (user == null) return null;

            inputSolicitudOperacion.Roles = (List<string>)Task.Run(async () => await _userManager.GetRolesAsync(user)).Result;

            DateTime localDate = DateTime.Now;

            if (inputSolicitudOperacion.IdOficina == 0)
                inputSolicitudOperacion.IdOficina = 1;

            try
            {
                var solicitud = await _solicitudesOperacionesManager.IngresarSolicitudOperacion(inputSolicitudOperacion);

                if (solicitud.IdSolicitud > 0)
                {
                    return new GetSingleRespuestaExtend(localDate, solicitud.IdSolicitud, "Solicitud Negociada",
                                                        new SolicitudesOperacionesDto()
                                                        {
                                                            IdSolicitud = solicitud.IdSolicitud,
                                                            TipoDocumento = solicitud.TipoDocumento,
                                                            IdOficina = solicitud.IdOficina,
                                                            Producto = solicitud.Producto,
                                                            TipoEndoso = solicitud.TipoEndoso,
                                                            PorcNegociacion = solicitud.PorcNegociacion,
                                                            TipoTasaInteres = solicitud.TipoTasaInteres,
                                                            TasaInteresCliente = solicitud.TasaInteresCliente,
                                                            FecSolicitud = solicitud.FecSolicitud.Value,
                                                            Tercero = solicitud.Tercero,
                                                            TotalFacturas = solicitud.TotalFacturas,
                                                            TotalAceptadas = solicitud.TotalAceptadas,
                                                            TotalRechazadas = solicitud.TotalRechazadas,
                                                            FecAceptacion = solicitud.FecAceptacion
                                                        });

                }
                else
                {
                    return new GetSingleRespuestaExtend(localDate, -1, "Solicitud No Negociada", null);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task Insertar(Identity.SolicitudesOperaciones solicitudesOperaciones)
        {
            try
            {
                await _solicitudesOperacionesManager.Insertar(solicitudesOperaciones);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SolicitudesOperacionesDto> ObtenerSolicitudOperacionPorId(SolicitudesOperacionesDto Solicitud)
        {
            try
            {
                var solicitud = await _solicitudesOperacionesManager.ObtenerSolicitudOpePorId(Solicitud.TipoDocumento, Solicitud.IdOficina, Solicitud.IdSolicitud);
                var dto = ObjectMapper.Map<SolicitudesOperacionesDto>(solicitud);
                return dto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TituloDto>> ObtenerTitulosPorIdSolicitud(SolicitudesOperacionesDto Solicitud)
        {
            try
            {
                long idUsuario = AbpSession.UserId.Value;
                User user = await _userManager.GetUserByIdAsync(idUsuario);
                var tercero = await _tercerosManager.ObtenerTerceroPorId(user.UserName);

                var titulos = await _detalleSolicitudesManager.ObtTitulosFromDetalleSolicitudes(Solicitud.TipoDocumento,
                                                                                          Solicitud.IdOficina,
                                                                                          Solicitud.IdSolicitud,
                                                                                          tercero?.TerceroPadre != null ? tercero.TerceroPadre.Id : 0);

                return ObjectMapper.Map<List<TituloDto>>(titulos);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<SolicitudesOperacionesDto>> ObtenerSolicitudesOpePorTercero(TerceroDto tercero)
        {
            try
            {
                var result = ObjectMapper.Map<IList<SolicitudesOperacionesDto>>(await _solicitudesOperacionesManager.ObtenerSolicitudesOpePorTercero(tercero.IdTercero));
                return result;
            }
            catch
            {
                throw;
            }

        }

        [HttpGet]
        public async Task<PagedResultDto<SolicitudesOperacionesDto>> ObtenerSolicitudesOpe(int numeroTercero,
                                                                      string tipoDocumento,
                                                                      int idOficina,
                                                                      int idSolicitud,
                                                                      int skip,
                                                                      int limit,
                                                                      string sortField = "",
                                                                      int sortOrder = -1)
        {
            try
            {
               var result = await _solicitudesOperacionesManager.ObtenerSolicitudesOpe(numeroTercero,
                                                                                    tipoDocumento,
                                                                                    idOficina,
                                                                                    idSolicitud,
                                                                                    skip,
                                                                                    limit,
                                                                                    sortField,
                                                                                    sortOrder);


                return new PagedResultDto<SolicitudesOperacionesDto>(
                    result.Item2,
                    ObjectMapper.Map<IList<SolicitudesOperacionesDto>>(result.Item1).ToList().AsReadOnly()
                );


            }
            catch
            {
                throw;
            }
        }


        public async Task<List<EndosoSolicitudDto>> ObtenerEndosoSolicitud(SolicitudesOperacionesDto Solicitud)
        {
            try
            {
                List<EndosoSolicitudDto> listEndosoSolicitudDto = new List<EndosoSolicitudDto>();

                int contador = 0;
                var detalleSolicitud = await _solicitudesOperacionesManager.ObtenerDetalleSolicitudPorId(Solicitud.TipoDocumento, Solicitud.IdOficina, Solicitud.IdSolicitud);
                for (int i = 0; i < detalleSolicitud.Count; i++)
                {

                    if (i == 0)
                    {
                        EndosoSolicitudDto endosoSolicitudDto = new EndosoSolicitudDto();
                        endosoSolicitudDto.Pagador = detalleSolicitud[i].Pagador;
                        endosoSolicitudDto.DetalleSolicitudes = new List<Identity.DetalleSolicitudes>();
                        endosoSolicitudDto.DetalleSolicitudes.Add(detalleSolicitud[i]);
                        endosoSolicitudDto.listTitulos = detalleSolicitud[i].IdTitulo;
                        endosoSolicitudDto.Endoso = false;
                        listEndosoSolicitudDto.Add(endosoSolicitudDto);

                    }
                    else
                    {
                        if (listEndosoSolicitudDto[contador].Pagador.Id == detalleSolicitud[i].NumeroPagador)
                        {
                            listEndosoSolicitudDto[contador].DetalleSolicitudes.Add(detalleSolicitud[i]);
                            listEndosoSolicitudDto[contador].listTitulos = listEndosoSolicitudDto[contador].listTitulos + ", " + detalleSolicitud[i].IdTitulo;
                        }
                        else
                        {
                            EndosoSolicitudDto endosoSolicitudDto = new EndosoSolicitudDto();
                            endosoSolicitudDto.Pagador = detalleSolicitud[i].Pagador;
                            endosoSolicitudDto.DetalleSolicitudes = new List<Identity.DetalleSolicitudes>();
                            endosoSolicitudDto.DetalleSolicitudes.Add(detalleSolicitud[i]);
                            endosoSolicitudDto.listTitulos = detalleSolicitud[i].IdTitulo;
                            endosoSolicitudDto.Endoso = false;
                            listEndosoSolicitudDto.Add(endosoSolicitudDto);
                            contador++;
                        }
                    }
                }
                return listEndosoSolicitudDto;
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<bool> ValidarCodigoTercero(AutorizarSolicitudDto autorizarSolicitudDto)
        {
            try
            {
                Identity.SolicitudesOperaciones solicitudOpe;
                solicitudOpe = ObjectMapper.Map<Identity.SolicitudesOperaciones>(autorizarSolicitudDto.solicitudesOperacionesDto);

                if (await _solicitudesOperacionesManager.ValidarCodigoTercero(solicitudOpe, autorizarSolicitudDto.codigoAutorizacion))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut]
        public async Task<GetSingleRespuesta> AutorizarSolicitud(AutorizarSolicitudDto autorizarSolicitudDto)
        {
            Identity.SolicitudesOperaciones solicitudOpe;
            GetSingleRespuesta _respuesta;
            DateTime localDate = DateTime.Now;

            try
            {
                solicitudOpe = ObjectMapper.Map<Identity.SolicitudesOperaciones>(autorizarSolicitudDto.solicitudesOperacionesDto);

#pragma warning disable CS0618
                if (await _solicitudesOperacionesManager.AutorizarSolicitudOperacion(solicitudOpe, autorizarSolicitudDto.estadoSolicitud))
#pragma warning restore CS0618
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Solicitud Autorizada");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Solicitud No Autorizada");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<GetSingleRespuesta> ActualizarEstadoSolicitud(SolicitudesOperacionesDto autorizarSolicitudDto)
        {
            Identity.SolicitudesOperaciones solicitudOpe;
            GetSingleRespuesta _respuesta;
            DateTime localDate = DateTime.Now;

            try
            {
                solicitudOpe = ObjectMapper.Map<Identity.SolicitudesOperaciones>(autorizarSolicitudDto);

                if (await _solicitudesOperacionesManager.ActualizarEstadoSolicitud(solicitudOpe, autorizarSolicitudDto.EstadoSolicitud, null))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Solicitud Autorizada");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Solicitud No Autorizada");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<GetSingleRespuesta> RechazarSolicitud(SolicitudesOperacionesDto Solicitud)
        {
            Identity.SolicitudesOperaciones solicitudOpe;
            GetSingleRespuesta _respuesta;
            DateTime localDate = DateTime.Now;

            try
            {
                solicitudOpe = ObjectMapper.Map<Identity.SolicitudesOperaciones>(Solicitud);

                if (await _solicitudesOperacionesManager.RechazarSolicitudOperacion(solicitudOpe))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Solicitud Rechazada");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Solicitud No Rechazada");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetSingleRespuesta> ValidarActualizaSolicitudes(TerceroDto tercero)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _solicitudesOperacionesManager.validarModificacionSolicitudes(tercero.IdTerceroPadre))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "solicitud fue actualizado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "solicitud no fue actualizado");
                    return _respuesta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> GenerarSolicitudPdf(SolicitudesOpeDto Solicitud)
        {
            try
            {
                return await _solicitudesOperacionesManager.GenerarPdfSolicitud(Solicitud.TipoDocumento, Solicitud.IdOficina, Solicitud.IdSolicitud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<byte[]> ObtenerZipPdfTitulosSolicitud(SolicitudOperacionesRequestDto request)
        {
            try
            {
                return await _solicitudesOperacionesManager.ObtenerZipPdfTitulosSolicitud(request.TipoDocumento, request.IdOficina, request.IdSolicitud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CargarDocumentosAutorizarSol(SolicitudesOperacionesDto Solicitud)
        {
            try
            {
                return await _solicitudesOperacionesManager.CargarDocumentosAutorizacionSol(Solicitud.TipoDocumento, Solicitud.IdOficina, Solicitud.IdSolicitud, Solicitud.Tercero.IdTercero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<GetSingleRespuesta> GenerarArchivoCufeXSolicitud(SolicitudOperacionesRequestDto request)
        {
            await _solicitudesOperacionesManager.GenerarArchivoCufeSolicitud(request.TipoDocumento, request.IdOficina, request.IdSolicitud);
            return new GetSingleRespuesta(DateTime.Now, 1, "Generación de Archivo Cufe en proceso");
        }

        [HttpPost]
        public async Task<GetSingleRespuesta> CargarFacturaCufeXSolicitud(SolicitudOperacionesRequestDto request)
        {
            await _solicitudesOperacionesManager.CargarFacturaCufeXSolicitud(request.TipoDocumento, request.IdOficina, request.IdSolicitud);
            return new GetSingleRespuesta(DateTime.Now, 1, "Cargue de factura(s) por Cufe en proceso");
        }


        public async Task<GetSingleRespuesta> EndosarSolicitudOperacion(InputSolicitudOperacion request)
        {
            var fileEndoso = new FileEndosoFTPJobArgs()
            {
                TipoDocumento = request.TipoDocumento,
                IdOficina = request.IdOficina,
                IdSolicitud = request.IdSolicitud,
                IdInversionista = request.NumeroInversionista,
                Titulos = request.TitulosInputDto.Select(x => new TituloFTP() { IdTitulo = x.IdTitulo, NumeroTercero = x.NumeroTercero, TipoTitulo = x.TipoTitulo }).ToList()
            };
            await _solicitudesOperacionesManager.GenerarArchivoEndosoSolicitud(fileEndoso);
            return new GetSingleRespuesta(DateTime.Now, 1, "Generación de Archivo Endodos en proceso");
        }


        [HttpDelete]
        public async Task<GetSingleRespuesta> EliminarDetalleSolictud(int idSolicitud, int tipoTitulo, int numeroTercero, string idTitulo)
        {
            await _solicitudesOperacionesManager.EliminarDetalleSolicitud(idSolicitud, tipoTitulo, numeroTercero, idTitulo);
            return new GetSingleRespuesta(DateTime.Now, 1, "El titulo se ha removido de manera exitosa");
        }


        [HttpPost]
        public async Task<GetSingleRespuesta> EliminarTitulosSolicitudOperacion(InputSolicitudOperacion request)
        {
            foreach (var titulo in request.TitulosInputDto)
            {
                await _solicitudesOperacionesManager.EliminarDetalleSolicitud(request.IdSolicitud, titulo.TipoTitulo, titulo.NumeroTercero, titulo.IdTitulo);
            }
            return new GetSingleRespuesta(DateTime.Now, 1, "El titulo se ha removido de manera exitosa");

        }


    }
}
