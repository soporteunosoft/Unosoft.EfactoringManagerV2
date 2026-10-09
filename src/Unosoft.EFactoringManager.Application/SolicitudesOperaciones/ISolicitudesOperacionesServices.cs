using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.SolicitudesOperaciones.Dto;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.Terceros.Dto;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones
{
    public interface ISolicitudesOperacionesServices : IApplicationService
    {
        Task<GetSingleRespuestaExtend> IngresarSolicitudOperacion(SolicitudOperacionInput inputSolicitudOperacion);

        Task<SolicitudesOperacionesDto> ObtenerSolicitudOperacionPorId(SolicitudesOperacionesDto Solicitud);

        Task<IList<SolicitudesOperacionesDto>> ObtenerSolicitudesOpePorTercero(TerceroDto tercero);

        Task<PagedResultDto<SolicitudesOperacionesDto>> ObtenerSolicitudesOpe(int numeroTercero, string tipoDocumento, int idOficina, int idSolicitud, int skip, int limit, string sortField = "", int sortOrder = -1);

        Task<List<EndosoSolicitudDto>> ObtenerEndosoSolicitud(SolicitudesOperacionesDto Solicitud);

        Task<GetSingleRespuesta> AutorizarSolicitud(AutorizarSolicitudDto autorizarSolicitudDto);

        Task<GetSingleRespuesta> ValidarActualizaSolicitudes(TerceroDto tercero);

        Task<byte[]> GenerarSolicitudPdf(SolicitudesOpeDto Solicitud);

        Task<byte[]> ObtenerZipPdfTitulosSolicitud(SolicitudOperacionesRequestDto request);

        Task<bool> CargarDocumentosAutorizarSol(SolicitudesOperacionesDto Solicitud);

        Task<bool> ValidarCodigoTercero(AutorizarSolicitudDto autorizarSolicitudDto);
    }
}
