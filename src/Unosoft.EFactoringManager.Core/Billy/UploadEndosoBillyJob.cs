using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Billy.DTO;
using Unosoft.EFactoringManager.BillyServices;
using Unosoft.EFactoringManager.BillyServices.Configuration;
using Unosoft.EFactoringManager.BillyServices.DTO;
using Unosoft.EFactoringManager.Identity;
using Unosoft.EFactoringManager.IRepositorios;

namespace Unosoft.EFactoringManager.Billy
{
    public class UploadEndosoBillyJob : BackgroundJob<EndosoBillyJobArgs>, ITransientDependency
    {

        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly SolicitudesOperaciones.SolicitudesOperacionesManager _solicitudesOperaciones;
        private readonly Titulos.TitulosManager _titulosManager;
        private readonly BillyConfiguration _billyConfiguration;
        private readonly BillyProcessManager _billyProcessManager;

        public UploadEndosoBillyJob(ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio,
                            SolicitudesOperaciones.SolicitudesOperacionesManager solicitudesOperaciones,
                            Titulos.TitulosManager titulosManager, BillyProcessManager billyProcessManager,
                            IWebHostEnvironment hostingEnvironment, IOptions<BillyConfiguration> _settings
            )
        {
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;
            _solicitudesOperaciones = solicitudesOperaciones;
            _titulosManager = titulosManager;
            _billyProcessManager = billyProcessManager;
            _billyConfiguration = _settings.Value;

        }

        [UnitOfWork]
        public override void Execute(EndosoBillyJobArgs args)
        {
            var solicitudOperacion = _solicitudesOperacionesRepositorio.GetAll()
                           .Where(a => a.TipoDocumento == args.TipoDocumento
                           && a.IdOficina == args.IdOficina
                           && a.IdSolicitud == args.IdSolicitud)
                           .FirstOrDefault();

            if (solicitudOperacion == null)
                return;

            var proxy = new BillyProxy(_billyConfiguration.UrlBase, _billyConfiguration.Token, _billyConfiguration.Market);
            var tasa = solicitudOperacion.TipoTasaInteres == 1 ? solicitudOperacion.TasaInteresCliente / 10 : solicitudOperacion.TasaInteresCliente / 100;

            foreach (var cufe in args.Titulos)
            {
                Logger.Info("En proceso Endoso : " + cufe);
                try
                {
                    var titulo = _titulosManager.ObtenerTituloPorCufe(cufe).GetAwaiter().GetResult();
                    var billyprocess = _billyProcessManager.Get(args.TipoDocumento, args.IdOficina, args.IdSolicitud, cufe).GetAwaiter().GetResult();
                    if (billyprocess == null ||
                        billyprocess.Estado == (int)EFactoringManagerConsts.EstadosTitulos.gEnProceso)
                        continue;
                    try
                    {


                        var response = proxy.EndosoFacturaCufe(cufe, (double)tasa).GetAwaiter().GetResult();

                        Logger.Info("Response Endoso : " + cufe + ";Json :" + JsonConvert.SerializeObject(response));
                        if (response.Status == System.Net.HttpStatusCode.NoContent ||
                            (response.Status == System.Net.HttpStatusCode.BadRequest && GetError(response).Contains("invoice holder cannot")) ||
                            (response.Status == System.Net.HttpStatusCode.BadRequest && GetError(response).Contains("Invoice is not available for purchase"))
                            )
                        {
                            ActualizarProcesoBilly(billyprocess);
                        }
                        else
                        {
                            ActualizarTituloConError(solicitudOperacion, titulo, response);
                        }
                    }
                    catch (Exception ex)
                    {

                        ActualizarTituloConError(solicitudOperacion, titulo, new BillyResponse<CargaRespondeDTO>()
                        {
                            Response = new CargaRespondeDTO()
                            {
                                Errors = new List<Error>() { new Error() { Detail = ex.Message } }
                            },
                            Status = System.Net.HttpStatusCode.InternalServerError
                        });
                    }
                }
                catch (Exception ex)
                {

                    Logger.Error("Error proceso Endoso : " + cufe + ";Error :" + ex.Message);
                }
            }
        }

        private void ActualizarProcesoBilly(ProcesoEndosoBilly billyprocess)
        {
            billyprocess.Estado = (int)EFactoringManagerConsts.EstadosTitulos.gEnProceso;
            billyprocess.FechaProceso = DateTime.Now;
            _billyProcessManager.Actualizar(billyprocess).GetAwaiter().GetResult();
        }

        private string GetError(BillyResponse<CargaRespondeDTO> response)
        {
            if (response.Response?.Errors != null && response.Response.Errors.Any())
            {
                var error = response.Response.Errors.First();
                return $"{error.Detail} {error.Title}";
            }
            return "";
        }

        private void ActualizarTituloConError(Identity.SolicitudesOperaciones solicitudOperacion,
                                            Identity.Titulos titulo,
                                            BillyResponse<CargaRespondeDTO> response)
        {
            if (response.Response?.Errors != null && response.Response.Errors.Any())
            {
                var error = response.Response.Errors.First();
                titulo.MensajeProcesamiento = $"{error.Detail} {error.Title}";
            }
            else
            {
                titulo.MensajeProcesamiento = "Error desconocido en el endoso.";
            }
            titulo.CodigoError = response.Status.ToString();
            titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gError;

            _solicitudesOperaciones.ActualizarEstadoSolicitud(solicitudOperacion, (int)EFactoringManagerConsts.EstadosSolicitudes.gErrcufes, null).GetAwaiter().GetResult();
            _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();

            var billyprocess = _billyProcessManager.Get(solicitudOperacion.TipoDocumento, solicitudOperacion.IdOficina, solicitudOperacion.IdSolicitud, titulo.CufeTitulo).GetAwaiter().GetResult();
            if (billyprocess == null) return;
            billyprocess.JsoResponse = titulo.MensajeProcesamiento;
            billyprocess.FechaProceso = DateTime.Now;
            billyprocess.Estado = (int)EFactoringManagerConsts.EstadosTitulos.gError;
            _billyProcessManager.Actualizar(billyprocess).GetAwaiter().GetResult();

        }
    }
}
