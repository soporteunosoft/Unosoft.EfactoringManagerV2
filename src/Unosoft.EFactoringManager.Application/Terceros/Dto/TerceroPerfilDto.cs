using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Terceros.Dto
{
    [AutoMap(typeof(Identity.Terceros))]
    public class TerceroPerfilDto
    {
        public int Id { get; set; }
        public string IdTercero { get; set; }
        public string IdTerceroPadre { get; set; }
        public string NomCorto { get; set; }
        public byte IdOficina { get; set; }
        public string RazonSocial { get; set; }
        public string RazonSocialPadre { get; set; }
        public string TelOficina { get; set; }
        public string TelMovil { get; set; }
        public string DirOficina { get; set; }
        public string Email { get; set; }
        public int Codigo { get; set; }
        public bool? Firma { get; set; }
        public string NomTipoOrdenante { get; set; }
        public int? TipoCertificado { get; set; }
        public string Pais { get; set; }
        public string IndicativoPais { get; set; }
        public bool CertificadoVigente { get; set; }
        public int VigenciaCert { get; set; }
        public float? TasaInteres { get; set; }
        public float? PorcNegociacion { get; set; }


        public TerceroPerfilDto()
        {

        }

        public TerceroPerfilDto(Identity.Terceros tercero)
        {
            Id = tercero.Id;
            IdTercero = tercero.IdTercero;
            IdTerceroPadre = tercero.TerceroPadre?.IdTercero;
            RazonSocial = tercero.RazonSocial;
            RazonSocialPadre = tercero.TerceroPadre?.RazonSocial;
            NomCorto = tercero.TiposIdentificacion.NomCorto;
            IdOficina = tercero.IdOficina;
            DirOficina = tercero.DirOficina;
            Email = tercero.Email;
            TelMovil = tercero.TelMovil;
            TelOficina = tercero.TelOficina;
            Firma = tercero.Firma;
            NomTipoOrdenante = tercero.TiposOrdenantes?.NomTipoOrdenante;
            TipoCertificado = tercero.TiposOrdenantes?.TipoCertificado;
            IndicativoPais = tercero.Ciudades?.Pais.IndicativoPais;
            Pais = tercero.Ciudades.Pais.NomPais;
            CertificadoVigente = tercero.FecVigenciaCert > DateTime.Now;
        }

    }


}
