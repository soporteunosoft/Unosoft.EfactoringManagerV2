using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Vinculaciones.Dto
{
    [AutoMap(typeof(Identity.Vinculaciones))]
    public class VinculacionesDto
    {
        public string Identificacion { get; set; }
        public string RazonSocial { get; set; }
        public string DirOficina { get; set; }
        public int IdCiudad { get; set; }
        public string TelOficina { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string NomContacto { get; set; }
        public string MovilContacto { get; set; }
        public string CargoCon { get; set; }
        public bool? GestionVinculacion { get; set; }
        public DateTime? FecVinculacion { get; set; }
        public DateTime? FecGestion { get; set; }
        public string IdUsrMod { get; set; }
    }
}
