using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.ErroresEbill.Dto;
using Unosoft.EFactoringManager.ErroresEBill;

namespace Unosoft.EFactoringManager.ErroresEbill
{
    public  class ErroresEbillAppService : ApplicationService, IErroresEbillAppService
    {

        private readonly ErroresEBillManager _erroresEBillManager;
        public ErroresEbillAppService(ErroresEBillManager erroresEBillManager)
        {
            _erroresEBillManager = erroresEBillManager;
        }

        [HttpGet]
        public List<ErroresEbillDto> ObtErrores(string codError)
        {
            try
            {
                if (codError == null)
                    return null;

                var errores = _erroresEBillManager.getListaErrores(codError);

                return ObjectMapper.Map<List<ErroresEbillDto>>(errores);
            }
            catch
            {
                throw;
            }
        }
    }
}
