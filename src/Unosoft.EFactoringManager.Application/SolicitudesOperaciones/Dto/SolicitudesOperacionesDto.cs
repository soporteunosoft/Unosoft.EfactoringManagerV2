using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones.Dto
{

    [AutoMap(typeof(Identity.SolicitudesOperaciones))]
    public class SolicitudesOperacionesDto
    {
        public string TipoDocumento { get; set; }
        public byte IdOficina { get; set; }
        public int IdSolicitud { get; set; }
        public int TipoTercero { get; set; }
        public Identity.Terceros Tercero { get; set; }
        public int EstadoSolicitud { get; set; }
        public DateTime FecSolicitud { get; set; }
        public float TasaInteresCliente { get; set; }
        public float porcAdmon { get; set; }
        public decimal? TotalVrAdmon { get; set; }
        public decimal? TotalVrEntregarBruto { get; set; }
        public decimal? TotalVrEntregarNeto { get; set; }
        public decimal? TotalVrIntereses { get; set; }
        public decimal? TotalReteFuenteInteres { get; set; }
        public decimal? TotalVrNetoRecibir { get; set; }
        public decimal? TotalVrComisionBruto { get; set; }
        public decimal? IvaComision { get; set; }
        public decimal? ReteFuenteComision { get; set; }
        public decimal? ReteIcaComision { get; set; }
        public decimal? ReteIvaComision { get; set; }
        public decimal? TotalVrComisionNeto { get; set; }
        public decimal? TotalVrNetoTitulo { get; set; }
        public decimal? TotalReserva => TotalVrNetoTitulo - TotalVrNetoRecibir;
        public int? Producto { get; set; }
        public int? TipoEndoso { get; set; }
        public float? PorcNegociacion { get; set; }
        public int TipoTasaInteres { get; set; }
        public Identity.EstadosSolicitudes EstadoSol { get; set; }
        public int? NumeroInversionista { get; set; }
        public int TotalFacturas { get; set; }
        public int TotalAceptadas { get; set; }
        public int TotalRechazadas { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecAceptacion { get; set; }

        public List<DetalleSolicitudesDto> DetalleSolicitudes { get; set; }
    }

    [AutoMap(typeof(Identity.DetalleSolicitudes))]
    public class DetalleSolicitudesDto
    {
        public int NumeroPagador { get; set; }
        public PagadorDto Pagador { get; set; }
    }

    [AutoMap(typeof(Identity.Terceros))]
    public class PagadorDto
    {
        public string IdTercero { get; set; }
        public string NomTercero { get; set; }
        public string ApeTercero { get; set; }
        public string RazonSocial { get; set; }
    }

    [AutoMap(typeof(Identity.SolicitudesOperaciones))]
    public class SolicitudesOpeDto
    {
        public string TipoDocumento { get; set; }
        public byte IdOficina { get; set; }
        public int IdSolicitud { get; set; }

    }
}
