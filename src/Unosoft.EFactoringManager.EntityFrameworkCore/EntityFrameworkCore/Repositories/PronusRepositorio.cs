using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class PronusRepositorio : EFactoringManagerRepositoryBase<Identity.ConsolidadoOperaciones, int>, IPronusRepositorio
    {
        public PronusRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public List<Identity.ConsolidadoOperaciones> ObtConsolidadoOperacionesPeriodo(int IdOficina,
            string IdCliente, string IdInversionista,string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            var IdOficina_ = new SqlParameter("@IdOficinaFiltro", IdOficina);
            var IdCliente_ = new SqlParameter("@IdClienteFiltro", IdCliente);
            var IdInversionista_ = new SqlParameter("@IdInversionistaFiltro", IdInversionista);
            var IdPagador_ = new SqlParameter("@IdPagadorFiltro", IdPagador);
            var IdComisionista_ = new SqlParameter("@IdComisionistaFiltro", IdComisionista);
            var FecIniPeriodo_ = new SqlParameter("@FecIniPeriodo", FecIniPeriodo);
            var FecFinPeriodo_ = new SqlParameter("@FecFinPeriodo", FecFinPeriodo);
            var Api_ = new SqlParameter("@Api", Api);

            
            var consulta = GetContext().ConsolidadoOperaciones.FromSqlRaw("sp_ObtApisPronus @IdOficinaFiltro, @IdClienteFiltro, @IdInversionistaFiltro, @IdPagadorFiltro, @IdComisionistaFiltro, @FecIniPeriodo, @FecFinPeriodo, @Api",
                IdOficina_,
                IdCliente_,
                IdInversionista_,
                IdPagador_,
                IdComisionista_,
                FecIniPeriodo_,
                FecFinPeriodo_,
                Api_
            ).ToList();

            return consulta;
        }

        public List<Identity.HistorialCuposPagadores> ObtHistorialCuposPagadores(string IdPagador, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            var IdPagador_ = new SqlParameter("@IdPagadorFiltro", IdPagador);
            var FecIniPeriodo_ = new SqlParameter("@FecIniPeriodo", FecIniPeriodo);
            var FecFinPeriodo_ = new SqlParameter("@FecFinPeriodo", FecFinPeriodo);
            var Api_ = new SqlParameter("@Api", Api);


            var consulta = GetContext().HistorialCuposPagadores.FromSqlRaw("sp_ObtApisPronus 0, '', '', @IdPagadorFiltro, '', @FecIniPeriodo, @FecFinPeriodo, @Api",
                IdPagador_,
                FecIniPeriodo_,
                FecFinPeriodo_,
                Api_
            ).ToList();

            return consulta;
        }

        public List<Identity.ResumenRecaudos> ObtResumenRecaudosClientes(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            var IdOficina_ = new SqlParameter("@IdOficinaFiltro", IdOficina);
            var IdCliente_ = new SqlParameter("@IdClienteFiltro", IdCliente);
            var IdInversionista_ = new SqlParameter("@IdInversionistaFiltro", IdInversionista);
            var IdPagador_ = new SqlParameter("@IdPagadorFiltro", IdPagador);
            var IdComisionista_ = new SqlParameter("@IdComisionistaFiltro", IdComisionista);
            var FecIniPeriodo_ = new SqlParameter("@FecIniPeriodo", FecIniPeriodo);
            var FecFinPeriodo_ = new SqlParameter("@FecFinPeriodo", FecFinPeriodo);
            var Api_ = new SqlParameter("@Api", Api);


            var consulta = GetContext().ResumenRecaudos.FromSqlRaw("sp_ObtApisPronus @IdOficinaFiltro, @IdClienteFiltro, @IdInversionistaFiltro, @IdPagadorFiltro, @IdComisionistaFiltro, @FecIniPeriodo, @FecFinPeriodo, @Api",
                IdOficina_,
                IdCliente_,
                IdInversionista_,
                IdPagador_,
                IdComisionista_,
                FecIniPeriodo_,
                FecFinPeriodo_,
                Api_
            ).ToList();

            return consulta;
        }

        public List<Identity.DetalleRecaudos> ObtDetalleRecaudosClientes(int IdOficina,
            string IdCliente, string IdInversionista, string IdPagador, string IdComisionista, string FecIniPeriodo,
            string FecFinPeriodo, int Api)
        {

            var IdOficina_ = new SqlParameter("@IdOficinaFiltro", IdOficina);
            var IdCliente_ = new SqlParameter("@IdClienteFiltro", IdCliente);
            var IdInversionista_ = new SqlParameter("@IdInversionistaFiltro", IdInversionista);
            var IdPagador_ = new SqlParameter("@IdPagadorFiltro", IdPagador);
            var IdComisionista_ = new SqlParameter("@IdComisionistaFiltro", IdComisionista);
            var FecIniPeriodo_ = new SqlParameter("@FecIniPeriodo", FecIniPeriodo);
            var FecFinPeriodo_ = new SqlParameter("@FecFinPeriodo", FecFinPeriodo);
            var Api_ = new SqlParameter("@Api", Api);


            var consulta = GetContext().DetalleRecaudos.FromSqlRaw("sp_ObtApisPronus @IdOficinaFiltro, @IdClienteFiltro, @IdInversionistaFiltro, @IdPagadorFiltro, @IdComisionistaFiltro, @FecIniPeriodo, @FecFinPeriodo, @Api",
                IdOficina_,
                IdCliente_,
                IdInversionista_,
                IdPagador_,
                IdComisionista_,
                FecIniPeriodo_,
                FecFinPeriodo_,
                Api_
            ).ToList();

            return consulta;
        }
    }
}