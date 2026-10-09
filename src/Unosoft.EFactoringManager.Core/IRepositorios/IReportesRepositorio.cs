using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;
namespace Unosoft.EFactoringManager.IRepositorios
{
    public interface IReportesRepositorio : IRepository<EFactoringManager.Identity.Reportes, int>
    {

        List<Identity.RespuestaConsultaReporte> ConsultarReporte(string query);

        DataSet GetDataReporte(string query);

    }
}
