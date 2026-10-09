using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;

namespace Unosoft.EFactoringManager.DetalleDocumentos
{
    public interface IDetalleDocumentosServices : IApplicationService
    {
        Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentos(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput);
        Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosFirmar(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput);
        Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosFirmarSol(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput);
        Task<List<DetalleDocumentos.Dto.DetalleDocumentosDto>> ObtenerDetalleDocumentosFirmadosSol(DetalleDocumentos.Dto.DetalleDocumentosInputDto detalleDocDtoInput);
        byte[] ObtenerPdfDocumento(DetalleDocumentos.Dto.DetalleDocumentosDto detalleDocumento);
        Task<GetSingleRespuesta> ActualizarEstadosDetalleDocumentos(DetalleDocumentos.Dto.DetalleDocumentosDto detalleDocumento);
        Task<GetSingleRespuesta> ActualizarFechaRenovacionDetalleDocumentos(DetalleDocumentos.Dto.DetalleDocumentosDto detalleDocumento);
        Task<bool> ValidarDocumentosIniciales(string idTercero);
        Task<string> ValidarDocumentosAutorizarSolicitud(string idTercero);
    }
}
