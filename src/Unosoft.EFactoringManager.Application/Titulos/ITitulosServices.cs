using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Titulos.Dto;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.DetalleSolicitudes.DTO;

namespace Unosoft.EFactoringManager.Titulos
{
    public interface ITitulosServices : IApplicationService
    {
        void Prueba(TituloInputDto tituloInputDto);
        Task<List<TituloDto>> ObtenerTitulosPorTercero(TituloInputDto tituloInputDto);

        Task<List<TituloDto>> ObtenerTitulosPorFiltros(TituloInputDto tituloInputDto);

        Task<List<DetalleTituloDTO>> ObtTitulosEndosado(int numeroTercero, int idOficina, string tipoDocumento, int producto);

        Task<GetSingleRespuesta> ValidarActualizaTitulos(TituloInputDto tituloInputDto);

        Task<GetSingleRespuesta> ActualizarFechaProbableTitulos(FechaProbableDto request);
        byte[] ObtenerPdfTitulo(PdfDto request);

        Task<GetSingleRespuesta> AceptaRechazaTitulo(AceptaRechazaDto request);

        void Get();
    }
}
