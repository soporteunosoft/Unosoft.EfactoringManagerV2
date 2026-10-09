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
    class OrdenantesTerceros : Entity<int>
    {
        #region Overrides of Entity<long>

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [NotMapped]
        public override int Id { get; set; }

        #endregion
        [Key]
        public string IdOrdenanteTercero { get; set; }
        [Key]
        public string IdTercero { get; set; }
        public string NomOrdenanteCliente { get; set; }
        public string ApeOrdenanteCliente { get; set; }
        public string ProfesionOrdCliente { get; set; }
        public string DirOficinaOrdCliente { get; set; }
        public string TelDomicilioOrdCliente { get; set; }
        public string TelOficinaOrdCliente { get; set; }
        public string TelMovilOrdCliente { get; set; }
        public int TipoIdentificacionOrd { get; set; }
        public string FaxOrdenanteCliente { get; set; }
        public string ApartadoAereoOrdCliente { get; set; }
        public string EmailOrdCliente { get; set; }
        public int IdCiudadOficinaOrdCliente { get; set; }
        public int TipoOrdenante { get; set; }
        public string AreaOrdCliente { get; set; }
        public string AtribucionLimitacion { get; set; }
        public bool Certificado { get; set; }
        public System.DateTime? FechaCert { get; set; }
        public int? VigenciaCert { get; set; }
        public System.DateTime? FecVigenciaCert { get; set; }
        public string NumSolicitudCert { get; set; }
        public string IdUsrMod { get; set; }
        public System.DateTime FecModifica { get; set; }


        /// <summary>
        /// Objeto Pagador 
        /// </summary>
        [ForeignKey("IdCiudadOficinaOrdCliente")]
        public virtual Identity.Ciudades Ciudad { get; set; }

        //[NotMapped]
        //public bool RequiereCertificado { get; set; }

        public bool FirmaOrdenanteCliente { get; set; }
    }
}
