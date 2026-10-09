using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unosoft.EFactoringManager.Billy.DTO;
using Unosoft.EFactoringManager.BillyServices;
using Unosoft.EFactoringManager.BillyServices.Configuration;
using Unosoft.EFactoringManager.BillyServices.DTO;
using Unosoft.EFactoringManager.Identity;
using Unosoft.EFactoringManager.IRepositorios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Unosoft.EFactoringManager.Billy
{
    public class UploadInvoiceBillyJob : BackgroundJob<BillyJobArgs>, ITransientDependency
    {
        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly SolicitudesOperaciones.SolicitudesOperacionesManager _solicitudesOperaciones;
        private readonly Titulos.TitulosManager _titulosManager;
        private readonly BillyConfiguration _billyConfiguration;
        private readonly BillyProcessManager _billyProcessManager;
        public UploadInvoiceBillyJob(ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio, Titulos.TitulosManager titulosManager,
                    SolicitudesOperaciones.SolicitudesOperacionesManager solicitudesOperaciones, BillyProcessManager billyProcessManager,
                    IOptions<BillyConfiguration> _settings
            )
        {
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;
            _titulosManager = titulosManager;
            _solicitudesOperaciones = solicitudesOperaciones;
            _billyProcessManager = billyProcessManager;
            _billyConfiguration = _settings.Value;

        }

        [UnitOfWork]
        public override void Execute(BillyJobArgs args)
        {
            var solicitudOperacion = _solicitudesOperacionesRepositorio.GetAll()
                            .Include(s => s.DetalleSolicitudes)
                            .ThenInclude(d => d.Titulo)
                            .Include(s => s.DetalleSolicitudes)
                            .ThenInclude(d => d.Titulo.EstadosTitulos)
                            .Where(a => a.TipoDocumento == args.TipoDocumento && a.IdOficina == args.IdOficina && a.IdSolicitud == args.IdSolicitud)
                            .FirstOrDefault();

            if (solicitudOperacion == null)
                return;

            var proxy = new BillyProxy(_billyConfiguration.UrlBase, _billyConfiguration.Token, _billyConfiguration.Market);

            foreach (var titulo in solicitudOperacion.DetalleSolicitudes.Select(x => x.Titulo).ToList())
            {
                try
                {
                    var cufe = titulo.CufeTitulo;

                    if (titulo.EstadosTitulos.Id != (int)EFactoringManagerConsts.EstadosTitulos.gEndosada)
                    {
                        var cargaResponde = proxy.CargarFacturaCufe(cufe).GetAwaiter().GetResult();

                        var statusList = new List<HttpStatusCode>() {
                            HttpStatusCode.OK,
                            HttpStatusCode.NoContent,
                            HttpStatusCode.Conflict
                        };

                        if (statusList.Any(x => x == cargaResponde.Status))
                        {
                            _titulosManager.ActualizarEstadoTitulo(titulo, (int)EFactoringManagerConsts.EstadosTitulos.gValidando, null, solicitudOperacion).GetAwaiter().GetResult();  //Validando o Cargando la factura en el sistema de Billy
                            var proceso = _billyProcessManager.Get(args.TipoDocumento, args.IdOficina, args.IdSolicitud, titulo.CudeTitulo).GetAwaiter().GetResult();

                            if (proceso == null)
                            {
                                proceso = new ProcesoEndosoBilly()
                                {
                                    Cufe = titulo.CufeTitulo,
                                    FechaProceso = DateTime.Now,
                                    IdOficina = args.IdOficina,
                                    IdSolicitud = args.IdSolicitud,
                                    TipoDocumento = args.TipoDocumento,
                                    Estado = (int)EFactoringManagerConsts.EstadosTitulos.gValidando,
                                    JsoResponse = JsonConvert.SerializeObject(cargaResponde.Response)

                                };
                                _billyProcessManager.Insertar(proceso).GetAwaiter().GetResult();
                            }
                            else
                            {
                                proceso.Estado = (int)EFactoringManagerConsts.EstadosTitulos.gValidando;
                                proceso.FechaProceso = DateTime.Now;
                                proceso.JsoResponse = JsonConvert.SerializeObject(cargaResponde.Response);
                                _billyProcessManager.Actualizar(proceso).GetAwaiter().GetResult();
                            }
                        }
                        else
                        {
                            titulo.MensajeProcesamiento = cargaResponde.Response.Errors.FirstOrDefault().Title + " " + cargaResponde.Response.Errors.FirstOrDefault().Detail;
                            titulo.CodigoError = cargaResponde.Status.ToString();
                            titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gError;
                            _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();
                            if (cargaResponde.Status == HttpStatusCode.InternalServerError)
                            {
                                _solicitudesOperaciones.ActualizarEstadoSolicitud(solicitudOperacion, (int)EFactoringManagerConsts.EstadosSolicitudes.gInicial, null).GetAwaiter().GetResult();
                            }
                            else _solicitudesOperaciones.ActualizarEstadoSolicitud(solicitudOperacion, (int)EFactoringManagerConsts.EstadosSolicitudes.gErrcufes, null).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
