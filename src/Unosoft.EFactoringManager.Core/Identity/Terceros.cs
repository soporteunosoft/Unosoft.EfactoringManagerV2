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
    
    [Table("Terceros", Schema = "dbo")]
    public class Terceros : Entity<int>
    #region Atributos 
    {

        #region Overrides of Entity<long>

        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        // [NotMapped]
        [Key]
        [Column("NumeroTercero")]
        public override int Id { get; set; }

        #endregion

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdTercero { get; set; }
        public int? IdTerceroPadre { get; set; }
        public int TipoIdentificacion { get; set; }
        public int? TipoContribuyente { get; set; }
        public short? DigitoVerificacion { get; set; }
        public byte IdOficina { get; set; }
        public string IdComisionista { get; set; }
        public string IdCorredor { get; set; }
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
        public int? IdCiudad { get; set; }
        public byte? IdSectorInd { get; set; }
        public int? TipoRegimen { get; set; }
        public bool? AgenteReteFuente { get; set; }
        public bool? AgenteReteIca { get; set; }
        public bool? AgenteReteIva { get; set; }
        public bool? AutoReteFuente { get; set; }
        public bool? AutoReteIva { get; set; }
        public bool? AutoReteIca { get; set; }
        public decimal? SaldoActual { get; set; }
        public float? TasaInteres { get; set; }
        public float? PorcNegociacion { get; set; }
        public float? PorcGravBancario { get; set; }
        public bool? Juridico { get; set; }
        public string Login { get; set; }
        public string Contrasena { get; set; }
        public bool Cliente { get; set; }
        public bool Inversionista { get; set; }
        public bool Pagador { get; set; }
        public bool Empleado { get; set; }
        public bool Ordenante { get; set;}
        public bool Comisionista { get; set; }
        public int? Codigo { get; set; }
        public System.DateTime? FecCodigo { get; set; }
        public int? ConsecutivoAnexo { get; set; }
        public int? TipoOrdenante { get; set; }
        public string Profesion { get; set; }
        public string Area { get; set; }
        public string AtribucionLimitacion { get; set; }
        public bool? Certificado { get; set; }
        public System.DateTime? FechaCert { get; set; }
        public int? VigenciaCert { get; set; }
        public System.DateTime? FecVigenciaCert { get; set; }
        public string NumSolicitudCert { get; set; }
        public bool Firma { get; set; }  
        public int? PlazoFirmar { get; set; }
        [StringLength(2)]
        public string CanalNotificacionFirma { get; set; }
        public bool Activa { get; set; }
        public string IdUsrMod { get; set; }
        public System.DateTime FecModifica { get; set; }
        public string HoraModifica { get; set; }
        
        [ForeignKey("IdTerceroPadre")]
        public Terceros TerceroPadre { get; set; }

        [ForeignKey("TipoIdentificacion")]
        public TiposIdentificacion TiposIdentificacion { get; set; }

        [ForeignKey("TipoOrdenante")]
        public TiposOrdenantes TiposOrdenantes { get; set; }

        [ForeignKey("IdCiudad")]
        public Ciudades Ciudades { get; set; }
        public bool Aceptacion { get; set; }
        public bool Notifica { get; set; }



        #endregion
    }
}
