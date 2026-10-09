using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Titulos.Dto;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.Authorization;
using Unosoft.EFactoringManager.DetalleSolicitudes;
using Unosoft.EFactoringManager.DetalleSolicitudes.DTO;
using Unosoft.EFactoringManager.SolicitudesOperaciones;

namespace Unosoft.EFactoringManager.Titulos
{

    [AbpAuthorize]
    public class TitulosServices : ApplicationService, ITitulosServices
    {
        #region Atributos
        TitulosManager _titulosManager;
        DetalleSolicitudesManager _detalleSolicitudesManager;
        SolicitudesOperacionesManager _solicitudesOperacionesManager;

        #endregion

        public TitulosServices(TitulosManager titulosManager, Terceros.TercerosManager tercerosManager, SolicitudesOperacionesManager solicitudesOperacionesManager,  DetalleSolicitudes.DetalleSolicitudesManager detalleSolicitudesManager)
        {
            _titulosManager = titulosManager;
            _detalleSolicitudesManager = detalleSolicitudesManager;
            _solicitudesOperacionesManager = solicitudesOperacionesManager;
        }

        [HttpPost]
        public async Task<List<TituloDto>> ObtenerTitulosPorTercero(TituloInputDto tituloDto)
        {
            try
            {
                if (PermissionChecker.IsGranted(PermissionNames.Grd_Facturas))
                {
                    var titulos = await _titulosManager.ObtenerTitulos();
                    return ObjectMapper.Map<List<TituloDto>>(titulos);
                }
                else
                {
                    var titulos = await _titulosManager.ObtenerTitulosPorTercero(tituloDto.NumeroTercero);
                    return ObjectMapper.Map<List<TituloDto>>(titulos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<List<TituloDto>> ObtenerTitulosPorFiltros(TituloInputDto tituloInputDto)
        {
            try
            {
                if (PermissionChecker.IsGranted(PermissionNames.Form_PerfilTodos))
                {
                    var titulos = await _titulosManager.ObtTitulosPorFiltrosAdm(tituloInputDto.IdPagador,
                                                                       (tituloInputDto.FecRecepcionInicial == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecRecepcionInicial)).ToString("yyyy-MM-dd"),
                                                                       (tituloInputDto.FecRecepcionFinal == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecRecepcionFinal)).ToString("yyyy-MM-dd"),
                                                                       (tituloInputDto.FecFinalInicial == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecFinalInicial)).ToString("yyyy-MM-dd"),
                                                                       (tituloInputDto.FecFinalFinal == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecFinalFinal)).ToString("yyyy-MM-dd"),
                                                                       tituloInputDto.VrNetoTituloInicial,
                                                                       tituloInputDto.VrNetoTituloFinal,
                                                                       tituloInputDto.TipoTitulo);
                    return ObjectMapper.Map<List<TituloDto>>(titulos);
                }
                else
                {
                    var titulos = await _titulosManager.ObtTitulosPorFiltros(tituloInputDto.IdPagador,
                                                                       (tituloInputDto.FecRecepcionInicial == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecRecepcionInicial)).ToString("yyyy-MM-dd"),
                                                                       (tituloInputDto.FecRecepcionFinal == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecRecepcionFinal)).ToString("yyyy-MM-dd"),
                                                                       (tituloInputDto.FecFinalInicial == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecFinalInicial)).ToString("yyyy-MM-dd"),
                                                                       (tituloInputDto.FecFinalFinal == null) ? "" : (Convert.ToDateTime(tituloInputDto.FecFinalFinal)).ToString("yyyy-MM-dd"),
                                                                       tituloInputDto.VrNetoTituloInicial,
                                                                       tituloInputDto.VrNetoTituloFinal,
                                                                       tituloInputDto.TipoTitulo,
                                                                       tituloInputDto.NumeroTercero);
                    return ObjectMapper.Map<List<TituloDto>>(titulos);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<List<DetalleTituloDTO>> ObtTitulosEndosado(int numeroTercero, int idOficina, string tipoDocumento, int producto)
        {
            try
            {
                var titulos = await _titulosManager.ObtTitulosEndosado(numeroTercero, idOficina, tipoDocumento, producto);


                return titulos.Select(x => new DetalleTituloDTO
                {
                    NumeroTercero = x.NumeroTercero,
                    TipoDocumento = x.TipoDocumento,
                    IdSolicitud = x.IdSolicitud,
                    IdTitulo = x.IdTitulo,
                    TipoTitulo = x.TipoTitulo,
                    NumeroPagador = x.NumeroPagador,
                    VrNetoTitulo = x.VrNetoTitulo,
                    FecInicial = x.FecInicial,
                    FecFinal = x.FecFinal,
                    FecProbable = x.FecProbable,
                    DiasOperacion = x.DiasOperacion,
                    Tercero = x.Tercero,
                    Pagador = x.Pagador,
                    EstadoTitulo = x.ConceptoDetalleSolicitud

                }).ToList();

                //return ObjectMapper.Map<List<DetalleTituloDTO>>(titulos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public void Prueba(TituloInputDto tituloInputDto)
        {
            var xml = System.IO.File.ReadAllBytes(@"D:\ControlActividades\UnoSoft.EFactoringManager\900300965-01-984005021.xml");
            //_titulosManager.RegistrarFactura(tituloInputDto.IdTercero, xml);
        }

        public async Task<GetSingleRespuesta> ValidarActualizaTitulos(TituloInputDto tituloInputDto)
        {
            try
            {
                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                //FOR DO:PRUEBA ARREGLAR CON ARLEY
                if (await _titulosManager.ValidarModificacionTitulo(tituloInputDto.NumeroTercero))
                //if (_titulosManager.validarModificacionTitulo("860028580"))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Titulo fue actualizado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Titulo no fue actualizado");
                    return _respuesta;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GetSingleRespuesta> ActualizarFechaProbableTitulos(FechaProbableDto request)
        {
            try
            {
                foreach (var titulo in request.Titulos)
                {
                    await _titulosManager.ActualizarFecProbableTitulo(titulo.IdTitulo, titulo.NumeroTercero, request.FechaProbable);

                    Identity.Titulos titulos = new Identity.Titulos
                    {
                        IdTitulo = titulo.IdTitulo,
                        NumeroTercero = titulo.NumeroTercero
                    };

                    Identity.SolicitudesOperaciones solicitud = new Identity.SolicitudesOperaciones
                    {
                        IdSolicitud = titulo.IdSolicitud,
                    };

                    await _titulosManager.ActualizarEstadoTitulo(titulos, request.EstadoTitulo, null, solicitud);

                    Identity.DetalleSolicitudes IdetalleSolicitudes = new Identity.DetalleSolicitudes
                    {
                        IdSolicitud = titulo.IdSolicitud,
                        IdTitulo = titulo.IdTitulo,
                        NumeroTercero = titulo.NumeroTercero
                    };

                    await _detalleSolicitudesManager.Actualizar(IdetalleSolicitudes, request.EstadoTitulo, 0);
                }

                return new GetSingleRespuesta(DateTime.Now, 0, "Titulo fue actualizado");

            }
            catch (Exception)
            {
                return new GetSingleRespuesta(DateTime.Now, 1, "Titulo no fue actualizado");
            }
        }

        [HttpPut]
        public async Task<GetSingleRespuesta> AceptaRechazaTitulo(AceptaRechazaDto request)
        {
            try
            {
                foreach (var titulo in request.Titulos)
                {
                    Identity.Titulos titulos = new Identity.Titulos
                    {
                        IdTitulo = titulo.IdTitulo,
                        NumeroTercero = titulo.NumeroTercero,
                        VrNetoTitulo = request.VrNetoPagar,
                        FecProbable = request.FecProbable
                    };

                    Identity.DetalleSolicitudes IdetalleSolicitudes = new Identity.DetalleSolicitudes
                    {
                        IdSolicitud = titulo.IdSolicitud,
                        TipoDocumento = titulo.TipoDocumento,
                        IdTitulo = titulo.IdTitulo,
                        NumeroTercero = titulo.NumeroTercero
                    };

                    Identity.SolicitudesOperaciones solicitudOperacion = new Identity.SolicitudesOperaciones
                    {
                        IdSolicitud = titulo.IdSolicitud,
                        IdOficina = 1,
                        TipoDocumento = titulo.TipoDocumento,
                        NumeroTercero = titulo.NumeroTercero
                    };

                    await _titulosManager.ActualizarEstadoTitulo(titulos, request.EstadoTitulo, request.FecProbable, solicitudOperacion);
                    await _detalleSolicitudesManager.Actualizar(IdetalleSolicitudes, request.EstadoTitulo, request.VrNetoPagar);

                    await _solicitudesOperacionesManager.ActualizarEstadoSolicitud(solicitudOperacion, (int)EFactoringManagerConsts.EstadosSolicitudes.gEnviada, null);
                }

                return new GetSingleRespuesta(DateTime.Now, 0, "Titulo fue actualizado");
            }
            catch (Exception)
            {
                return new GetSingleRespuesta(DateTime.Now, 1, "Titulo no fue actualizado");
            }
        }

        [AbpAuthorize]
        public byte[] ObtenerPdfTitulo(PdfDto request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.RepresentacionGrafica))
                    return _titulosManager.ObtenerPdfTitulo(request.RepresentacionGrafica);

                if (!string.IsNullOrEmpty(request.RepresentacionXml))
                    return _titulosManager.ObtenerPdfTitulo(request.RepresentacionXml);

                throw new ArgumentException("No se proporcionó ninguna representación válida.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Get()
        {
            throw new NotImplementedException();
        }
    }
}
