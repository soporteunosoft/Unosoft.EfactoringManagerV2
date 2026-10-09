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

namespace Unosoft.EFactoringManager.Vinculaciones
{
    public class VinculacionesManager : IDomainService
    {
        #region Atributos
        private readonly IVinculacionesRepositorio _vinculacionesRepositorio;
        private readonly IDetalleVinculacionesRepositorio _detalleVinculacionesRepositorio;
        #endregion

        #region Constructor de la clase
        public VinculacionesManager(
                IVinculacionesRepositorio vinculacionesRepositorio,
                IDetalleVinculacionesRepositorio detalleVinculacionesRepositorio
            )
        {
            _vinculacionesRepositorio = vinculacionesRepositorio;
            _detalleVinculacionesRepositorio = detalleVinculacionesRepositorio;
        }
        #endregion

        public async Task<bool> InsertarViculacion(Identity.Vinculaciones vinculacion)
        {
            vinculacion.FecVinculacion = DateTime.Now;
            vinculacion.FecModifica = DateTime.Now;
            vinculacion.HoraModifica = DateTime.Now.ToString("HH:mm:ss");
            vinculacion.IdUsrMod = "FM";
            await _vinculacionesRepositorio.InsertAsync(vinculacion);
            return true;
        }

        public async Task<bool> ActualizarVinculacion(Identity.Vinculaciones vinculacion)
        {
            var _vinculacion = await _vinculacionesRepositorio.GetAll().Where(a => a.Identificacion == vinculacion.Identificacion).FirstOrDefaultAsync();
            if (_vinculacion == null)
            {
                return false;
            }
            else
            {
                _vinculacion.RazonSocial = vinculacion.RazonSocial;
                _vinculacion.DirOficina = vinculacion.DirOficina;
                _vinculacion.IdCiudad = vinculacion.IdCiudad;
                _vinculacion.TelOficina = vinculacion.TelOficina;
                _vinculacion.Email = vinculacion.Email;
                _vinculacion.NomContacto = vinculacion.NomContacto;
                _vinculacion.CargoCon = vinculacion.CargoCon;
                await _vinculacionesRepositorio.UpdateAsync(_vinculacion);
                return true;
            }
        }

        public async Task<bool> ActualizarEstadoVinculacion(Identity.Vinculaciones vinculacion)
        {
            var _vinculacion = await _vinculacionesRepositorio.GetAll().Where(a => a.Identificacion == vinculacion.Identificacion).FirstOrDefaultAsync();
            if (_vinculacion == null)
            {
                return false;
            }
            else
            {
                _vinculacion.GestionVinculacion = !vinculacion.GestionVinculacion;
                if (!vinculacion.GestionVinculacion.Value)
                    _vinculacion.FecGestion = DateTime.Now;
                else
                    _vinculacion.FecGestion = null;

                _vinculacion.FecModifica = DateTime.Now;
                _vinculacion.HoraModifica = DateTime.Now.ToString("HH:mm:ss");
                _vinculacion.IdUsrMod = vinculacion.IdUsrMod;
                await _vinculacionesRepositorio.UpdateAsync(_vinculacion);
                return true;
            }
        }

        public async Task<(List<Identity.Vinculaciones> Data, int Total)> ObtenerVinculaciones(int skip = 0,
                                                                int limit = 0,
                                                                string sortField = "",
                                                                int sortOrder = -1,
                                                                string filter = "")
        {
            var query = _vinculacionesRepositorio.GetAll();

            if (!string.IsNullOrEmpty(filter?.Trim()))
            {
                query = query.Where(x => x.RazonSocial.Contains(filter.Trim()) ||
                                         x.Identificacion.Contains(filter.Trim()));
            }

            var total = await query.CountAsync();

            if (skip > 0)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return (await query.ToListAsync(), total);
        }


    }
}
