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

namespace Unosoft.EFactoringManager.ImpDocumentosOperativos
{
    public class ImpDocumentosOperativosManager : IDomainService
    {
        #region Atributos
        private readonly IImpDocumentosOperativosRepositorio _impDocumentosOperativosRepositorio;
        #endregion

        public ImpDocumentosOperativosManager(IImpDocumentosOperativosRepositorio impDocumentosOperativosRepositorio)
        {
            _impDocumentosOperativosRepositorio = impDocumentosOperativosRepositorio;
        }

        public async Task<List<Identity.ImpDocumentosOperativos>> ObtenerImpDocumentosOperativos(string tipoDocumento, int idOficina, string IdConcepto)
        {
            return await _impDocumentosOperativosRepositorio.GetAll().Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdConcepto == IdConcepto).ToListAsync();
        }
    }
}
