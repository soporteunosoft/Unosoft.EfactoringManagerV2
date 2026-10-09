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
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.DetalleDocumentos.Dto;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.DetalleDocumentos
{
    public class DetalleDocumentosServices : ApplicationService, IDetalleDocumentosServices
    {
        #region Atributos
        DetalleDocumentosManager _detalleDocumentosManager;
        private readonly UserManager _userManager;
        private readonly Terceros.TercerosManager _tercerosManager;
        #endregion

        public DetalleDocumentosServices(Terceros.TercerosManager tercerosManager, DetalleDocumentosManager detalleDocumentosManager, UserManager userManager)
        {
            _detalleDocumentosManager = detalleDocumentosManager;
            _userManager = userManager;
            _tercerosManager = tercerosManager;
        }

        [AbpAuthorize]
        public async Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentos(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput)
        {
            Identity.Terceros tercero = null;
            if (detalleDocDtoInput.NumeroTercero <= 0)
                tercero = await _tercerosManager.ObtenerTerceroPorId(detalleDocDtoInput.IdTercero);
            else
                tercero = await _tercerosManager.ObtenerTerceroPorId(detalleDocDtoInput.NumeroTercero);

            var detalleDocumentos = await _detalleDocumentosManager.ObtenerDetalleDocumentos(tercero.Id, detalleDocDtoInput.IdDocOrigen, detalleDocDtoInput.TipoDocumento);

            if (tercero != null && tercero.TerceroPadre != null)
            {
                detalleDocumentos.AddRange(await _detalleDocumentosManager.ObtenerDetalleDocumentos(tercero.TerceroPadre.Id, detalleDocDtoInput.IdDocOrigen, detalleDocDtoInput.TipoDocumento));
            }

            return ObjectMapper.Map<List<DetalleDocumentos.Dto.DetalleDocumentosDto>>(detalleDocumentos);
        }

        [AbpAuthorize]
        public async Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosXNumeroTercero(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput)
        {
            var tercero = detalleDocDtoInput.NumeroTercero > 0
                ? await _tercerosManager.ObtenerTerceroPorId(detalleDocDtoInput.NumeroTercero)
                : await _tercerosManager.ObtenerTerceroPorId(detalleDocDtoInput.IdTercero);

            if (tercero == null)
                return null;

            var detalleDocumentos = await _detalleDocumentosManager.ObtenerDetalleDocumentos(tercero.Id, detalleDocDtoInput.IdDocOrigen, detalleDocDtoInput.TipoDocumento);

            if (tercero != null && tercero.TerceroPadre != null)
            {
                detalleDocumentos.AddRange(await _detalleDocumentosManager.ObtenerDetalleDocumentos(tercero.TerceroPadre.Id, detalleDocDtoInput.IdDocOrigen, detalleDocDtoInput.TipoDocumento));
            }

            return ObjectMapper.Map<List<DetalleDocumentos.Dto.DetalleDocumentosDto>>(detalleDocumentos);
        }

        [AbpAuthorize]
        public async Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosFirmar(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput)
        {
            var tuple = await _detalleDocumentosManager.ObtenerDetalleDocumentosFirmar(detalleDocDtoInput.IdTercero, detalleDocDtoInput.IdDocOrigen);
            var detDocumentos = ObjectMapper.Map<List<DetalleDocumentos.Dto.DetalleDocumentosDto>>(tuple.Item1);

            detDocumentos.ForEach(x => x.RazonSocial =
                                    tuple.Item2.FirstOrDefault(t => t.IdTerceroPadre == x.NumeroTercero)?.RazonSocial);
            return detDocumentos;
        }

        public async Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosFirmarSol(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput)
        {
            var detalleDocumentos = await _detalleDocumentosManager.ObtenerDetalleDocumentosFirmarSol(detalleDocDtoInput.IdTercero, detalleDocDtoInput.IdDocOrigen,
                                                detalleDocDtoInput.TipoDocumento, detalleDocDtoInput.IdOficina.Value, detalleDocDtoInput.IdDocumento.Value);
            return ObjectMapper.Map<List<DetalleDocumentos.Dto.DetalleDocumentosDto>>(detalleDocumentos);
        }

        public async Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosFirmadosSol(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput)
        {
            var detalleDocumentos = await _detalleDocumentosManager.ObtenerDetalleDocumentosFirmadosSol(detalleDocDtoInput.IdTercero, detalleDocDtoInput.IdDocOrigen,
                                                detalleDocDtoInput.TipoDocumento, detalleDocDtoInput.IdOficina.Value, detalleDocDtoInput.IdDocumento.Value);
            return ObjectMapper.Map<List<DetalleDocumentos.Dto.DetalleDocumentosDto>>(detalleDocumentos);
        }

        [AbpAuthorize]
        public byte[] ObtenerPdfDocumento(DetalleDocumentos.Dto.DetalleDocumentosDto detalleDocumento)
        {
            return _detalleDocumentosManager.ObtenerPdfDocumento(detalleDocumento.DirDocumentoFirmado ?? detalleDocumento.DirDocumento);
        }

        [AbpAuthorize]
        public async Task<GetSingleRespuesta> ActualizarEstadosDetalleDocumentos(DetalleDocumentos.Dto.DetalleDocumentosDto detalleDocumento)
        {
            try
            {
                Identity.DetalleDocumentos detalleDoc = ObjectMapper.Map<Identity.DetalleDocumentos>(detalleDocumento);

                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _detalleDocumentosManager.ActualizarEstadosDetalleDocumentos(detalleDoc))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Documento Actualizado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Documento no Actualizado");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AbpAuthorize]
        public async Task<GetSingleRespuesta> ActualizarFechaRenovacionDetalleDocumentos(DetalleDocumentos.Dto.DetalleDocumentosDto detalleDocumento)
        {
            try
            {
                Identity.DetalleDocumentos detalleDoc = ObjectMapper.Map<Identity.DetalleDocumentos>(detalleDocumento);

                DateTime localDate = DateTime.Now;
                GetSingleRespuesta _respuesta;
                if (await _detalleDocumentosManager.ActualizarFechaRenovacionDetalleDocumentos(detalleDoc))
                {
                    _respuesta = new GetSingleRespuesta(localDate, 0, "Documento Actualizado");
                    return _respuesta;
                }
                else
                {
                    _respuesta = new GetSingleRespuesta(localDate, 1, "Documento no Actualizado");
                    return _respuesta;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<bool> ValidarDocumentosIniciales(string idTercero)
        {
            try
            {
                var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);

                var documentos = await _detalleDocumentosManager.ObtenerDetalleDocumentosPorTercero(tercero.TerceroPadre?.IdTercero);

                return !documentos.Any(x => x.Documento.Documentos.TipoFirma == (int)TipoFirma.Digital && (x.SinFirma = true || x.Documento.DirDocumento == null));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<string> ValidarDocumentosAutorizarSolicitud(string idTercero)
        {
            try
            {
                return await _detalleDocumentosManager.ValidarDocumentosAutorizarSolicitud(idTercero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public void RenovarDetDocumento(RenovarDocumentoDto documento)
        {
            long idUsuario = AbpSession.UserId.Value;
        }

        public void InsertarDetDocumento()
        {
        }

        public void EliminarDetDocumento()
        {
        }
    }
}
