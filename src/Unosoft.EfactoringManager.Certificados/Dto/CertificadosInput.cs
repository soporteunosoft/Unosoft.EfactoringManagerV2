using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto
{
    public class CertificadosInput
    {
        
        
        public int tipoCert { get; set; }
        public int tipoDoc { get; set; }
        public string documento { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public int municipio { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string ocupacion { get; set; }
        public string fechaCert { get; set; }
        public int vigenciaCert { get; set; }
        public int formato { get; set; }
        public string testigo { get; set; }
        public string foto { get; set; }
        public string soporte { get; set; }
        public string pin { get; set; }
        public string pkcs10 { get; set; }
        public string verific_doc { get; set; }
        public string plantillaHuella { get; set; }
    }
}
