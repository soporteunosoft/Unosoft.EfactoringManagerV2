using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.ErroresEbill.Dto;

namespace Unosoft.EFactoringManager.ErroresEbill
{
    public interface IErroresEbillAppService
    {
        List<ErroresEbillDto> ObtErrores(string codError);
    }
}
