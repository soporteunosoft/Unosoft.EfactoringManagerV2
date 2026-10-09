using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposFirmas
{


    public class TiposFirmasManager : IDomainService
    {
        #region Atributos
        private readonly ITiposFirmasRepositorio _tiposFirmasRepositorio;
        #endregion

        public TiposFirmasManager(ITiposFirmasRepositorio tiposFirmasRepositorio)
        {
            _tiposFirmasRepositorio = tiposFirmasRepositorio;
        }

        //    public List<Identity.TiposTitulo> ObtenerTiposTitulo()
        //    {
        //        return _tiposTitulosRepositorio.GetAll().ToList();
        //    }
    }






}
