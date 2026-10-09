using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Oficina.Dto
{
    [AutoMap(typeof(Identity.Oficinas))]
    public class OficinaDto
    {
        public int IdOficina { get; set; }
        public string NitOficina { get; set; }
        public string NomOficina { get; set; }
        public string SiglaOficina { get; set; }
        public string NomRepresentante { get; set; }
        public string Identificacion { get; set; }
        public string DirOficina { get; set; }
        public string TelOficina { get; set; }
        public string FaxOficina { get; set; }
        public string EmailOficina { get; set; }
        public string WebSiteOficina { get; set; }
        public int? IdCiudad { get; set; }
        public int? IdContable { get; set; }
        public string CodContable { get; set; }
        public string EscrituraConstitucion { get; set; }
        public string DirRaizDocumentos { get; set; }
        public int? IdEmpresaDispapeles { get; set; }
        public string UsuarioDispapeles { get; set; }
        public string PasswordDispapeles { get; set; }
        public string TokenDispapeles { get; set; }
        public DateTime? FecTokenDispapeles { get; set; }
        public string DirEbillFTP { get; set; }
        public string IdUsrMod { get; set; }
        public DateTime? FecModifica { get; set; }
    }
}
