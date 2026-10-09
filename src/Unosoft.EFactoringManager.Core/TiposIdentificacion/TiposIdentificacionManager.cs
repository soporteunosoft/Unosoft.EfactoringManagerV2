using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.TiposNotasRadian
{
    public class TiposIdentificacionManager : IDomainService
    {
        #region Atributos
        private readonly ITiposIdentificacionRepositorio _tiposIdentificacionRepoisitorio;
        #endregion

        public TiposIdentificacionManager(ITiposIdentificacionRepositorio tiposIdentificacionRepoisitorio)
        {
            _tiposIdentificacionRepoisitorio = tiposIdentificacionRepoisitorio;
        }

        //    public Identity.TiposOperaciones ObtenerTipoOperacion(int tipoOperacion)
        //    {
        //        return _tiposOperacionesRepositorio.GetAll().Where(x => x.TipoOperacion == tipoOperacion).FirstOrDefault();
        //    }
    }
}
