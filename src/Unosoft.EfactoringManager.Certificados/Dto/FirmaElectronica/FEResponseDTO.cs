using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto.FirmaDigital
{
    public class FEResponseDTO
    {
        public string Id { get; set; }
        public Boolean Estado { get; set; }
        public String Mensaje { get; set; }
        public string Url { get; set; }
    }
}
