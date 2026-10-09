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

namespace Unosoft.EFactoringManager.TiposOperacionesRadian
{
    public class TiposOperacionesRadianManager : IDomainService
    {

        #region Atributos
        private readonly ITiposOperacionesRadianRepositorio _TiposOperacionesRadianRepositorio;
        #endregion

        public TiposOperacionesRadianManager(ITiposOperacionesRadianRepositorio tiposOperacionesRadianRepositorio)
        {
            _TiposOperacionesRadianRepositorio = tiposOperacionesRadianRepositorio;
        }


        public Identity.TiposOperacionesRadian ObtenerTiposOperacionesRadianPorTipoEvento(string TipoEvento)
        {

            return _TiposOperacionesRadianRepositorio.GetAll().Where(a => a.TipoEvento == TipoEvento).FirstOrDefault();

        }

    }

}
