using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Pronus.Dto
{
    [AutoMap()]
    public class ResumenVigentesDto
    {
        public int IdOficina { get; set; }
        public string IdCliente { get; set; }
        public string IdInversionista { get; set; }
        public string IdPagador { get; set; }
        public string IdComisionista { get; set; }
        public string FecIniPeriodo { get; set; }
        public string FecFinPeriodo { get; set; }
    }
}
