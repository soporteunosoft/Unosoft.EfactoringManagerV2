using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.ErroresEbill.Dto
{
    [AutoMap(typeof(Identity.ErroresEbill))]
    public class ErroresEbillDto
    {
        public int Id { get; set; }
        public string CodigoError { get; set; }
        public string DescripcionError { get; set; }
        public string ProcesoError { get; set; }
    }
}
