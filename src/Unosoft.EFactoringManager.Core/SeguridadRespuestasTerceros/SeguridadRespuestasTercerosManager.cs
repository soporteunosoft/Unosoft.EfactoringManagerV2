using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.SeguridadRespuestasTerceros
{
    public class SeguridadRespuestasTercerosManager : IDomainService
    {
        #region Atributos
        private readonly ISeguridadRespuestasTercerosRepositorio _seguridadRespuestasTercerosRepositorio;
        #endregion

        public SeguridadRespuestasTercerosManager(ISeguridadRespuestasTercerosRepositorio seguridadRespuestasTercerosRepositorio)
        {
            _seguridadRespuestasTercerosRepositorio = seguridadRespuestasTercerosRepositorio;
        }

        public async Task InsRespuestasTercero(Identity.SeguridadRespuestasTerceros seguridadRespuestasTerceros)
        {
            var respuesta = await _seguridadRespuestasTercerosRepositorio.GetAll().Where(a => a.IdTercero == seguridadRespuestasTerceros.IdTercero && a.IdPregunta == seguridadRespuestasTerceros.IdPregunta).FirstOrDefaultAsync();
            if (respuesta != null)
            {
                respuesta.Respuesta = seguridadRespuestasTerceros.Respuesta;
                respuesta.FecModifica = seguridadRespuestasTerceros.FecModifica;
                respuesta.IdUsrMod = seguridadRespuestasTerceros.IdUsrMod;
                await _seguridadRespuestasTercerosRepositorio.UpdateAsync(respuesta);
            }
            else
            {
                await _seguridadRespuestasTercerosRepositorio.InsertAsync(seguridadRespuestasTerceros);
            }
        }

        public async Task<bool> verificarRespuestasTercero(Identity.SeguridadRespuestasTerceros seguridadRespuestasTerceros)
        {
            var respuesta = await _seguridadRespuestasTercerosRepositorio.GetAll().Where(x => x.IdTercero == seguridadRespuestasTerceros.IdTercero && x.IdPregunta == seguridadRespuestasTerceros.IdPregunta).FirstOrDefaultAsync();
            if (respuesta != null)
                return seguridadRespuestasTerceros.Respuesta == respuesta.Respuesta;

            return false;
        }
    }
}
