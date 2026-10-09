using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dispapeles.Dto;
using Unosoft.EFactoringManager.Titulos;

namespace Unosoft.EFactoringManager.Dispapeles
{

    //[AbpAuthorize]
    public class ConsultaEventosServices : ApplicationService, IConsultaEventosServices
    {
        #region Atributos
        DispapelesManager _dispapelesManager;
        TitulosManager _titulosManager;
        #endregion

        public ConsultaEventosServices(DispapelesManager dispapelesManager, TitulosManager titulosManager)
        {
            _dispapelesManager = dispapelesManager;
            _titulosManager = titulosManager;
        }


        [HttpGet]
        public async Task<List<EventosDIAN>> ConsultarFactura(string cufe)
        {
            try
            {
                var factura = await _dispapelesManager.ConsultarFactura(cufe);
                return factura.Eventos;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
