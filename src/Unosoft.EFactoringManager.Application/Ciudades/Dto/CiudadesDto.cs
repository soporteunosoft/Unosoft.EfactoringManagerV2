using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Unosoft.EFactoringManager.Ciudades.Dto
{
    [AutoMap(typeof(Identity.Ciudades))]
    public class CiudadesDto
    {
        public int IdCiudad { get; set; }
        public string NomCiudad { get; set; }
        public int IdPais { get; set; }
        public int IdDepartamento { get; set; }
      
    }
}
