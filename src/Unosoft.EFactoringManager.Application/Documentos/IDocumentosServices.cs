using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Documentos.Dto;

namespace Unosoft.EFactoringManager.Documentos
{
    public interface IDocumentosServices: IApplicationService
    {
        Task<List<DocumentosDto>> ObtenerDocumentos(int documentoOrigen);

        Task<bool> InsertarDocumento(DocumentoNewDto documento);

        Task<bool> EliminarDocumento(int idDetDoc);

    }
}
