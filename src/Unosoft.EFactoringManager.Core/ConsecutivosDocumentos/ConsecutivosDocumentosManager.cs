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

namespace Unosoft.EFactoringManager.ConsecutivosDocumentos
{
    public class ConsecutivosDocumentosManager : IDomainService
    {

        #region Atributos
        private readonly IConsecutivosDocumentosRepositorio _consecutivosDocumentosRepositorio;
        #endregion

        public ConsecutivosDocumentosManager(IConsecutivosDocumentosRepositorio consecutivosDocumentosRepositorio)
        {
            _consecutivosDocumentosRepositorio = consecutivosDocumentosRepositorio;
        }

        public async Task<int> ActualizarConsecutivo(string tipoDocumento, int idOficina)
        {
            var _consecutivoDocumento = await _consecutivosDocumentosRepositorio.GetAll().Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina).FirstOrDefaultAsync();
            if(_consecutivoDocumento == null)
            {
                return 0;
            }
            else
            {
                _consecutivoDocumento.ConsDocumento = _consecutivoDocumento.ConsDocumento + 1;
                _consecutivosDocumentosRepositorio.Update(_consecutivoDocumento);
                return _consecutivoDocumento.ConsDocumento;
            }
        }
    }
}
