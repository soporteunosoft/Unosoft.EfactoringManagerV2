using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.GestionesTerceros.Dto;

namespace Unosoft.EFactoringManager.GestionesTerceros
{
    public interface IGestionesTercerosServices : IApplicationService
    {
        Task<List<GestionesTercerosDto>> ObtenerGestionesTerceros(int idGestion, int oficina, int? numTercero, string idTercero, int tipoTercero);

        Task<GestionesTercerosDto> ObtenerGestionesTercerosPorId(GestionesTercerosDto gestionesDto);

        Task<GetSingleRespuestaExtend> Insertar(GestionesTercerosDto gestionesTerceros);

        Task<bool> ActualizarGestiones(Identity.GestionesTerceros gestionesTerceros);

        Task<bool> EliminarGestiones(Identity.GestionesTerceros gestionesTerceros);

        Task<List<DocumentosGestionDto>> ObtenerDocumentosPorIdGestion(GestionesTercerosDto Gestion);

        Task<bool> EliminarDocumentoGestiones(DocumentosGestionDto documentosGestionDto);
    }
}
