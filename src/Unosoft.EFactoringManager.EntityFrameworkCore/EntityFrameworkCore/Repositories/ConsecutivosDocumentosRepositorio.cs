using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Unosoft.EFactoringManager.IRepositorios;
using Abp.EntityFrameworkCore;
using Unosoft.EFactoringManager.Identity;
using Microsoft.EntityFrameworkCore;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Repositories
{
    public class ConsecutivosDocumentosRepositorio : EFactoringManagerRepositoryBase<Identity.ConsecutivosDocumentos, int>, IConsecutivosDocumentosRepositorio
    {
        public ConsecutivosDocumentosRepositorio(IDbContextProvider<EFactoringManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
