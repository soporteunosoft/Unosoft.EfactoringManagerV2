using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface ITituloRepositorio : IRepository<EFactoringManager.Identity.Titulos, int>
    {

       List<Identity.Titulos> ObtenerTitulosPorFiltros(string IdTercero, string IdPagador, string FecRecepcionInicial, string FecRecepcionFinal, string FecFinalInicial, string FecFinalFinal, decimal VrNetoTituloInicial, decimal VrNetoTituloFinal, int TipoTitulo);
       bool EliminarTitulo(string idTitulo, int numeroTercero, int numeroPagador, int idSolicitud, int idOficina, string tipoDocumento);
    }

}
