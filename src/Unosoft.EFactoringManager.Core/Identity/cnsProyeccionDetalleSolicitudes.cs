using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Identity
{
    public class CNSProyeccionDetalleSolicitudes
    {
        public string TipoDocumento { get; set; }
        public byte IdOficina { get; set; }
        
        public int IdSolicitud { get; set; }
        public string IdCliente { get; set; }
        public DateTime FecSolicitud { get; set; }
        public string NomOperacion { get; set; }
        public string NomModOperacion { get; set; }
        public float TasaInteresCliente { get; set; }
        public decimal TotalVrAdmon { get; set; }
        public decimal TotalVrEntregarBruto { get; set; }
        public decimal TotalVrEntregarNeto { get; set; }
        public decimal TotalVrIntereses { get; set; }
        public decimal TotalReteFuenteInteres { get; set; }
        public decimal TotalVrNetoRecibir { get; set; }
        public decimal TotalVrComisionBruto { get; set; }
        public decimal IvaComision { get; set; }
        public decimal ReteFuenteComision { get; set; }
        public decimal ReteIcaComision { get; set; }
        public decimal ReteIvaComision { get; set; }
        public decimal TotalVrComisionNeto { get; set; }
        public decimal TotalVrNetoTitulo { get; set; }
        [Key]
        public string IdTitulo { get; set; }
        public DateTime FecInicial { get; set; }
        public DateTime FecFinal { get; set; }
        public short DiasOperacion { get; set; }
        public decimal VrPresente { get; set; }
        public decimal VrFuturo { get; set; }
        public decimal VrAdmon { get; set; }
        public decimal VrComision { get; set; }
        public decimal VrNetoTitulo { get; set; }
        public float PorcNegociacion { get; set; }
        public string Cliente { get; set; }
        public string Pagador { get; set; }
        public string IdPagador { get; set; }
        public string IdOrdenanteTercero { get; set; }
        public string Ordenante { get; set; }
        public int TipoIdentificacionOrd { get; set; }
        public string NomCiudad { get; set; }
        public int ConsecutivoAnexo { get; set; }
        public int TipoOrdenante { get; set; }
        public short DigitoVerificacionCli { get; set; }
        public short DigitoVerificacionPag { get; set; }
        public string IdTercero { get; set; }

        public float  PorcGravBancario { get; set; }
        public decimal GravamenOtros { get; set; }

        public decimal VrIntereses { get; set; }

        public string NumCuenta { get; set; }

        public string NomEstadoTitulo { get; set; }

        public string EndosadoA { get; set; }

        public string ReendosadoA { get; set; }

        public string CufeTitulo { get; set; }
    }


}