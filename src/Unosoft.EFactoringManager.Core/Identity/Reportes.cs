using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("Reportes", Schema = "dbo")]
    public class Reportes : Entity<int>
    {

        /*
         * EnviadoId	int	Unchecked
NomReporte	varchar(50)	Checked
DirReporte	varchar(500)	Checked
Consulta	varchar(500)	Checked
TipoLinea	int	Checked
TipoTercero	int	Checked
ClaseDocumento	int	Checked
TipoDocumento	char(3)	Checked
TipoOpcion	int	Checked
NamePremissions	nvarchar(128)	Checked
IdUsrMod	varchar(8)	Checked
FecModifica	smalldatetime	Checked
		Unchecked
         * */

        public string NomReporte { get; set; }
        public string DirReporte { get; set; }
        public int? TipoTercero { get; set; }
        public string Consulta { get; set; }
        public string ConsultaReporte { get; set; }
        public string ConsultaParametrosReporte { get; set; }
        //public int? ClaseDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int? TipoOpcion { get; set; }
        public int? ClaseDocumento { get; set; }
        public bool Activa { get; set; }
        public int? TipoFormato { get; set; }
        public string IdUsrMod { get; set; }
        //to do; agregar campos permisos
        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }
    }
}
