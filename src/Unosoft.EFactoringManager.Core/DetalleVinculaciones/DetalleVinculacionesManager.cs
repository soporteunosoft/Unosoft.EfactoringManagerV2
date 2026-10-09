using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.DetalleVinculaciones
{
    public class DetalleVinculacionesManager : IDomainService
    {
        #region Atributos
        private readonly IDetalleVinculacionesRepositorio _detalleVinculacionesRepositorio;
        #endregion

        #region Constructor de la clase
        public DetalleVinculacionesManager(IDetalleVinculacionesRepositorio detalleVinculacionesRepositorio)
        {
            _detalleVinculacionesRepositorio = detalleVinculacionesRepositorio;
        }
        #endregion

        public async Task insertarDetalleVin(Identity.DetalleVinculaciones detalleVinculaciones)
        {
            await _detalleVinculacionesRepositorio.InsertAsync(detalleVinculaciones);
        }

        public async Task<bool> actualizarDetalleVin(Identity.DetalleVinculaciones detalleVinculaciones)
        {
            var _Detallevinculacion = await _detalleVinculacionesRepositorio.GetAll().Where(a => a.TipoDocumento == detalleVinculaciones.TipoDocumento && a.Identificacion == detalleVinculaciones.Identificacion).FirstOrDefaultAsync();
            if(_Detallevinculacion==null)
            {
                return false;
            }
            else
            {
                _Detallevinculacion.NomTercero = detalleVinculaciones.NomTercero;
                _Detallevinculacion.IdCiudad = detalleVinculaciones.IdCiudad;
                _Detallevinculacion.VrVentasMensuales = detalleVinculaciones.VrVentasMensuales;
                _Detallevinculacion.VrPromedioFactura = detalleVinculaciones.VrPromedioFactura;
                _Detallevinculacion.Plazo = detalleVinculaciones.Plazo;
                _Detallevinculacion.PlazoRealPago = detalleVinculaciones.PlazoRealPago;
                _Detallevinculacion.PorcCompras = detalleVinculaciones.PorcCompras;
                await _detalleVinculacionesRepositorio.InsertAsync(_Detallevinculacion);
                return true;
            }
        }
    }
}
