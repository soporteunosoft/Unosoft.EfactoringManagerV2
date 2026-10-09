using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.EstadosTitulos.Dto
{
    [AutoMap(typeof(Identity.EstadosTitulos))]
    public class EstadosTitulosDto
    {
        public int Id { get; set; }
        public string NomEstadoTitulo { get; set; }
        public string DesEstadoTitulo { get; set; }
    }
}
