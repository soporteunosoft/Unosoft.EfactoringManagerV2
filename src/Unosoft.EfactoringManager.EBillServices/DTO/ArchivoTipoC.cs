using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.EBillServices.DTO
{
    [DelimitedRecord(","), IgnoreFirst(1)]
    //[DelimitedRecord("|"), IgnoreFirst(1)]
    public class ArchivoTipoC
    {
        public string NombreArchivoProcesado { get; set; }
        public string NitAdquiriente { get; set; }
       // public string DVAdquiriente { get; set; }
        public string NombreAdquiriente { get; set; }
        public string IdFactura { get; set; }
        public string EstadoDian { get; set; }
        public string CUFE { get; set; }
        public string Prefijo { get; set; }
        public string Vencida { get; set; }
        public string DescripcionEstadoCUFE { get; set; }
        public string FechaEntregaProductoServicio { get; set; }
        public string AcuseRecibo { get; set; }
        public string FechaEntregaProductoServicioValidoNegociar { get; set; }
        public string AceptacionTacita { get; set; }
        public string AceptacionExpresa { get; set; }
        public string InscritaFEcomoTV { get; set; }
        public string Endoso { get; set; }
        public string EndosadoA { get; set; }
        public string NitEmpresaEmisora { get; set; }
        public string DigitoVerificacion { get; set; }
        public string RazonSocialEmpresaEmisora { get; set; }
        public string FechaGeneracion { get; set; }
        public string NumeroFactura { get; set; }
        public string FechaExpedicion { get; set; }
        public string FechaVencimiento { get; set; }
        public string TotalAPagar { get; set; }
        public string CodigoMoneda { get; set; }
        public string ValorNotaCredito { get; set; }
        public string ValorNotaDebito { get; set; }
        public string Observaciones { get; set; }
        public string FechaVencimientoObligacion { get; set; }
        public string TipoPorcentajeDescuento { get; set; }
        public string PorcentajeDescuento { get; set; }
        
        public string ValorPagoVendedor { get; set; }

        //public string Exchangemail { get; set; }
        //public string EventoPago { get; set; }
        //public string ValorPago { get; set; }
        //public string FechaPago { get; set; }
        //public string EstadoFirmaAppResponse { get; set; }
        //public string EstadoFirmaFactura { get; set; }
    }
}
