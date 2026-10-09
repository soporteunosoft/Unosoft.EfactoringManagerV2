using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;

namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IPronusRepositorio : IRepository<EFactoringManager.Identity.ConsolidadoOperaciones, int>
    {
        List<Identity.ConsolidadoOperaciones> ObtConsolidadoOperacionesPeriodo(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api);

        List<Identity.HistorialCuposPagadores> ObtHistorialCuposPagadores(string IdPagador, string FecIniPeriodo,
            string FecFinPeriodo, int Api);

        List<Identity.ResumenRecaudos> ObtResumenRecaudosClientes(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api);

        List<Identity.DetalleRecaudos> ObtDetalleRecaudosClientes(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api);
    }
}
