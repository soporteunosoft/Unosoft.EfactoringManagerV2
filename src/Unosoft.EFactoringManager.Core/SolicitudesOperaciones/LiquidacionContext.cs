using System;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones
{
    public class LiquidacionContext
    {
        // Impuestos
        public float TasaIva { get; set; }
        public float TasaRetefuente { get; set; }
        public float TasaReteIca { get; set; }
        public float TasaReteIva { get; set; }
        public float TasaGao { get; set; }
        public decimal BaseReteFuente { get; set; }
        public decimal BaseReteIca { get; set; }
        public decimal ReteFuenteIntereses { get; set; }
        public decimal ReteIcaIntereses { get; set; }

        // Totales de liquidación
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

        public void ResetTotals()
        {
            TotalVrAdmon = 0;
            TotalVrEntregarBruto = 0;
            TotalVrEntregarNeto = 0;
            TotalVrIntereses = 0;
            TotalReteFuenteInteres = 0;
            TotalVrNetoRecibir = 0;
            TotalVrComisionBruto = 0;
            IvaComision = 0;
            ReteFuenteComision = 0;
            ReteIcaComision = 0;
            ReteIvaComision = 0;
            TotalVrComisionNeto = 0;
            TotalVrNetoTitulo = 0;
            ReteFuenteIntereses = 0;
            ReteIcaIntereses = 0;
            BaseReteFuente = 0;
            BaseReteIca = 0;
        }
    }
}
