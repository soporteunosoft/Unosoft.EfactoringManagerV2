using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Titulos.Dto
{
    [AutoMap(typeof(Identity.Titulos))]
    public class TituloDto
    {
        public int NumeroTercero { get; set; }
        public string IdTitulo { get; set; }
        public int TipoTitulo { get; set; }        
        public int NumeroPagador { get; set; }
        public decimal? VrNetoTitulo { get; set; }
        public DateTime FecInicial { get; set; }
        public DateTime FecFinal { get; set; }
        public DateTime? FecProbable { get; set; }
        public int? Dias { get; set; }
        public string CufeTitulo { get; set; }
        public string RepresentacionGrafica { get; set; }
        public string RepresentacionXml { get; set; }
        public Identity.Terceros Pagador { get; set; }
        public Identity.Terceros Tercero { get; set; }
        public Identity.EstadosTitulos EstadosTitulos { get; set; }
        public bool Vencida { get; set; }        
        public string DescripcionEstadoCUFE { get; set; }
        public bool AcuseRecibo { get; set; }
        public bool FecEntregaValidoNegociar { get; set; }
        public bool AceptacionTacita { get; set; }
        public bool AceptacionExpresa { get; set; }
        public bool InscritaFEcomoTV { get; set; }
        public bool Endoso { get; set; }       
        public string EndosadoA { get; set; }       
        public string CodigoError { get; set; }       
        public string MensajeProcesamiento { get; set; }      
        public bool FacturaVencida { get; set; }
        public string CudeTitulo { get; set; }

        public bool FechaEntregaProductoServicio { get; set; }



    }
}
