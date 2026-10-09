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
using System.Data;
using System.IO;
using System.Diagnostics;

namespace Unosoft.EFactoringManager.OrdenantesTerceros
{
    //     class OrdenantesTercerosManager : IDomainService
    //    {
    //        private readonly IOrdenantesTercerosRepositorio _ordenantesTercerosRepositorio;

    //        public OrdenantesTercerosManager(IOrdenantesTercerosRepositorio ordenantesTercerosRepositorio)
    //        {
    //            _ordenantesTercerosRepositorio = ordenantesTercerosRepositorio;
    //        }

    //        public List<Identity.OrdenantesTerceros> ObtenerOrdenantesTerceros(string idTercero)
    //        {
    //            var ordenantes = _ordenantesTercerosRepositorio.GetAll().Where(a => a.IdTercero == idTercero /*&& a.firma == true*/).ToList();

    //            return ordenantes;
    //        }

    //        public Identity.OrdenantesTerceros ObtenerOrdenantePorId(string idOrdenante)
    //        {
    //            return _ordenantesTercerosRepositorio.GetAll().Where(a => a.IdOrdenanteTercero == idOrdenante).Include(x=>x.Ciudad).FirstOrDefault();
    //        }

    //        public Identity.OrdenantesTerceros ObtenerRepresentantePorTercero(string idTercero)
    //        {
    //            return _ordenantesTercerosRepositorio.GetAll().Where(a => a.IdTercero == idTercero && a.FirmaOrdenanteCliente /*&& a.TipoOrdenante==9*/).FirstOrDefault();
    //        }

    //        public bool ActualizarOrdenantePorId(Identity.OrdenantesTerceros ordenante)
    //        {
    //            var _ordenante= _ordenantesTercerosRepositorio.GetAll().Where(a => a.IdTercero == ordenante.IdTercero && a.IdOrdenanteTercero == ordenante.IdOrdenanteTercero).FirstOrDefault();

    //            if (_ordenante == null)
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                _ordenante.Certificado = ordenante.Certificado;
    //                _ordenante.FechaCert = ordenante.FechaCert;
    //                _ordenante.VigenciaCert = ordenante.VigenciaCert;
    //                _ordenante.FecVigenciaCert = ordenante.FecVigenciaCert;
    //                _ordenante.NumSolicitudCert = ordenante.NumSolicitudCert;
    //                _ordenante.FecModifica = DateTime.Now;
    //                _ordenante.IdUsrMod = ordenante.IdUsrMod;
    //                return true;
    //            }
    //        }

    //    }
}
