using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Identity;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.Oficina
{
    public class OficinaManager : IDomainService
    {
        private readonly IOficinasRepositorio oficinaRepositorio;
        public OficinaManager(IOficinasRepositorio _oficinaRepositorio)
        {
            oficinaRepositorio = _oficinaRepositorio;
        }

        public async Task<Oficinas> Get(int id)
        {
            return await oficinaRepositorio.GetAll().FirstOrDefaultAsync(x => x.IdOficina == id);
        }

        public async Task Update(Oficinas oficina)
        {
            await oficinaRepositorio.UpdateAsync(oficina);
        }

        public async Task<List<Identity.Oficinas>> ObtOficinas(int idOficina)
        {
            return await oficinaRepositorio.GetAll().Where(a => a.IdOficina == idOficina || idOficina == 0).ToListAsync();
        }

    }
}
