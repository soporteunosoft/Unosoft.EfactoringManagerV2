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
    public class PagadoresTitulosSol
    {   
        public string IdPagador { get; set; }
        public string Pagador { get; set; }
        [Key]
        public string Titulos { get; set; }
        public string IdCliente { get; set; }
        public string Cliente { get; set; }
        public string Ordenante { get; set; }
        public string IdOrdenante { get; set; }
        public int IdCuentaRecaudadora { get; set; }
        public string DirOficinaPag { get; set; }
        public string OrdenantePagador { get; set; }
        public string ProfesionOrdPag { get; set; }
        public string CiudadPagador { get; set; }
        public string NumCuenta { get; set; }
        public string NomCuenta { get; set; }
        public string CiudadExpOrdCli { get; set; }


    }
}
