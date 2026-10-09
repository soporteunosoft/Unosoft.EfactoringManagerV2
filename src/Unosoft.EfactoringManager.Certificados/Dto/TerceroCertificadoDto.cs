using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EfactoringManager.Certificados.Dto
{

    public class TerceroCertificadoDto
    {
        public int Id { get; set; }
        public string IdTercero { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string NumSolicitud { get; set; }
        public string NombreTercero { get; set; }
        public bool Vigente { get; set; }
        public int TipoCertificado { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaCert { get; set; }
    }
}
