
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

namespace Unosoft.EFactoringManager.EstadosSolicitudes
{
    public class EstadosSolicitudesManager : IDomainService
    {
        #region Atributos
        private readonly IEstadosSolicitudesRepositorio _estadosSolicitudesRepositorio;
        #endregion

        #region Constructor de la clase
        public EstadosSolicitudesManager(
                IEstadosSolicitudesRepositorio estadosSolicitudesRepositorio

            )
        {
            _estadosSolicitudesRepositorio = estadosSolicitudesRepositorio;
       
            //_solicitudesOperacionesManager = solicitudesOperacionesManager;

        }
        #endregion


        public async Task<Identity.EstadosSolicitudes> ObtenerEstadosSolicitudes(string tipoDocumento, string nomEstadoSol)
        {
            return await _estadosSolicitudesRepositorio.GetAll().Where(a => a.TipoDocumento == tipoDocumento && a.NomEstadoSol == nomEstadoSol).FirstOrDefaultAsync();
        

            //return _detalleDocumentosRepositorio.GetAll()
            //   .Where(a => a.IdTercero == idTercero && a.Documentos.IdDocLinea == idDocLinea)
            //   .OrderByDescending(x => x.IdDetDoc).ToList();
        }

        //public Identity.Ciudades ObntenerCiudadPorId(int idCiudad)
        //{
        //    return _ciudadesRepositorio.GetAll().Where(a => a.IdCiudad == idCiudad).FirstOrDefault();
        //}
    }
}


