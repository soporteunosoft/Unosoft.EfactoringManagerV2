using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Unosoft.EFactoringManager.Pronus.Dto;

namespace Unosoft.EFactoringManager.Pronus
{
    [AbpAuthorize]
    public class PronusServices : ApplicationService, IPronusServices
    {
        #region Atributos
        PronusManager _pronusManager;
        #endregion

        public PronusServices(PronusManager pronusManager)
        {
            _pronusManager = pronusManager;
        }

        [HttpPost]
        public List<Identity.ConsolidadoOperaciones> ObtConsolidadoOperacionesPeriodo(ResumenVigentesDto ResumenVigentesDto)
        {
            return _pronusManager.ObtConsolidadoOperacionesPeriodo(ResumenVigentesDto.IdOficina,
            ResumenVigentesDto.IdCliente, ResumenVigentesDto.IdInversionista, ResumenVigentesDto.IdPagador, ResumenVigentesDto.IdComisionista, ResumenVigentesDto.FecIniPeriodo,
            ResumenVigentesDto.FecFinPeriodo, 1);
        }

        public List<Identity.HistorialCuposPagadores> ObtHistorialCuposPagadores(ResumenVigentesDto ResumenVigentesDto)
        {
            return _pronusManager.ObtHistorialCuposPagadores(ResumenVigentesDto.IdPagador, ResumenVigentesDto.FecIniPeriodo,
            ResumenVigentesDto.FecFinPeriodo, 2);
        }

        public List<Identity.ResumenRecaudos> ObtResumenRecaudosClientes(ResumenVigentesDto ResumenVigentesDto)
        {
            return _pronusManager.ObtResumenRecaudosClientes(ResumenVigentesDto.IdOficina,
            ResumenVigentesDto.IdCliente, ResumenVigentesDto.IdInversionista, ResumenVigentesDto.IdPagador, ResumenVigentesDto.IdComisionista, ResumenVigentesDto.FecIniPeriodo,
            ResumenVigentesDto.FecFinPeriodo, 3);
        }

        public List<Identity.DetalleRecaudos> ObtDetalleRecaudosClientes(ResumenVigentesDto ResumenVigentesDto)
        {
            return _pronusManager.ObtDetalleRecaudosClientes(ResumenVigentesDto.IdOficina,
            ResumenVigentesDto.IdCliente, ResumenVigentesDto.IdInversionista, ResumenVigentesDto.IdPagador, ResumenVigentesDto.IdComisionista, ResumenVigentesDto.FecIniPeriodo,
            ResumenVigentesDto.FecFinPeriodo, 4);
        }
    }
}