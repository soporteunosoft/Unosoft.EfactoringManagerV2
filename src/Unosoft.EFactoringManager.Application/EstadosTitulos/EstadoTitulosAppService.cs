using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.EstadosTitulos.Dto;
using Unosoft.EFactoringManager.EstadosTitulosCore;

namespace Unosoft.EFactoringManager.EstadosTitulos
{
    public  class EstadoTitulosAppService : ApplicationService, IEstadoTitulosAppService
    {

        private readonly EstadosTitulosManager _estadosTitulosManager;
        public EstadoTitulosAppService(EstadosTitulosManager estadosTitulosManager)
        {
            _estadosTitulosManager = estadosTitulosManager;

        }

        [HttpGet]
        public List<EstadosTitulosDto> ObtEstados()
        {
            try
            {
                var estados = _estadosTitulosManager.ObtenerEstadosTitulos();

                return ObjectMapper.Map<List<EstadosTitulosDto>>(estados);

            }
            catch
            {
                throw;
            }
        }



    }
}
