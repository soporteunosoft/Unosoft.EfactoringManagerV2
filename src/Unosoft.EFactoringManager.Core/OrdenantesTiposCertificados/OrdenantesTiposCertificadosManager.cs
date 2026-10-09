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

namespace Unosoft.EFactoringManager.OrdenantesTiposCertificados
{
    public class OrdenantesTiposCertificadosManager : IDomainService
    {

        #region Atributos
        private readonly IOrdenantesTiposCertificados _ordenantesTiposCertificados;
        #endregion

        public OrdenantesTiposCertificadosManager(IOrdenantesTiposCertificados ordenantesTiposCertificados)
        {
            _ordenantesTiposCertificados = ordenantesTiposCertificados;
        }


        public async Task<List<Identity.OrdenantesTiposCertificados>> GetTiposDocumentos(int tipoOrdenante)
        {
            return await _ordenantesTiposCertificados.GetAll()
                                        .Include(x => x.TiposOrdenantes)
                                        .Where(x => x.TipoOrdenante == tipoOrdenante)
                                        .ToListAsync();
        }
    }
}
