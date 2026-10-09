using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.SeguridadPreguntasTerceros
{
    public class SeguridadPreguntasTercerosManager : IDomainService
    {
        #region Atributos
        private readonly ISeguridadPreguntasTercerosRepositorio _seguridadPreguntasTercerosRepositorio;
        private readonly AppSettings _settings;
        #endregion

        public SeguridadPreguntasTercerosManager(
            ISeguridadPreguntasTercerosRepositorio seguridadPreguntasTercerosRepositorio,
            IOptions<AppSettings> settings)
        {
            _seguridadPreguntasTercerosRepositorio = seguridadPreguntasTercerosRepositorio;
            _settings = settings.Value;
        }

        public async Task<List<Identity.SeguridadPreguntasTerceros>> ObtenerPreguntasTercero()
        {
            return await _seguridadPreguntasTercerosRepositorio.GetAll()
                .Where(s => s.Activa)
                .OrderBy(r => Guid.NewGuid())
                .ToListAsync();
        }

        public async Task<List<Identity.SeguridadPreguntasTerceros>> ObtenerPreguntasTerceroAleatorio()
        {
            int cantidad = _settings.CantidadPreguntasSeguridad > 0 ? _settings.CantidadPreguntasSeguridad : 3;
            return await _seguridadPreguntasTercerosRepositorio.GetAll()
                .Where(s => s.Activa)
                .OrderBy(r => Guid.NewGuid())
                .Take(cantidad)
                .ToListAsync();
        }
    }
}
