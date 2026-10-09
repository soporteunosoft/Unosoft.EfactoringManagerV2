using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.InformesNegociacionRadian
{
    public class InformesNegociacionRadianManager : IDomainService
    {

        #region Atributos
        private readonly IInformesNegociacionRadianRepositorio _InformesNegociacionRadianRepositorio;
        #endregion

        public InformesNegociacionRadianManager(IInformesNegociacionRadianRepositorio informesNegociacionRadianManagerRepositorio)
        {
            _InformesNegociacionRadianRepositorio = informesNegociacionRadianManagerRepositorio;
        }



    }
}
