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

namespace Unosoft.EFactoringManager.Ciudades
{

    public class CiudadesManager : IDomainService
    {
        #region Atributos
        private readonly ICiudadesRepositorio _ciudadesRepositorio;
        #endregion

        public CiudadesManager(ICiudadesRepositorio ciudadesRepositorio)
        {
            _ciudadesRepositorio = ciudadesRepositorio;
        }

        public async Task<List<Identity.Ciudades>> ObtenerCiudades(string nomCiudad)
        {
            return await _ciudadesRepositorio.GetAll().Where(a => a.NomCiudad.Contains(nomCiudad)).Include(x => x.Pais).Include(x => x.Departamento).ToListAsync();
        }

        public async Task<Identity.Ciudades> ObntenerCiudadPorId(int idCiudad)
        {
            return await _ciudadesRepositorio.GetAll().Where(a => a.IdCiudad == idCiudad).FirstOrDefaultAsync();
        }

        public async Task<Identity.Ciudades> ObntenerCiudadPorCodDane(string codDane)
        {
            return await _ciudadesRepositorio.GetAll().Where(a => a.CodDaneCiudad == codDane).FirstOrDefaultAsync();
        }
    }
}
