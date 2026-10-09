using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.DetalleSolicitudes.DTO;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.Terceros.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Unosoft.EFactoringManager.DetalleSolicitudes
{
    public class DetalleSolicitudesServices : ApplicationService, IDetalleSolicitudesServices
    {
        private readonly DetalleSolicitudesManager _detalleSolicitudesManager;
        private readonly UserManager _userManager;
        Terceros.TercerosManager _tercerosManager;
        Titulos.TitulosManager _titulosManager;
        public DetalleSolicitudesServices(DetalleSolicitudesManager detalleSolicitudesManager,
                                                 UserManager userManager, Terceros.TercerosManager tercerosManager,
                                                 Titulos.TitulosManager titulosManager)
        {
            _detalleSolicitudesManager = detalleSolicitudesManager;
            _userManager = userManager;
            _tercerosManager = tercerosManager;
            _titulosManager = titulosManager;
        }

        public async Task<List<DetalleSolicitudDTO>> ObtDetalleSolicitudes(ObtDetalleSolicitudesRequest request)
        {
            try
            {
                long idUsuario = AbpSession.UserId.Value;
                User user = await _userManager.GetUserByIdAsync(idUsuario);
                var tercero = await _tercerosManager.ObtenerTerceroPorId(user.UserName);

                var lstDetalleSolicitudes = await _detalleSolicitudesManager.ObtDetalleSolicitudesXPagador(tercero.TerceroPadre != null ? tercero.TerceroPadre.Id : 0, request.TipoDocumento, request.Producto, request.FechaInicial, request.FechaFinal, request.EstadoTitulo, request.IdSolicitud, request.IdTitulo);
                return lstDetalleSolicitudes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<bool> InsertDetalleSolicitudes(InsertDetalleSolicitudes request)
        {
            try
            {
                var titulo = await _titulosManager.ObtenerTituloPorId(request.Terceros.Id, request.IdTitulo, 1);
                List<Identity.Titulos> _titulos = new List<Identity.Titulos> { titulo };

                await _detalleSolicitudesManager.Insertar(request.TipoDocumento, request.IdOficina, request.IdSolicitud, _titulos, request.Terceros);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
