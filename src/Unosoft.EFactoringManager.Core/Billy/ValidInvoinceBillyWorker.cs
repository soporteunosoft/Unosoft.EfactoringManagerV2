using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unosoft.EFactoringManager.BillyServices;
using Unosoft.EFactoringManager.BillyServices.Configuration;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.TiposEventosRadian;

namespace Unosoft.EFactoringManager.Billy
{
    public class ValidInvoinceBillyWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly BillyConfiguration _billyConfiguration;
        private readonly BillyProcessManager _billyProcessManager;
        private readonly TiposEventosRadianManager _tiposEventosRadian;
        private readonly Titulos.TitulosManager _titulosManager;
        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        const string SCHEDULE_VALID_ENDORSEMENT = "Fecha_Ejecucion_Validacion_Endoso_Billy";
        public ValidInvoinceBillyWorker(AbpTimer timer,
                                        IOptions<BillyConfiguration> billyConfiguration,
                                        BillyProcessManager billyProcessManager,
                                        TiposEventosRadianManager tiposEventosRadian,
                                         Titulos.TitulosManager titulosManager,
                                        ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio
                                        ) : base(timer)
        {
            _billyConfiguration = billyConfiguration.Value;
            Timer.Period = 1000 * 60 * (_billyConfiguration.TimerPeriod > 0 ? _billyConfiguration.TimerPeriod : 1);
            _billyProcessManager = billyProcessManager;
            _tiposEventosRadian = tiposEventosRadian;
            _titulosManager = titulosManager;
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;
        }

        [UnitOfWork]

        protected override void DoWork()
        {
            Logger.Info("Procesando Invoice BILLY");


            if (_billyConfiguration.Token != null)
            {
                try
                {
                    ValidateInvoiceEndorsement();
                }
                catch (Exception ex)
                {

                    Logger.Error($"Error ValidateInvoiceEndorsement : {ex.Message}");
                }

                try
                {

                    SearchBillByBilly();
                }
                catch (Exception ex)
                {

                    Logger.Error($"Error SearchBillByBilly : {ex.Message}");
                }

            }


        }

        #region Metodos Privados
        private void SearchBillByBilly()
        {
            var lstBill = _billyProcessManager.GetByEstado((int)EFactoringManagerConsts.EstadosTitulos.gValidando, DateTime.Now.AddMinutes(_billyConfiguration.TimeSearchBill));

            Logger.Info($"Procesando SearchBillByBilly : {lstBill.Count}");

            if (lstBill.Count <= 0)
                return;

            var proxy = new BillyProxy(_billyConfiguration.UrlBase, _billyConfiguration.Token, _billyConfiguration.Market);

            var eventosRadian = _tiposEventosRadian.ObtenerTiposEventosRadian().GetAwaiter().GetResult();

            int? tipoEndoso = 0;
            string[] formatos = { "dd/MM/yyyy", "MM/dd/yyyy" };

            foreach (var obj in lstBill.OrderBy(x => x.IdSolicitud))
            {
                try
                {
                    var billyprocess = _billyProcessManager.Get(obj.TipoDocumento, obj.IdOficina, obj.IdSolicitud, obj.Cufe).GetAwaiter().GetResult();

                    /* if (solicitud != billyprocess.IdSolicitud)
                     {
                         var sol = _solicitudesOperacionesRepositorio.GetAll()
                                                   .Include(s => s.DetalleSolicitudes)
                                                   .ThenInclude(d => d.Titulo)
                                                   .Where(a => a.TipoDocumento == billyprocess.TipoDocumento && a.IdOficina == billyprocess.IdOficina && a.IdSolicitud == billyprocess.IdSolicitud)
                                                   .FirstOrDefault();

                         detalleSolicitudes = sol.DetalleSolicitudes.ToList();
                         solicitud = billyprocess.IdSolicitud;
                         tipoEndoso = sol.TipoEndoso;
                     }*/
                    var billyFound = proxy.EncontrarFacturaCufe(billyprocess.Cufe).GetAwaiter().GetResult();
                    billyprocess.JsoResponse = JsonConvert.SerializeObject(billyFound);
                    var EstadoFactura = 0;
                    var MensajePr = "";
                    var CodigoErr = "";
                    if (billyFound.Status == System.Net.HttpStatusCode.OK)
                    {
                        Logger.Info($"Solicitud: {billyprocess.IdSolicitud} Factura {billyprocess.Cufe} : {System.Net.HttpStatusCode.OK}");
                        //string fixedJson = JsonFixer.FixInvoiceJson(jsonString);

                        //var invoice = JsonConvert.DeserializeObject<BillyServices.DTO.Attributes > (fixedJson);

                        var invoice = billyFound.Response.Data.Attributes;
                        Logger.Info($"Detalle Factura: {JsonConvert.SerializeObject(invoice)}");
                        Logger.Info($"Fecha Expiración: {invoice.Expiration}");


                        DateTime.TryParseExact(invoice.Expiration, formatos, CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None, out DateTime fechaExpira);

                        //var fechaExpira = DateTime.ParseExact(DateTime.ParseExact(invoice.Expiration, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (eventosRadian.Any(e => e.Rechazo && invoice.Events.Select(x => x.Code).ToList().Contains(e.TipoEvento)))
                        {
                            EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable;
                            CodigoErr = "Evento de rechazo";
                            MensajePr = "La factura posee algún evento que genera rechazo ante la Dian";
                        }
                        // comparamos eventos obligatorios con eventos de la factura
                        //Revisar la idea es validar que la factura tenga los enventos obligatorios
                        else if (eventosRadian.Count(e => e.Obligatorio) > eventosRadian.Count(e => e.Obligatorio && invoice.Events.Select(x => x.Code).ToList().Contains(e.TipoEvento)))
                        {
                            EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable;
                            CodigoErr = "No Posee " + EFactoringManagerConsts.CodigosEventos.Acuse + " o " + EFactoringManagerConsts.CodigosEventos.Recibo;
                            MensajePr = "La factura no posee el evento " + EFactoringManagerConsts.CodigosEventos.Acuse + " Acuse de recibo de la Factura Electrónica de Venta o " + EFactoringManagerConsts.CodigosEventos.Recibo + " Recibo del bien o prestación del servicio";
                        }
                        else if (fechaExpira < DateTime.Now && tipoEndoso != (EFactoringManagerConsts.TipoEndoso.CesionOrdinaria))
                        {
                            EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable;
                            CodigoErr = "Factura Vencida";
                            MensajePr = "La Factura esta vencida y por ende solo se puede hacer ENDOSO EN CESIÓN ORDINARIA";
                        }
                        else
                        {
                            EstadoFactura = (int)EFactoringManagerConsts.EstadosTitulos.gEndosable;
                        }

                        var titulo = _titulosManager.ObtenerTituloPorCufe(billyprocess.Cufe).GetAwaiter().GetResult();
                        //.Where(x => x.Titulo.CufeTitulo == billyprocess.Cufe)
                        //.Select(x => x.Titulo).FirstOrDefault();
                        billyprocess.Estado = EstadoFactura;
                        if (titulo != null)
                        {
                            Logger.Info($"titulo: {titulo.IdTitulo}  Estado Factura: {EstadoFactura} ");
                            ActualizarTitulo(titulo, EstadoFactura, MensajePr, CodigoErr, invoice);
                        }
                        else
                            Logger.Info($"titulo NULL");
                    }
                    billyprocess.FechaProceso = DateTime.Now;
                    _billyProcessManager.Actualizar(billyprocess).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error procesando Solicitud: {obj.IdSolicitud} Factura {obj.Cufe} : {ex.Message}");
                }
            }
        }

        private void ValidateInvoiceEndorsement()
        {
            var lstBill = _billyProcessManager.GetByEstado((int)EFactoringManagerConsts.EstadosTitulos.gEnProceso, DateTime.Now.AddMinutes(_billyConfiguration.TimeValidateEndoso));

            if (lstBill.Count() <= 0)
                return;

            var proxy = new BillyProxy(_billyConfiguration.UrlBase, _billyConfiguration.Token, _billyConfiguration.Market);

            Logger.Info($"Ejecutando ValidateInvoiceEndorsement");
            foreach (var obj in lstBill.Where(x => x.FechaProceso >= DateTime.Now.AddDays(-60)).OrderBy(x => x.IdSolicitud))
            {
                var billyprocess = _billyProcessManager.Get(obj.TipoDocumento, obj.IdOficina, obj.IdSolicitud, obj.Cufe).GetAwaiter().GetResult();
                try
                {

                    Logger.Info($"process: {billyprocess.Id}  Solicitud : {billyprocess.IdSolicitud} ");
                    Logger.Info($"Cufe: {billyprocess.Cufe}");

                    /* if (solicitud != billyprocess.IdSolicitud)
                     {
                         var sol = _solicitudesOperacionesRepositorio.GetAll()
                                                   .Include(s => s.DetalleSolicitudes)
                                                   .ThenInclude(d => d.Titulo)
                                                   .Where(a => a.TipoDocumento == billyprocess.TipoDocumento &&
                                                          a.IdOficina == billyprocess.IdOficina && a.IdSolicitud == billyprocess.IdSolicitud)
                                                   .FirstOrDefault();

                         detalleSolicitudes = sol.DetalleSolicitudes.ToList();
                         solicitud = billyprocess.IdSolicitud;
                     }*/

                    var billyFound = proxy.EncontrarFacturaCufe(billyprocess.Cufe).GetAwaiter().GetResult();
                    Logger.Info($" Factura billy: {JsonConvert.SerializeObject(billyFound)}");
                    if (billyFound != null && billyFound.Status == System.Net.HttpStatusCode.OK)
                    {


                        var invoice = billyFound.Response.Data.Attributes;
                        Logger.Info($"Detalle Factura: {JsonConvert.SerializeObject(invoice)}");

                        var endosoEventList = new List<string>() {
                        EFactoringManagerConsts.CodigosEventos.EndosoResponsabilidad,
                        EFactoringManagerConsts.CodigosEventos.EndosoSinResponsabilidad,
                        EFactoringManagerConsts.CodigosEventos.EndosoOrdinario
                    };

                        if (invoice.HolderIdNumber != invoice.IssuerIdNumber &&
                            invoice.Events.Select(x => x.Code).ToList().Any(code => endosoEventList.Contains(code)))
                        {
                            /*var titulo = detalleSolicitudes
                                                .Where(x => x.Titulo.CufeTitulo == billyprocess.Cufe)
                                                .Select(x => x.Titulo).FirstOrDefault();*/
                            var titulo = _titulosManager.ObtenerTituloPorCufe(billyprocess.Cufe).GetAwaiter().GetResult();

                            if (titulo != null)
                            {
                                Logger.Info($"titulo: {titulo.IdTitulo}  Estado Factura: Endosada ");
                                titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gEndosada;
                                titulo.Endoso = true;
                                titulo.EndosadoA = invoice.HolderIdNumber != invoice.IssuerIdNumber ? invoice.HolderName : "";
                                titulo.FecModifica = DateTime.Now;
                                titulo.IdUsrMod = "SERVBILLY";
                                _titulosManager.UpdateTitulo(titulo).GetAwaiter().GetResult();
                                Logger.Info($"titulo: Actualizada ");


                                billyprocess.Estado = (int)EFactoringManagerConsts.EstadosTitulos.gEndosada;
                                billyprocess.FechaProceso = DateTime.Now;
                                _billyProcessManager.Actualizar(billyprocess).GetAwaiter().GetResult();
                            }
                            else
                                Logger.Info($"titulo NULL");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error procesando Solicitud Endoso: {billyprocess.IdSolicitud} Factura {billyprocess.Cufe} : {ex.Message}");
                }
            }
        }

        private bool CanValidateEndorsement()
        {

            DateTime getCurrentDate(int index)
            {
                var count = _billyConfiguration.ScheduleValidEndorsementList.Count;
                return DateTime.Parse($"{DateTime.Now.AddDays(index < count ? 0 : 1):dd/MM/yyyy} {_billyConfiguration.ScheduleValidEndorsementList[index < count ? index : 0]}");

            }

            var schedeluleExecution = SettingManager.GetSettingValueForApplication(SCHEDULE_VALID_ENDORSEMENT);

            if (string.IsNullOrEmpty(schedeluleExecution))
            {
                schedeluleExecution = $"{DateTime.Now:dd/MM/yyyy} {_billyConfiguration.ScheduleValidEndorsementList[0]}";
            }



            //Fecha proxima ejecución
            var scheduleDateTime = DateTime.Parse(schedeluleExecution);
            DateTime fechaActual = DateTime.Now;

            for (int i = 0; i < _billyConfiguration.ScheduleValidEndorsementList.Count; i++)
            {
                var schedule = _billyConfiguration.ScheduleValidEndorsementList[i];

                var initialDate = getCurrentDate(i);
                var finalDate = getCurrentDate(i + 1);


                if (fechaActual >= initialDate &&
                    fechaActual < finalDate &&
                    fechaActual >= scheduleDateTime)
                {

                    SettingManager.ChangeSettingForApplication(SCHEDULE_VALID_ENDORSEMENT, finalDate.ToString("dd/MM/yyyy HH:mm:ss"));

                    return true;

                }
            }

            return false;
        }

        private void ActualizarTitulo(Identity.Titulos titulo, int estado, string mensaje, string codigoError, BillyServices.DTO.Attributes invoice)
        {
            titulo.EstadoTitulo = estado;
            titulo.Vencida = titulo.FecFinal < DateTime.Now;
            titulo.AcuseRecibo = invoice.Events.Select(x => x.Code).Contains(EFactoringManagerConsts.CodigosEventos.Acuse);
            titulo.AceptacionTacita = invoice.Events.Select(x => x.Code).Contains(EFactoringManagerConsts.CodigosEventos.AceptacionTacita);
            titulo.AceptacionExpresa = invoice.Events.Select(x => x.Code).Contains(EFactoringManagerConsts.CodigosEventos.AceptacionExpresa);
            titulo.InscritaFEcomoTV = invoice.Events.Select(x => x.Code).Contains(EFactoringManagerConsts.CodigosEventos.Inscripcion);
            titulo.Endoso = invoice.HolderIdNumber != invoice.IssuerIdNumber;
            titulo.EndosadoA = titulo.Endoso ? invoice.HolderName : "";
            titulo.MensajeProcesamiento = mensaje;
            titulo.CodigoError = codigoError;
            titulo.FecModifica = DateTime.Now;
            titulo.IdUsrMod = "SERVBILLY";
            _titulosManager.UpdateTitulo(titulo).GetAwaiter().GetResult();
        }

        #endregion
    }
}
