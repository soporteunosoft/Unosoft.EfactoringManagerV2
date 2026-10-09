using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.Cupos
{
    public class CuposManager : IDomainService
    {
        #region Atributos
        private readonly ICuposRepositorio _cuposRepositorio;
        private readonly Terceros.TercerosManager _tercerosManager;
        #endregion

        public CuposManager(ICuposRepositorio cuposRepositorio, 
                            Terceros.TercerosManager tercerosManager
                            )
        {
            _cuposRepositorio = cuposRepositorio;
            _tercerosManager = tercerosManager;
        }

        //public List<Identity.Cupos> ObtenerCuposPorTercero(string idTercero)
        //{
        //    var tercero = _tercerosManager.ObtenerTerceroPorId(idTercero);

        //    if (tercero == null)
        //        return null;

        //    return _cuposRepositorio.GetAll().Where(x => x.IdTercero == tercero.TerceroPadre.IdTercero).Include(x=>x.TiposOperaciones).ToList();
        //}




        public async Task<List<Identity.Cupos>> ObtenerCuposPorTercero(int numeroTercero)
        {
            return await _cuposRepositorio.GetAll().Where(x => x.NumeroTercero == numeroTercero).Include(x => x.TiposOperaciones).ToListAsync();
        }

    }
}
