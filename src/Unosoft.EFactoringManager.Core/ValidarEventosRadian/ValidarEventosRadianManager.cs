using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Unosoft.EFactoringManager.IRepositorios;


namespace Unosoft.EFactoringManager.ValidarEventosRadian
{
    public class ValidarEventosRadianManager : IDomainService
    {

        #region Atributos
        private readonly IValidarEventosRadianRepositorio _ValidarEventosRadianRepositorio;
        #endregion

        public ValidarEventosRadianManager(IValidarEventosRadianRepositorio validarEventosRadianRepositorioRepositorio)
        {
            _ValidarEventosRadianRepositorio = validarEventosRadianRepositorioRepositorio;
        }


        public async Task<List<Identity.ValidarEventosRadian>> ObtenerCamposObligatorios(string TipoEvento)
        {
            return await _ValidarEventosRadianRepositorio.GetAll().Where(a => a.TipoEvento == TipoEvento).ToListAsync();
        }
       
    }


}