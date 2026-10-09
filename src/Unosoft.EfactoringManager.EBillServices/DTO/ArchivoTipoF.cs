using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.EBillServices.DTO
{
    [DelimitedRecord("|"), IgnoreFirst(1)]
    //[DelimitedRecord(","), IgnoreFirst(1)]
    public class ArchivoTipoF
    {
        public string NombredelArchivo { get; set; }
        public string EstadoEndoso { get; set; }
        public string CodigoError { get; set; }
        public string Mensaje { get; set; }
        public string IdFactura { get; set; }
        public string NitAdquiriente { get; set; }
        public string DVAdquiriente { get; set; }
        public string RazonSocialAdquiriente { get; set; }
        public string NitEmpresaEmisora { get; set; }
        public string DVEmisor { get; set; }
        public string RazonSocialEmpresaEmisora { get; set; }
        public string EstadoDIAN { get; set; }
        public string Prefijo { get; set; }
        public string NumeroFactura { get; set; }
        public string TotalAPagar { get; set; }
        public string DescripcionEstadoCUFE { get; set; }
        public string FechaEntregaProductoBienServicio { get; set; }
        public string FechaExpedicion { get; set; }
        public string FechaVencimiento { get; set; }
        public string FacturaVencida { get; set; }
        public string FechaGeneracionFactura { get; set; }
        public string EnvioCorreo { get; set; }
        public string IDEvento { get; set; }
        public string CUDE { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }

    }
}
