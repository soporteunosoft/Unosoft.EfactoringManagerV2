using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.SolicitudesOperaciones.Dto;
using Unosoft.EFactoringManager.Terceros.Dto;


namespace Unosoft.EFactoringManager.Terceros
{
    public interface ITercerosServices : IApplicationService
    {

        Task Insertar(Identity.Terceros tercero);

        Task<List<Identity.Terceros>> ObtenerTerceros(string idTercero = null);

        Task<TerceroDto> ObtenerTerceroPorId(string idTercero);

        Task<GetSingleRespuesta> ActualizarTercero(TerceroDto TerceroDto);

        Task<GetSingleRespuesta> InsertarCodigoTercero(SolicitudesOperacionesDto solicitudOperacion);

        Task<PagedResultDto<Identity.Terceros>> ObtenerTercerosUsuarios(bool esUsuario);

        Task<bool> VerificarRespuestasTercero(List<RespuestasTerceros> respuestasTerceros);

        Task<bool> ValidarCodigo(TerceroDto tercero);

        Task<List<PreguntasTerceros>> ObtenerPreguntasTerceros();

        Task<List<PreguntasTerceros>> ObtenerPreguntasTercerosAleatorio();

        Task<bool> InsertarRespuestasTercero(List<RespuestasTerceros> respuestasTerceros);

        Task GenerarCodigoTercero(string idTercero, int? tipoTercero);

    }
}
