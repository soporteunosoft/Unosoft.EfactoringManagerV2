using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.Certificado.Dto;
using Unosoft.EFactoringManager.Terceros.Dto;

namespace Unosoft.EFactoringManager.Certificado
{
    public interface ICertificadoService : IApplicationService
    {
        Task<GetSingleRespuesta> CertificadosVinculacion(TerceroPerfilDto tercero);

        Task<GetSingleRespuesta> ValidarDocumentosCertificado(string idTercero);

        Task<bool> ValidarCertificado(string idTercero);

        Task<GetSingleRespuesta> FirmarDocumento(DocumentoFirmarInput documentoFirmar);
        Task<GetSingleRespuesta> FirmarDocumentoAutorizar(DocumentoFirmarInput documentoFirmar);
    }
}
