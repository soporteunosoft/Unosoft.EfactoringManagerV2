using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Configuration
{
    public class AppSettings
    {
        public bool NoCrearPagador { get; set; }
        public bool QuitarEstadoError { get; set; }
        public int OficinaTodos { get; set; }
        public bool AceptaPagador { get; set; }
        public List<int> Reportes { get; set; }
        public bool SolicitudOperacionesEstadoInicialEmisor { get; set; }
        public string IdCorreoNotificacionSolicitudOperacion { get; set; }
        public int CantidadPreguntasSeguridad { get; set; }
    }
}