using Abp.Domain.Entities;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.Billy
{
    public class BillyProcessManager : IDomainService
    {
        #region Atributos
        private readonly IProcesoEndosoBillyRepositorio _procesoEndosoBillyRepositorio;
        #endregion

        #region Constructor de la clase
        public BillyProcessManager(
            IProcesoEndosoBillyRepositorio procesoEndosoBillyRepositorio)
        {
            _procesoEndosoBillyRepositorio = procesoEndosoBillyRepositorio;
        }
        #endregion

        public async Task Insertar(Identity.ProcesoEndosoBilly entity)
        {
            await _procesoEndosoBillyRepositorio.InsertAsync(entity);
        }

        public async Task Actualizar(Identity.ProcesoEndosoBilly entity)
        {
            await _procesoEndosoBillyRepositorio.UpdateAsync(entity);
        }

        public async Task<Identity.ProcesoEndosoBilly> Get(string tipoDocumento, int idOficina, int idSolicitud, string cufe)
        {
            return await _procesoEndosoBillyRepositorio.GetAll().Where(x => x.TipoDocumento == tipoDocumento &&
                                                              x.IdOficina == idOficina &&
                                                              x.IdSolicitud == idSolicitud &&
                                                              x.Cufe == cufe).FirstOrDefaultAsync();
        }

        //public List<Identity.ProcesoEndosoBilly> GetByEstado(int estado, DateTime? fechaProceso)
        //{
        //    return _procesoEndosoBillyRepositorio.GetAll().Where(x => x.Estado == estado 
        //                                                     && x.FechaProceso < fechaProceso).ToList();
           
        //}

        public List<Identity.ProcesoEndosoBilly> GetByEstado(int estado, DateTime? fechaProceso)
        {
            return _procesoEndosoBillyRepositorio.GetList(estado, fechaProceso);

        }



    }
}
