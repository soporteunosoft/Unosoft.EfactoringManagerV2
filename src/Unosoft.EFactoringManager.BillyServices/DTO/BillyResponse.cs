using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BillyServices.DTO
{
    public class BillyResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        public T Response { get; set; }
    }
}
