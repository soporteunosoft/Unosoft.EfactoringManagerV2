using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Oficina.Dto;
using Unosoft.EFactoringManager.Oficina;

namespace Unosoft.EFactoringManager.Oficina
{
    public  class OficinaAppService : ApplicationService, IOficinaAppService
    {

        private readonly OficinaManager oficinaManager;
        public OficinaAppService(OficinaManager _oficinaManager)
        {
            oficinaManager = _oficinaManager;
        }

        [HttpGet]
        public List<OficinaDto> ObtOficinas(int idOficina)
        {
            try
            {
                var oficinas = oficinaManager.ObtOficinas(idOficina);

                return ObjectMapper.Map<List<OficinaDto>>(oficinas);
            }
            catch
            {
                throw;
            }
        }
    }
}
