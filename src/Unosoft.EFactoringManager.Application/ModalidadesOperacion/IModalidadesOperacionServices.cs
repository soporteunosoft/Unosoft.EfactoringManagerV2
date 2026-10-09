using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Dto;

namespace Unosoft.EFactoringManager.ModalidadesOperacion
{
    public interface IModalidadesOperacionServices : IApplicationService
    {
        List<ModalidadesOperacion.Dto.ModalidadOperacionDto> ObtenerModalidadPorTipoOpe(ModalidadesOperacion.Dto.TipoOperacionDto TipoOperacion);
    }
}
