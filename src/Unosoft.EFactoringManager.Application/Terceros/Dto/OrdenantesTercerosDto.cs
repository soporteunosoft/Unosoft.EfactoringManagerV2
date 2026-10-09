using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;


namespace Unosoft.EFactoringManager.Terceros.Dto
{
    //[AutoMap(typeof(Identity.OrdenantesTerceros))]
     class OrdenantesTercerosDto
    {
        public string IdOrdenanteTercero { get; set; }
        public string IdTercero { get; set; }
        public string NomOrdenanteCliente { get; set; }
        public string ApeOrdenanteCliente { get; set; }
        public bool Certificado { get; set; }
        public System.DateTime? FechaCert { get; set; }
        public int? VigenciaCert { get; set; }
        public System.DateTime? FecVigenciaCert { get; set; }
        public string NumSolicitudCert { get; set; }
        //public bool CertificadoVigente { get; set; }
    }
}
