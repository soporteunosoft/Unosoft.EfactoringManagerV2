using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.FirmaDigital;
using Unosoft.EFactoringManager.FirmaElectronica.Dto;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.FirmaElectronica
{
    public class FirmaElectronicaAppService : ApplicationService, IFirmaElectronicaAppService
    {
        private readonly FirmaElectronicaManager _firmaElectronicaManager;
        private readonly FirmaDigitalManager _firmaDigitalManager;
        public FirmaElectronicaAppService(FirmaElectronicaManager firmaElectronicaManager, FirmaDigitalManager firmaDigitalManager)
        {
            _firmaElectronicaManager = firmaElectronicaManager;
            _firmaDigitalManager = firmaDigitalManager;
        }

        public async Task<GetSingleRespuesta> CrearProcesoFirma(CrearProcesoDto request)
        {
            try
            {
                StringBuilder result;

                if (request.TipoFirma == (int)TipoFirma.Digital)
                    result = await _firmaDigitalManager.CrearProceso(request.Documentos, request.Terceros, AbpSession.UserId.Value);
                else
                    result = await _firmaElectronicaManager.CrearProceso(request.Documentos, request.Terceros, AbpSession.UserId.Value);

                return new GetSingleRespuesta(
                                                DateTime.Now,
                                                result.Length > 0 ? -1 : 0,
                                                result.ToString()
                                             );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task NotificarProcesoFirma(NotificacionDto notificacion)
        {
            await _firmaElectronicaManager.Notificar(notificacion.Id, notificacion.Estado, notificacion.Url);
        }

        [HttpPut()]
        public async Task<GetSingleRespuesta> AnularProcesoFirma(int detalleDocumentoId)
        {
            try
            {
                await _firmaElectronicaManager.Anular(detalleDocumentoId);
                return new GetSingleRespuesta(
                                                  DateTime.Now,
                                                   0,
                                                  "La anulación del proceso se realizó de manera exitosa"
                                               );
            }
            catch (Exception)
            {
                return new GetSingleRespuesta(
                                              DateTime.Now,
                                              -1,
                                              "La anulación del proceso resentó un error"
                                           );
            }
        }

        [HttpPost()]
        public async Task<GetSingleRespuesta> ConsultarProcesoFirma(int detalleDocumentoId)
        {
            try
            {
                await _firmaElectronicaManager.Consultar(detalleDocumentoId);
                return new GetSingleRespuesta(
                                                  DateTime.Now,
                                                   0,
                                                  "La consulta del proceso se realizó de manera exitosa"
                                               );
            }
            catch (Exception)
            {
                return new GetSingleRespuesta(
                                              DateTime.Now,
                                              -1,
                                              "La consulta del proceso presentó un error"
                                           );
            }
        }
    }
}
