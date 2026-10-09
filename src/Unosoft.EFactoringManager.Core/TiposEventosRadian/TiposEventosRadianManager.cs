using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposEventosRadian
{
    public class TiposEventosRadianManager : IDomainService
    {

        #region Atributos
        private readonly ITiposEventosRadianRepositorio _TiposEventosRadianRepositorio;
        #endregion

        public TiposEventosRadianManager(ITiposEventosRadianRepositorio tiposEventosRadianRepositorio)
        {
            _TiposEventosRadianRepositorio = tiposEventosRadianRepositorio;
        }


        public async Task<Identity.TiposEventosRadian> ObtenerTiposEventosRadianPorTipo(string TipoEvento)
        {
            return await _TiposEventosRadianRepositorio.GetAll()
                                                 .Include(x => x.Notas)
                                                 .ThenInclude(x => x.TiposNotasRadian)
                                                 .Include(x => x.CamposObligatorios)
                                                 .Where(a => a.TipoEvento == TipoEvento).FirstOrDefaultAsync();
        }

        public async Task<List<Identity.TiposEventosRadian>> ObtenerTiposEventosRadian()
        {
            return await _TiposEventosRadianRepositorio.GetAll().ToListAsync();
        }

    }
}
