using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Oficina.Dto;

namespace Unosoft.EFactoringManager.Oficina
{
    public interface IOficinaAppService
    {
        List<OficinaDto> ObtOficinas(int idOficina);
    }
}
