using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.ValidarOperaciones
{
    public class ValidarOperacionesManager : IDomainService
    {

        #region Atributos
        private readonly IValidarOperacionRepositorio _validarOperacionRepositorio;
        #endregion


        public ValidarOperacionesManager(IValidarOperacionRepositorio validarOperacionRepositorio)
        {
            _validarOperacionRepositorio = validarOperacionRepositorio;

        }

        public List<Identity.ValidarOperacion> Validar(string idTerceroEmisor, string idTerceroPagador, decimal montoOperacion, int tipoOperacion)
        {
            return _validarOperacionRepositorio.ValidarOperacion(idTerceroEmisor, idTerceroPagador, montoOperacion, tipoOperacion);
        }
    }
}
