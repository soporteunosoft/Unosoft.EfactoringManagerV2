 using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.GestionesTerceros.Dto;

namespace Unosoft.EFactoringManager.GestionesTerceros
{

    public class GestionesTercerosServices : ApplicationService, IGestionesTercerosServices
    {
        #region Atributos
        GestionesTercerosManager _gestionesTercerosManager;
        #endregion

        public GestionesTercerosServices(GestionesTercerosManager gestionesTercerosManager)
        {
            _gestionesTercerosManager = gestionesTercerosManager;
        }

        [HttpGet]
        public async Task<List<GestionesTercerosDto>> ObtenerGestionesTerceros(int idGestion, int oficina, int? numTercero, string idTercero, int tipoTercero)
        {
            try
            {
                List<GestionesTercerosDto> gestionesTercerosDto = new List<GestionesTercerosDto>();

                var gestiones = await _gestionesTercerosManager.ObtenerGestionesTerceros(idGestion, oficina, numTercero, idTercero, tipoTercero);
                foreach (var gestion in gestiones)
                {
                    GestionesTercerosDto gestionesTerceroDto = new GestionesTercerosDto();
                    gestionesTerceroDto.Id = gestion.Id;
                    gestionesTerceroDto.IdOficina = gestion.IdOficina;
                    gestionesTerceroDto.NumeroTercero = gestion.NumeroTercero;
                    gestionesTerceroDto.FecGestion = gestion.FecGestion;
                    gestionesTerceroDto.Contacto = gestion.Contacto;
                    gestionesTerceroDto.TipoGestion = gestion.TipoGestion;
                    gestionesTerceroDto.DatoGestion = gestion.DatoGestion;
                    gestionesTerceroDto.Observacion = gestion.Observacion;
                    gestionesTerceroDto.Tercero = gestion.Tercero;
                    gestionesTercerosDto.Add(gestionesTerceroDto);
                }
                return gestionesTercerosDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GestionesTercerosDto> ObtenerGestionesTercerosPorId(GestionesTercerosDto gestionDto)
        {
            try
            {
                var gestion = await _gestionesTercerosManager.ObtenerGestionesTercerosPorId(gestionDto.Id, gestionDto.IdOficina);

                return ObjectMapper.Map<GestionesTercerosDto>(gestion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetSingleRespuestaExtend> Insertar(GestionesTercerosDto gestionesTerceros)
        {
            try
            {
                var result = await _gestionesTercerosManager.Insertar(new Identity.GestionesTerceros()
                {
                    IdOficina = gestionesTerceros.IdOficina,
                    FecGestion = gestionesTerceros.FecGestion,
                    NumeroTercero = gestionesTerceros.NumeroTercero,
                    Contacto = gestionesTerceros.Contacto,
                    TipoGestion = gestionesTerceros.TipoGestion,
                    DatoGestion = gestionesTerceros.DatoGestion,
                    Observacion = gestionesTerceros.Observacion,
                    Activa = true,
                    FecModifica = DateTime.Now
                }, AbpSession.UserId.Value);

                return new GetSingleRespuestaExtend(DateTime.Now, 0, "Gestion registrada", result);
            }
            catch (Exception)
            {
                return new GetSingleRespuestaExtend(DateTime.Now, -1, "Gestion no registrada", null);
            }
        }

        public async Task<bool> ActualizarGestiones(Identity.GestionesTerceros gestionesTerceros)
        {
            try
            {
                gestionesTerceros.FecModifica = DateTime.Now;

                return await _gestionesTercerosManager.ActualizarGestiones(gestionesTerceros, AbpSession.UserId.Value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EliminarGestiones(Identity.GestionesTerceros gestionesTerceros)
        {
            try
            {
                gestionesTerceros.Activa = false;
                gestionesTerceros.FecModifica = DateTime.Now;

                return await _gestionesTercerosManager.ActualizarGestiones(gestionesTerceros, AbpSession.UserId.Value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DocumentosGestionDto>> ObtenerDocumentosPorIdGestion(GestionesTercerosDto Gestion)
        {
            try
            {
                var documentos = await _gestionesTercerosManager.ObtDocumentosFromGestion(Gestion.Id, Gestion.IdOficina, Gestion.NumeroTercero);

                return ObjectMapper.Map<List<DocumentosGestionDto>>(documentos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EliminarDocumentoGestiones(DocumentosGestionDto documentosGestionDto)
        {
            try
            {
                return _gestionesTercerosManager.EliminarDocumento(documentosGestionDto.IdOficina, documentosGestionDto.IdGestion, documentosGestionDto.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
