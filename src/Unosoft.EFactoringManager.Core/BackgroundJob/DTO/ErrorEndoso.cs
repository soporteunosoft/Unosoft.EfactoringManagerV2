using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BackgroundJob.DTO
{
    public class ErrorEndoso
    {
        public int NumeroLinea { get; set; }
        public int CUFE { get; set; }
        public string CodError { get; set; }
        public string DescError { get; set; }
    }
}
