
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.EstadosTitulosCore
{
    public class EstadosTitulosManager : IDomainService
    {
        #region Atributos
        private readonly IEstadosTitulosRepositorio _estadosTitulosRepositorio;
        #endregion

        #region Constructor de la clase
        public EstadosTitulosManager(
                IEstadosTitulosRepositorio estadosTitulosRepositorio

            )
        {
            _estadosTitulosRepositorio = estadosTitulosRepositorio;      
         
        }
        #endregion


        public async Task<List<Identity.EstadosTitulos>> ObtenerEstadosTitulos()
        {
            return await _estadosTitulosRepositorio.GetAll().ToListAsync();
        }

        public async Task<Identity.EstadosTitulos> ObtEstadoTituloPorId(int id)
        {
            return await _estadosTitulosRepositorio.GetAll().Where(a => a.Id == id).FirstOrDefaultAsync();
        }
    }
}


