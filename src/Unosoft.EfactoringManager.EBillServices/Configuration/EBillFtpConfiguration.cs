using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.EBillServices.Configuration
{
    public  class EBillFtpConfiguration
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UploadDirectory { get; set; }
        public int Port { get; set; }
        public string NitEmpresa { get; set; }
        public string TipoIdentificacion { get; set; }
        public string RazonSocial { get; set; }
        public string DigitoVerificacion { get; set; }



    }
}
