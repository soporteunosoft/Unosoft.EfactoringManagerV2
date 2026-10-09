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

namespace Unosoft.EFactoringManager.ModalidadesOperacion
{
    public class ModalidadesOperacionManager : IDomainService
    {

        #region Atributos
        private readonly IModalidadesOperacionRepositorio _modalidadesOperacionRepositorio;
        #endregion

        public ModalidadesOperacionManager(IModalidadesOperacionRepositorio modalidadesOperacionRepositorio)
        {
            _modalidadesOperacionRepositorio = modalidadesOperacionRepositorio;
        }

        public async Task<List<Identity.ModalidadesOperacion>> ObtenerModalidadesOpePorTipoOpe(int TipoOperacion)
        {
            return await _modalidadesOperacionRepositorio.GetAll().Where(a => a.TipoOperacion == TipoOperacion).Include(x => x.TasaInteres).ToListAsync();
        }

        public async Task<Identity.ModalidadesOperacion> ObtenerModalidadOpePorTipoOpe(int TipoOperacion)
        {
            return await _modalidadesOperacionRepositorio.GetAll().Where(a => a.TipoOperacion == TipoOperacion).Include(x => x.TasaInteres).FirstOrDefaultAsync();
        }
    }
}
