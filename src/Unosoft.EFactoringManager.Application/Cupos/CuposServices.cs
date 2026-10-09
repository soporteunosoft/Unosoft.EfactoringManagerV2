using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Dto;
using System.Globalization;
using Abp.Authorization;

namespace Unosoft.EFactoringManager.Cupos
{
    public class CuposServices : ApplicationService,ICuposServices
    {
        #region Atributos
        CuposManager _cuposManager;
        #endregion

        public CuposServices(CuposManager cuposManager)
        {
            _cuposManager = cuposManager;
        }

        //public List<Cupos.Dto.CuposDto> ObtenerCuposPorTercero(Terceros.Dto.TerceroDto tercero)
        //{
        //    //FOR DO:AJUSTAR CON ARLEY
        //    try
        //    {
        //        var cupos = _cuposManager.ObtenerCuposPorTercero(tercero.IdTercero);
        //        return ObjectMapper.Map< List<Cupos.Dto.CuposDto>> (cupos);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        public List<Cupos.Dto.CuposDto> ObtenerCuposPorTercero(Terceros.Dto.TerceroDto tercero)
        {
            //FOR DO:AJUSTAR CON ARLEY
            try
            {
                var cupos = _cuposManager.ObtenerCuposPorTercero(tercero.IdTerceroPadre);
                
                if (cupos == null)
                    return null;
                
                return ObjectMapper.Map<List<Cupos.Dto.CuposDto>>(cupos);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
