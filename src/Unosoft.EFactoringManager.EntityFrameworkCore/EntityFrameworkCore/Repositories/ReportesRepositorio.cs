using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Unosoft.EFactoringManager.IRepositorios;
using Abp.EntityFrameworkCore;
using Unosoft.EFactoringManager.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class ReportesRepositorio : EFactoringManagerRepositoryBase<Identity.Reportes, int>, IReportesRepositorio
    {
        public ReportesRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public DataSet GetDataReporte(string query)
        {
            DataSet result = new DataSet();

         
            using (SqlConnection conn = new SqlConnection(((SqlConnection)GetContext().Database.GetDbConnection()).ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        //cmd.Transaction = (SqlTransaction)GetContext().Database.CurrentTransaction.GetDbTransaction();
                        da.SelectCommand = cmd;
                        

                        ///conn.Open();
                        da.Fill(result);
                        //cmd.Transaction.Commit();
                        
                    }
                    catch(Exception ex) { throw new Exception($"{ex.ToString()} con :{conn.ConnectionString} "); }
                    finally { conn.Close(); }
                }
            }

            return result;


        }


        public List<Identity.RespuestaConsultaReporte> ConsultarReporte(string query)
        {

            return GetContext().RespuestaConsultaReporte.FromSqlRaw(query).ToList();

        }

      

       
    }

    public static class EFExtensions
    {
        public static DbTransaction GetDbTransaction(this IDbContextTransaction source)
        {
            return (source as IInfrastructure<DbTransaction>).Instance;
        }
    }

}
