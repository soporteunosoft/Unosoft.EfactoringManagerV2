using Abp.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class TituloRepositorio : EFactoringManagerRepositoryBase<Identity.Titulos, int>, ITituloRepositorio
    {
        public TituloRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override Identity.Titulos Get(int id)
        {
            return base.GetAll()
                .Where(x => x.Id == id)
                .Include(x => x.Pagador)
                .FirstOrDefault();
        }


        public List<Identity.Titulos> ObtenerTitulosPorFiltros(string IdTercero, string IdPagador, string FecRecepcionInicial, string FecRecepcionFinal, string FecFinalInicial, string FecFinalFinal, decimal VrNetoTituloInicial, decimal VrNetoTituloFinal,int TipoTitulo)
        {
          
                var IdTercero_ = new SqlParameter("@IdTercero", IdTercero);
                var IdPagador_ = new SqlParameter("@IdPagador", IdPagador);
                var FecRecepcionInicial_ = new SqlParameter("@FecRecepcionInicial", FecRecepcionInicial);
                var FecRecepcionFinal_ = new SqlParameter("@FecRecepcionFinal", FecRecepcionFinal);
                var FecFinalInicial_ = new SqlParameter("@FecFinalInicial", FecFinalInicial);
                var FecFinalFinal_ = new SqlParameter("@FecFinalFinal", FecFinalFinal);
                var VrNetoTituloInicial_ = new SqlParameter("@VrNetoTituloInicial", VrNetoTituloInicial);
                var VrNetoTituloFinal_ = new SqlParameter("@VrNetoTituloFinal", VrNetoTituloFinal);
                var TipoTitulo_ = new SqlParameter("@TipoTitulo", TipoTitulo);

                return GetContext().Titulos.FromSqlRaw("sp_ObtTitulosPorFiltros @IdTercero,@IdPagador,@FecRecepcionInicial,@FecRecepcionFinal,@FecFinalInicial, @FecFinalFinal,@VrNetoTituloInicial,@VrNetoTituloFinal,@TipoTitulo",
                        IdTercero_,
                        IdPagador_,
                        FecRecepcionInicial_,
                        FecRecepcionFinal_,
                        FecFinalInicial_,
                        FecFinalFinal_,
                        VrNetoTituloInicial_,
                        VrNetoTituloFinal_,
                        TipoTitulo_

                    ).ToList();
           
        }

        public bool EliminarTitulo(string idTitulo, int numeroTercero, int numeroPagador, int idSolicitud, int idOficina, string tipoDocumento)
        {
            var idTitulo_ = new SqlParameter("@IdTitulo", idTitulo);
            var numeroTercero_ = new SqlParameter("@NumeroTercero", numeroTercero);
            var numeroPagador_ = new SqlParameter("@NumeroPagador", numeroPagador);
            var idSolicitud_ = new SqlParameter("@IdSolicitud", idSolicitud);
            var idOficina_ = new SqlParameter("@IdOficina", idOficina);
            var tipoDocumento_ = new SqlParameter("@TipoDocumento", tipoDocumento);

           _ = GetContext().Database.ExecuteSqlRaw("sp_DelEliminarTitulo @IdTitulo, @NumeroTercero, @NumeroPagador, @IdSolicitud, @IdOficina, @TipoDocumento",
                    idTitulo_,
                    numeroTercero_,
                    numeroPagador_,
                    idSolicitud_,
                    idOficina_,
                    tipoDocumento_
                );

            return true;
        }
    }
}
