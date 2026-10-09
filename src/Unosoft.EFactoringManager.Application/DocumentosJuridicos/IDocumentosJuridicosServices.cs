using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.DocumentosJuridicos.Dto;

namespace Unosoft.EFactoringManager.DocumentosJuridicos
{
    public interface IDocumentosJuridicosServices: IApplicationService
    {
        DocumentosJuridicosDto ObtenerDocumentoJuridico(int idDocJuridico);
    }
}
