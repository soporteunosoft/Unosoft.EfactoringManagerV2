using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Unosoft.EFactoringManager.Identity
{
    public class ObtenerTercerosUsuarios
    {
        [Key]
        public string IdTercero { get; set; }
        public int TipoIdentificacion { get; set; }
        public short DigitoVerificacion { get; set; }
        public byte IdOficina { get; set; }
        public DateTime FecApertura { get; set; }
        public string NomTercero { get; set; }
        public string ApeTercero { get; set; }
        public string RazonSocial { get; set; }
        public string DirOficina { get; set; }
        public string TelDomicilio { get; set; }
        public string TelOficina { get; set; }
        public string TelMovil { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        //public int EsUsuario { get; set; }
    }
}
