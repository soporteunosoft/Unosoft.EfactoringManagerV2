using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Titulos.Dto
{
    [AutoMap()]
    public class TituloInputDto
    {
        //public string IdTercero { get; set; }
        public int NumeroTercero { get; set; }
        public string IdCliente { get; set; }
        public string IdPagador { get; set; }
        public string FecRecepcionInicial { get; set; }
        public string FecRecepcionFinal { get; set; }
        public string FecFinalInicial { get; set; }
        public string FecFinalFinal { get; set; }
        public decimal VrNetoTituloInicial { get; set; }
        public decimal VrNetoTituloFinal { get; set; }
        public int? TipoTitulo { get; set; }
    }
}
