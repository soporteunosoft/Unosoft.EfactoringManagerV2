using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.ModalidadesOperacion.Dto
{

    [AutoMap(typeof(Identity.TiposOperaciones))]
    public class TipoOperacionDto
    {
        public int TipoOperacion { get; set; }
        public string NomOperacion { get; set; }
        public int TipoLinea { get; set; }
        public int TipoTitulo { get; set; }
        public string IdCuentaContable { get; set; }
        public bool Activa { get; set; }
        public int? DiasMaxMora { get; set; }
        public bool Pagador { get; set; }
    }
}
