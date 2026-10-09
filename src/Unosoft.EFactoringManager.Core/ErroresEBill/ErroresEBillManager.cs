using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.ErroresEBill
{
    public class ErroresEBillManager : IDomainService
    {
        #region Atributos
        private readonly IErroresEbillRepositorio _erroresEBillRepositorio;
        #endregion

        #region Constructor de la clase
        public ErroresEBillManager(
            IErroresEbillRepositorio erroresEBillRepositorio)
        {
            _erroresEBillRepositorio = erroresEBillRepositorio;
        }
        #endregion


        public async Task<List<Identity.ErroresEbill>> getListaErrores(string codError)
        {
            var codErrores = codError.Split(',').ToList();
            return await _erroresEBillRepositorio.GetAll().Where(x => codErrores.Contains(x.CodigoError)).ToListAsync();
        }
    }
}
