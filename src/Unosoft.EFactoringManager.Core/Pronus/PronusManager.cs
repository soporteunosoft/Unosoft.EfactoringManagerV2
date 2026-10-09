using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Terceros;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.Pronus
{
    public class PronusManager : IDomainService
    {
        #region Atributos
        private readonly IPronusRepositorio _pronusRepositorio;
        private List<Identity.ConsolidadoOperaciones> resultado = new List<Identity.ConsolidadoOperaciones>();
        private List<Identity.HistorialCuposPagadores> resultado2 = new List<Identity.HistorialCuposPagadores>();
        private List<Identity.ResumenRecaudos> resultado3 = new List<Identity.ResumenRecaudos>();
        private List<Identity.DetalleRecaudos> resultado4 = new List<Identity.DetalleRecaudos>();
        #endregion

        public PronusManager(IPronusRepositorio pronusRepositorio)
        {
            _pronusRepositorio = pronusRepositorio;
        }

        public List<Identity.ConsolidadoOperaciones> ObtConsolidadoOperacionesPeriodo(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            resultado = _pronusRepositorio.ObtConsolidadoOperacionesPeriodo(IdOficina, IdCliente, IdInversionista,
            IdPagador, IdComisionista, $"{FecIniPeriodo:dd/MM/yyyy}", $"{FecFinPeriodo:dd/MM/yyyy}", Api);

            return resultado;
        }

        public List<Identity.HistorialCuposPagadores> ObtHistorialCuposPagadores(string IdPagador, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            resultado2 = _pronusRepositorio.ObtHistorialCuposPagadores(IdPagador, $"{FecIniPeriodo:dd/MM/yyyy}", $"{FecFinPeriodo:dd/MM/yyyy}", Api);

            return resultado2;
        }

        public List<Identity.ResumenRecaudos> ObtResumenRecaudosClientes(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            resultado3 = _pronusRepositorio.ObtResumenRecaudosClientes(IdOficina, IdCliente, IdInversionista,
            IdPagador, IdComisionista, $"{FecIniPeriodo:dd/MM/yyyy}", $"{FecFinPeriodo:dd/MM/yyyy}", Api);

            return resultado3;
        }

        public List<Identity.DetalleRecaudos> ObtDetalleRecaudosClientes(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            resultado4 = _pronusRepositorio.ObtDetalleRecaudosClientes(IdOficina, IdCliente, IdInversionista,
            IdPagador, IdComisionista, $"{FecIniPeriodo:dd/MM/yyyy}", $"{FecFinPeriodo:dd/MM/yyyy}", Api);

            return resultado4;
        }
    }
}
