using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.EBillServices.DTO
{
    [DelimitedRecord(","), IgnoreFirst(1)]
    public class ErrorCUFE
    {
        public int NumeroLinea { get; set; }
        public string CUFE { get; set; }
        public string CodError { get; set; }
        public string DescError { get; set; }
    }


    [DelimitedRecord("|"), IgnoreFirst(1)]
    public class ErrorEndoso
    {
        public int NumeroLinea { get; set; }
        public string Prefijo { get; set; }
        public string NumeroFactura { get; set; }
        public string CodError { get; set; }        
    }

}
