using Abp.BackgroundJobs;
using Abp.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.EBillServices.Configuration;
using Unosoft.EFactoringManager.BackgroundJob;
using Unosoft.EFactoringManager.BackgroundJob.DTO;
using Unosoft.EFactoringManager.Billy;
using Unosoft.EFactoringManager.Billy.DTO;
using Unosoft.EFactoringManager.BillyServices.Configuration;
using Unosoft.EFactoringManager.Common.Extension;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.Dispapeles;
using Unosoft.EFactoringManager.EstadosSolicitudes;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Notificacion;
using UnoSoft.EFactoringManager.CrystalReportService;
using UnoSoft.EFactoringManager.CrystalReportService.Service;
using Abp.AspNetCore.SignalR.Notifications;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.SolicitudesOperaciones
{
    public class SolicitudesOperacionesManager : IDomainService
    {
        #region Atributos

        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly IOficinasRepositorio _oficinaRepositorio;
        //private readonly SignalRRealTimeNotifier _signalRRealTimeNotifier;

        DetalleSolicitudes.DetalleSolicitudesManager _detalleSolicitudesManager;
        Terceros.TercerosManager _tercerosManager;
        TiposOperaciones.TiposOperacionesManager _tiposOperacionesManager;
        TiposTasaInteres.TiposTasaInteresManager _tiposTasaInteresManager;
        ModalidadesOperacion.ModalidadesOperacionManager _modalidadesOperacionManager;
        ImpDocumentosOperativos.ImpDocumentosOperativosManager _impDocumentosOperativosManager;
        Titulos.TitulosManager _titulosManager;
        ConsecutivosDocumentos.ConsecutivosDocumentosManager _consecutivosDocumentosManager;
        private readonly EstadosSolicitudesManager _estadosSolicitudesManager;
        private readonly IReportApiProxy _reportApiProxy;

        private readonly DetalleDocumentos.DetalleDocumentosManager _detalleDocumentosManager;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly DispapelesManager _dispapelesManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly BillyConfiguration _billyConfiguration;
        private readonly EBillFtpConfiguration _eBillFtpConfiguration;

        private readonly LiquidacionContext _liquidacionContext;

        private readonly AppSettings _appSettings;
        private readonly NotificacionesManager _notificacionesManager;

        #endregion

        public SolicitudesOperacionesManager(ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio, Terceros.TercerosManager tercerosManager,
                                              TiposOperaciones.TiposOperacionesManager tiposOperacionesManager, TiposTasaInteres.TiposTasaInteresManager tiposTasaInteresManager,
                                              ModalidadesOperacion.ModalidadesOperacionManager modalidadesOperacionManager, ImpDocumentosOperativos.ImpDocumentosOperativosManager impDocumentosOperativosManager,
                                               DetalleSolicitudes.DetalleSolicitudesManager detalleSolicitudesManager, Titulos.TitulosManager titulosManager, ConsecutivosDocumentos.ConsecutivosDocumentosManager consecutivosDocumentosManager,
                                               DetalleDocumentos.DetalleDocumentosManager detalleDocumentosManager, IOficinasRepositorio oficinaRepositorio, IWebHostEnvironment hostingEnvironment, DispapelesManager dispapelesManager,
                                               NotificacionesManager notificacionesManager, EstadosSolicitudesManager estadosSolicitudesManager,
                                               IBackgroundJobManager backgroundJobManager, IOptions<BillyConfiguration> _settings, IOptions<EBillFtpConfiguration> _eBillsettings,
                                               IOptions<AppSettings> appSettings,
                                               IReportApiProxy reportApiProxy
            )
        {
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;
            _tercerosManager = tercerosManager;
            _tiposOperacionesManager = tiposOperacionesManager;
            _tiposTasaInteresManager = tiposTasaInteresManager;
            _modalidadesOperacionManager = modalidadesOperacionManager;
            _impDocumentosOperativosManager = impDocumentosOperativosManager;
            _detalleSolicitudesManager = detalleSolicitudesManager;
            _titulosManager = titulosManager;
            _consecutivosDocumentosManager = consecutivosDocumentosManager;
            _detalleDocumentosManager = detalleDocumentosManager;
            _oficinaRepositorio = oficinaRepositorio;
            _hostingEnvironment = hostingEnvironment;
            _dispapelesManager = dispapelesManager;
            _estadosSolicitudesManager = estadosSolicitudesManager;
            _backgroundJobManager = backgroundJobManager;
            _billyConfiguration = _settings.Value;
            _eBillFtpConfiguration = _eBillsettings.Value;
            _appSettings = appSettings.Value;
            _notificacionesManager = notificacionesManager;
            _reportApiProxy = reportApiProxy;

            _liquidacionContext = new LiquidacionContext();
        }

        //public bool IngresarSolicitudOperacion(IList<Identity.Titulos> titulos, Identity.TiposOperaciones tipoOperacion, Identity.ModalidadesOperacion modalidadOperacion)
        public async Task<Identity.SolicitudesOperaciones> IngresarSolicitudOperacion(SolicitudOperacionInput input)
        {

            Identity.SolicitudesOperaciones _solicitudOperacion = new Identity.SolicitudesOperaciones();
            Identity.Terceros tercero;

            tercero = await _tercerosManager.ObtenerTerceroPorId(input.NumeroTercero);


            //WIGELO:27/12/2022 Se comenta para nuevo flujo
            //_tipoOperacion = _tiposOperacionesManager.ObtenerTipoOperacion(tipoOperacion.TipoOperacion);
            //_modalidadOperacion = _modalidadesOperacionManager.ObtenerModalidadOpePorTipoOpe(_tipoOperacion.TipoOperacion);

            //WIGELO:27/12/2022 Se debe validar por opciones:EBILL ARCHIVOS PLANOS
            //idSolicitud = _consecutivosDocumentosManager.ActualizarConsecutivo(EFactoringManagerConsts.gSolicitud, tercero.IdOficina);
            var idSolicitud = await _consecutivosDocumentosManager.ActualizarConsecutivo(input.TipoDocumento, input.IdOficina);


            //WIGELO:27/12/2022 Se debe validar por opciones:EBILL ARCHIVOS PLANOS
            //_solicitudOperacion.TipoDocumento = EFactoringManagerConsts.gSolicitud;
            _solicitudOperacion.TipoDocumento = input.TipoDocumento;
            _solicitudOperacion.IdOficina = (byte)input.IdOficina;
            _solicitudOperacion.IdSolicitud = idSolicitud;

            _solicitudOperacion.IdConceptoSol = input.TipoDocumento;

            _solicitudOperacion.NumeroTercero = tercero.Id;
            _solicitudOperacion.TipoTercero = Convert.ToInt32(EFactoringManagerConsts.TipoTercero.gCliente);
            //_solicitudOperacion.IdConceptoSol = EFactoringManagerConsts.gSolicitud;
            _solicitudOperacion.FecDocumento = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            _solicitudOperacion.FecSolicitud = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            _solicitudOperacion.Producto = input.Producto;


            //WIGELO:27/12/2022 Se comenta para nuevo flujo
            //_solicitudOperacion.TipoOperacion = Convert.ToInt32(EFactoringManagerConsts.TipoOperacion.gFactoring);
            //_solicitudOperacion.ModalidadOperacion = Convert.ToInt32(EFactoringManagerConsts.ModalidadesOperacion.gFactoring_Electronico);


            //WIGELO:27/12/2022 Se debe ingresar de acuerdo al tipo de tasa seleccionado
            //_solicitudOperacion.TipoTasaInteres = Convert.ToByte(EFactoringManagerConsts.TiposTasaInteres.gEfectiva);
            //_solicitudOperacion.TipoTasaInteres = Convert.ToByte(EFactoringManagerConsts.TiposTasaInteres.gMensual);

            _solicitudOperacion.TipoTasaInteres = input.TipoTasaInteres;



            //_solicitudOperacion.ModalidadInteres = Convert.ToByte(EFactoringManagerConsts.ModalidadesInteres.gVencida);
            //_solicitudOperacion.TasaInteresCliente = tercero.TasaInteres;
            _solicitudOperacion.TasaInteresCliente = (float)input.TasaInteresCliente;
            _solicitudOperacion.PorcNegociacion = input.PorcNegociacion;
            _solicitudOperacion.TipoEndoso = input.TipoEndoso;


            var esAdmind = input.Roles != null && input.Roles.Any(r => r.Contains("admin", StringComparison.CurrentCultureIgnoreCase));

            //Cómo manejar los estados de acuerdo a tipo de la solicitud
            if (_appSettings.SolicitudOperacionesEstadoInicialEmisor && tercero.Cliente && !esAdmind)
            {
                _solicitudOperacion.EstadoSolicitud = (int)EFactoringManagerConsts.EstadosSolicitudes.gPendienteEnviar;
            }
            else
            {
                _solicitudOperacion.EstadoSolicitud = (int)EFactoringManagerConsts.EstadosSolicitudes.gInicial;
            }


            //rev
            _solicitudOperacion.RadicadoPor = tercero.IdTercero;
            _solicitudOperacion.IdUsrMod = tercero.IdTercero;
            _solicitudOperacion.FecModifica = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            _solicitudOperacion.HoraModifica = DateTime.Now.ToString("HH:mm:ss");

            if (_solicitudOperacion.TipoDocumento == EFactoringManagerConsts.gSolicitudSol)
            {
                _solicitudOperacion.FecOperacion = _solicitudOperacion.FecSolicitud;
                _solicitudOperacion.BaseDias = (int)EFactoringManagerConsts.BaseDias.gDias365;
            }


            await Insertar(_solicitudOperacion);

            _solicitudOperacion.Tercero = tercero;

            return _solicitudOperacion;


            /*


                    //IngresarDetalleSolicitud(idSolicitud, titulos, _tipoOperacion, _modalidadOperacion, cliente);


                    IngresarDetalleSolicitud(idSolicitud, titulos, _tipoOperacion, _modalidadOperacion, tercero);
                    if (ActualizarTotalSolicitudOperacion(_solicitudOperacion))
                    {
                        _titulosManager.ActualizarEstadoTitulos(titulos, (int)EFactoringManagerConsts.EstadosTitulos.gOfrecidas);
                        return true;
                    }
                    else { return false; }

                }
                else { return false; }
            }
            else { return false; }*/

        }

        public async Task IngresarDetalleSolicitud(int IdSolicitud, IList<Identity.Titulos> titulos, Identity.TiposOperaciones tipoOperacion, Identity.ModalidadesOperacion modalidadOperacion, Identity.Terceros tercero, SolicitudOperacionInput input)
        {

            Identity.TiposTasaInteres _tiposTasaInteres = await _tiposTasaInteresManager.ObtenerTiposTasaInteres(modalidadOperacion.TasaInteres.TipoTasaInteres);
            Identity.ModalidadesOperacion _modalidadesOperacion = await _modalidadesOperacionManager.ObtenerModalidadOpePorTipoOpe(tipoOperacion.TipoOperacion);

            await CargarImpuestos(EFactoringManagerConsts.gSolicitud, tercero.IdOficina, EFactoringManagerConsts.gFinanciera);
            decimal? vrNegociacion = 0;
            decimal? vrCobertura = 0;
            decimal? vrPresente = 0;
            decimal? vrInteres = 0;
            decimal? vrFuturo = 0;
            decimal? vrNetoRecibir = 0;
            decimal? vrDescuento = 0;
            decimal vrGao = 0;
            decimal ivaGao = 0;



            for (int i = 0; i < titulos.Count; i++)
            {
                Identity.Titulos _titulo = await _titulosManager.ObtenerTituloPorId(titulos[i].NumeroTercero, titulos[i].IdTitulo, titulos[i].TipoTitulo);
                Identity.DetalleSolicitudes _detalleSolicitudes = new Identity.DetalleSolicitudes();
                vrNegociacion = _titulo.VrNetoTitulo * (Convert.ToDecimal(tercero.PorcNegociacion) / 100);
                vrCobertura = _titulo.VrNetoTitulo - vrNegociacion;
                vrPresente = vrNegociacion;
                vrInteres = CalcularInteres(vrNegociacion, tercero.TasaInteres, _titulo.Dias, _modalidadesOperacion.TasaInteres.ModalidadInteres, modalidadOperacion.BaseDias, _tiposTasaInteres.Id, tipoOperacion.TipoOperacion, _modalidadesOperacion.ModalidadOperacion);
                if (modalidadOperacion.ModalidadOperacion == Convert.ToInt32(EFactoringManagerConsts.ModalidadesOperacion.gFactoring_Electronico))
                {
                    CalcularInteresesImpuestos(tercero, (decimal)vrInteres);
                }
                vrFuturo = vrNegociacion;
                vrDescuento = vrFuturo - vrInteres;
                vrGao = (decimal)vrDescuento * ((decimal)modalidadOperacion.PorcAdmon / 100);
                ivaGao = vrGao * ((decimal)_liquidacionContext.TasaGao / 100);
                vrNetoRecibir = vrFuturo - _liquidacionContext.ReteFuenteIntereses - _liquidacionContext.ReteIcaIntereses;
                _detalleSolicitudes.TipoDocumento = EFactoringManagerConsts.gSolicitud;
                _detalleSolicitudes.IdOficina = (byte)input.IdOficina;
                _detalleSolicitudes.IdSolicitud = IdSolicitud;
                _detalleSolicitudes.NumeroTercero = tercero.Id;
                _detalleSolicitudes.IdTitulo = _titulo.IdTitulo;
                _detalleSolicitudes.TipoTitulo = (int)EFactoringManagerConsts.TiposTitulos.gFacturaElectronica;
                _detalleSolicitudes.NumeroPagador = _titulo.Pagador.Id;
                _detalleSolicitudes.FecInicial = _titulo.FecInicial;
                _detalleSolicitudes.FecFinal = _titulo.FecProbable;
                _detalleSolicitudes.FecEmision = _titulo.FecEmision;
                _detalleSolicitudes.FecProbable = _titulo.FecProbable;
                _detalleSolicitudes.DiasOperacion = (short)_titulo.Dias;
                _detalleSolicitudes.VrPresente = (decimal)vrPresente;
                _detalleSolicitudes.VrFuturo = (decimal)vrFuturo;
                _detalleSolicitudes.VrIntereses = (decimal)vrInteres;
                _detalleSolicitudes.ReteFuenteIntereses = _liquidacionContext.ReteFuenteIntereses;
                _detalleSolicitudes.VrAdmon = vrGao;
                _detalleSolicitudes.IvaAdmon = ivaGao;
                _detalleSolicitudes.VrNetoRecibir = (decimal)vrFuturo;
                _detalleSolicitudes.BaseCapitalInteres = (decimal)vrFuturo;
                _detalleSolicitudes.PorcNegociacion = (float)tercero.PorcNegociacion;

                _detalleSolicitudes.VrBrutoTitulo = (decimal)_titulo.VrBrutoTitulo;
                _detalleSolicitudes.VrDescuentoTitulo = (decimal)_titulo.VrDescuentoTitulo;
                _detalleSolicitudes.VrAnticipoTitulo = (decimal)_titulo.VrAnticipoTitulo;
                _detalleSolicitudes.PorcIvaTitulo = (decimal)_titulo.PorcIvaTitulo;
                _detalleSolicitudes.VrIvaTitulo = (decimal)_titulo.VrIvaTitulo;
                _detalleSolicitudes.PorcReteIvaTitulo = (decimal)_titulo.PorcReteIvaTitulo;
                _detalleSolicitudes.VrReteIvaTitulo = (decimal)_titulo.VrReteIvaTitulo;
                _detalleSolicitudes.PorcReteFuenteTitulo = (decimal)_titulo.PorcReteFuenteTitulo;
                _detalleSolicitudes.VrReteFuenteTitulo = (decimal)_titulo.VrReteFuenteTitulo;
                _detalleSolicitudes.PorcReteIcaTitulo = (decimal)_titulo.PorcReteIcaTitulo;
                _detalleSolicitudes.VrReteIcaTitulo = (decimal)_titulo.VrReteIcaTitulo;
                _detalleSolicitudes.VrNetoTitulo = (decimal)_titulo.VrNetoTitulo;
                _detalleSolicitudes.VrReserva = (decimal)vrCobertura;
                _detalleSolicitudes.VrComision = (decimal)vrInteres;

                //_detalleSolicitudes.VrEntregarCliente = vrNegociacion - vrInteres - vrGao - ivaGao + retefuenteIntereses + reteIcaIntereses;
                _detalleSolicitudes.VrEntregarCliente = 0;

                _detalleSolicitudes.ConceptoDetalleSolicitud = _titulo.EstadosTitulos.NomEstadoTitulo;
                _detalleSolicitudes.IdUsrMod = tercero.IdTercero;
                _detalleSolicitudes.FecModifica = DateTime.Now;

                await _detalleSolicitudesManager.Insertar(_detalleSolicitudes);
                LiquidarSolicitudOpe(_detalleSolicitudes);


            }

        }

        public void CalcularInteresesImpuestos(Identity.Terceros tercero, decimal vrIntereses)
        {
            switch (tercero.TipoContribuyente)
            {
                case (int)EFactoringManagerConsts.TipoContribuyente.gNatural:
                    if ((bool)tercero.AgenteReteFuente) _liquidacionContext.ReteFuenteIntereses = vrIntereses * ((decimal)_liquidacionContext.TasaRetefuente / 100);
                    break;
                case (int)EFactoringManagerConsts.TipoContribuyente.gJuridico:
                    if ((bool)tercero.AgenteReteFuente) _liquidacionContext.ReteFuenteIntereses = vrIntereses * ((decimal)_liquidacionContext.TasaRetefuente / 100);
                    if ((bool)tercero.AgenteReteIca) _liquidacionContext.ReteIcaIntereses = vrIntereses * ((decimal)_liquidacionContext.TasaReteIca / 100);
                    break;
                case (int)EFactoringManagerConsts.TipoContribuyente.gGranContribuyente:
                    if ((bool)tercero.AgenteReteFuente) _liquidacionContext.ReteFuenteIntereses = vrIntereses * ((decimal)_liquidacionContext.TasaRetefuente / 100);
                    break;
            }
        }

        public async Task CargarImpuestos(string tipoDocumento, int idOficina, string idConcepto)
        {
            IList<Identity.ImpDocumentosOperativos> _impDocumentosOperativos = await _impDocumentosOperativosManager.ObtenerImpDocumentosOperativos(tipoDocumento, idOficina, idConcepto);
            foreach (var impuesto in _impDocumentosOperativos)
            {
                switch (impuesto.IdImpuesto)
                {
                    case "IVA":
                        _liquidacionContext.TasaIva = impuesto.PorcImpuesto;
                        break;
                    case "REF":
                        _liquidacionContext.TasaRetefuente = impuesto.PorcImpuesto;
                        _liquidacionContext.BaseReteFuente = impuesto.VrBaseImpuesto;
                        break;
                    case "ICA":
                        _liquidacionContext.TasaReteIca = impuesto.PorcImpuesto;
                        _liquidacionContext.BaseReteIca = impuesto.VrBaseImpuesto;
                        break;
                    case "REI":
                        _liquidacionContext.TasaReteIva = impuesto.PorcImpuesto;
                        break;
                    case "GAO":
                        _liquidacionContext.TasaGao = impuesto.PorcImpuesto;
                        break;
                }
            }
        }

        public decimal? CalcularInteres(decimal? capital, float? tasaInteres, int? plazo, int modalidadInteres, short? baseDias, int tipoTasaInteres, int tipoOperacion, int modalidadOperacion)
        {

            decimal? interes = 0;
            double? _tasaInteres = 0;
            switch (tipoTasaInteres)
            {
                case (int)EFactoringManagerConsts.TiposTasaInteres.gEfectiva:
                    switch (modalidadInteres)
                    {
                        case (int)EFactoringManagerConsts.ModalidadesInteres.gVencida:
                            _tasaInteres = (double)(Math.Pow((1 + ((double)(tasaInteres) / 100)), (1 / (double)(Convert.ToInt16(baseDias) / Convert.ToInt16(plazo)))) - 1);
                            _tasaInteres = _tasaInteres * (Convert.ToInt16(baseDias) / plazo) * 100;
                            break;
                    }
                    break;
            }
            interes = capital * Convert.ToDecimal(_tasaInteres / 100) / ((Convert.ToInt16(baseDias)) / Convert.ToInt16(plazo));
            return interes;

        }

        public void LiquidarSolicitudOpe(Identity.DetalleSolicitudes detSolicitudOpe)
        {
            //var _detalleSolicitud = _detalleSolicitudesManager.ObtDetalleSolicitudes(solicitudOpe);
            //foreach (var detalleSolicitud in _detalleSolicitud)
            //{
            _liquidacionContext.TotalVrAdmon = _liquidacionContext.TotalVrAdmon + detSolicitudOpe.VrAdmon;
            _liquidacionContext.TotalVrEntregarBruto = _liquidacionContext.TotalVrEntregarBruto + (decimal)detSolicitudOpe.VrEntregarCliente;
            _liquidacionContext.TotalVrEntregarNeto = _liquidacionContext.TotalVrEntregarNeto + (decimal)(detSolicitudOpe.VrPresente - detSolicitudOpe.VrComision);
            _liquidacionContext.TotalVrIntereses = _liquidacionContext.TotalVrIntereses + (decimal)detSolicitudOpe.VrComision;
            _liquidacionContext.TotalReteFuenteInteres = _liquidacionContext.TotalReteFuenteInteres + detSolicitudOpe.ReteFuenteIntereses;
            _liquidacionContext.TotalVrNetoRecibir = _liquidacionContext.TotalVrNetoRecibir + detSolicitudOpe.VrNetoRecibir;
            _liquidacionContext.TotalVrComisionBruto = _liquidacionContext.TotalVrComisionBruto + (decimal)detSolicitudOpe.VrComision;
            _liquidacionContext.IvaComision = _liquidacionContext.IvaComision + detSolicitudOpe.IvaAdmon;
            _liquidacionContext.ReteFuenteComision = 0;
            _liquidacionContext.ReteIcaComision = 0;
            _liquidacionContext.ReteIvaComision = 0;
            _liquidacionContext.TotalVrComisionNeto = _liquidacionContext.TotalVrComisionNeto + (decimal)detSolicitudOpe.VrComision + detSolicitudOpe.VrAdmon + detSolicitudOpe.IvaAdmon;
            _liquidacionContext.TotalVrNetoTitulo = _liquidacionContext.TotalVrNetoTitulo + (decimal)detSolicitudOpe.VrNetoTitulo;

            //}

            //ActualizarSolicitudOperacion(solicitudOpe);
        }

        public async Task<int> Insertar(Identity.SolicitudesOperaciones solicitudesOperaciones)
        {
            return await _solicitudesOperacionesRepositorio.InsertAndGetIdAsync(solicitudesOperaciones);

        }

        public async Task<bool> ActualizarTotalSolicitudOperacion(Identity.SolicitudesOperaciones solicitudOperacion)
        {

            if (solicitudOperacion == null)
            {
                return false;
            }
            else
            {
                solicitudOperacion.TotalVrAdmon = _liquidacionContext.TotalVrAdmon;
                solicitudOperacion.TotalVrEntregarBruto = _liquidacionContext.TotalVrEntregarBruto;
                solicitudOperacion.TotalVrEntregarNeto = _liquidacionContext.TotalVrEntregarNeto;
                solicitudOperacion.TotalVrIntereses = _liquidacionContext.TotalVrIntereses;
                solicitudOperacion.TotalReteFuenteInteres = _liquidacionContext.TotalReteFuenteInteres;
                solicitudOperacion.TotalVrNetoRecibir = _liquidacionContext.TotalVrNetoRecibir;
                solicitudOperacion.TotalVrComisionBruto = _liquidacionContext.TotalVrComisionBruto;
                solicitudOperacion.IvaComision = _liquidacionContext.IvaComision;
                solicitudOperacion.ReteFuenteComision = _liquidacionContext.ReteFuenteComision;
                solicitudOperacion.ReteIcaComision = _liquidacionContext.ReteIcaComision;
                solicitudOperacion.ReteIvaComision = _liquidacionContext.ReteIvaComision;
                solicitudOperacion.TotalVrComisionNeto = _liquidacionContext.TotalVrComisionNeto;
                solicitudOperacion.TotalVrNetoTitulo = _liquidacionContext.TotalVrNetoTitulo;
                solicitudOperacion.EstadoSolicitud = (int)EFactoringManagerConsts.EstadosSolicitudes.gEnviada;
                await _solicitudesOperacionesRepositorio.UpdateAsync(solicitudOperacion);
                return true;
            }
        }

        public async Task<bool> ActualizarEstadoSolicitud(Identity.SolicitudesOperaciones solicitudOperacion, int estadoSolicitud, string nombreArchivo)
        {

            if (solicitudOperacion == null)
                return false;

            solicitudOperacion = await _solicitudesOperacionesRepositorio.GetAll()
                .Include(s => s.DetalleSolicitudes)
                //.ThenInclude(d => d.Titulo)
                .Include(s => s.DetalleSolicitudes)
                .ThenInclude(d => d.Pagador)
                .Where(a => a.TipoDocumento == solicitudOperacion.TipoDocumento &&
                        a.IdOficina == solicitudOperacion.IdOficina &&
                        a.IdSolicitud == solicitudOperacion.IdSolicitud).FirstOrDefaultAsync();


            if (solicitudOperacion == null)
                return false;

            solicitudOperacion.EstadoSolicitud = estadoSolicitud;

            //Actualizar Titulos Acepta Pagador al enviar
            if (estadoSolicitud == (int)EFactoringManagerConsts.EstadosSolicitudes.gEnviada
                && solicitudOperacion.TipoDocumento == EFactoringManagerConsts.gSolicitudSol
                && _appSettings.AceptaPagador
                )
            {
                solicitudOperacion.TotalFacturas = solicitudOperacion.DetalleSolicitudes.Count;

                foreach (var _detalleSol in solicitudOperacion.DetalleSolicitudes)
                {
                    var titulo = await _titulosManager.ObtenerTituloPorId(solicitudOperacion.NumeroTercero, _detalleSol.IdTitulo, _detalleSol.TipoTitulo);


                    if (titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gAceptadaPagador)
                    {
                        continue;
                    }
                    else if (_detalleSol.Pagador.Aceptacion)
                    {
                        await _titulosManager.ActualizarEstadoTitulo(titulo, (int)EFactoringManagerConsts.EstadosTitulos.gPTEAceptacion, null, solicitudOperacion);  //PTE Aceptacion
                        solicitudOperacion.EstadoSolicitud = (int)EFactoringManagerConsts.EstadosSolicitudes.gPTEAceptacion;
                    }
                    else
                    {
                        await _titulosManager.ActualizarEstadoTitulo(titulo, (int)EFactoringManagerConsts.EstadosTitulos.gAceptadaPagador, null, solicitudOperacion);  //Aceptada Pagador
                        await _detalleSolicitudesManager.Actualizar(_detalleSol, (int)EFactoringManagerConsts.EstadosTitulos.gAceptadaPagador, 0);
                        solicitudOperacion.TotalAceptadas += 1;
                    }
                }
            }

            //Actualizar Titulos Acepta Pagador al enviar
            if (estadoSolicitud == (int)EFactoringManagerConsts.EstadosSolicitudes.gInicial
                && solicitudOperacion.TipoDocumento == EFactoringManagerConsts.gSolicitudSol
                && _appSettings.SolicitudOperacionesEstadoInicialEmisor
                )
            {
                Identity.Notificacion notificacion = new Identity.Notificacion()
                {
                    TipoDocumento = solicitudOperacion.TipoDocumento,
                    IdOficina = solicitudOperacion.IdOficina,
                    IdDocumento = solicitudOperacion.IdSolicitud,
                    IdCorreo = _appSettings.IdCorreoNotificacionSolicitudOperacion
                };
                _notificacionesManager.Notificar(notificacion);
            }


            await _solicitudesOperacionesRepositorio.UpdateAsync(solicitudOperacion);
            return true;
        }

        public async Task<Identity.SolicitudesOperaciones> ObtenerSolicitudOpePorId(string tipoDocumento, int idOficina, int idSolicitud)
        {

            var solicitud = await _solicitudesOperacionesRepositorio.GetAll().Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdSolicitud == idSolicitud)
                             .Include(x => x.Tercero)
                             .Include(x => x.EstadoSol).FirstOrDefaultAsync();

            return solicitud;
        }

        public async Task<List<Identity.SolicitudesOperaciones>> ObtenerSolicitudesOpePorTercero(string idTercero)
        {
            var terceros = await _tercerosManager.ObtenerTerceroId(idTercero);

            var lstterceros = terceros.Where(x => x.TerceroPadre != null)
                                        .Select(x => x.TerceroPadre.Id).ToList();
            //return _detalleSolicitudesManager.ObtenerSolicitudesOpePorTercero(tercero);
            //Consultar si TerceroPadre es pagador en detalle de la solicitud

            return await _solicitudesOperacionesRepositorio.GetAll().Where(a => lstterceros.Contains(a.NumeroTercero))
                                                                .Include(x => x.Tercero)
                                                                .Include(x => x.EstadoSol).ToListAsync();

        }

        public async Task<Tuple<List<Identity.SolicitudesOperaciones>, int>> ObtenerSolicitudesOpe(
            int numeroTercero,
            string tipoDocumento,
            int idOficina,
            int idSolicitud,
            int skip,
            int limit,
            string sortField,
            int sortOrder)
        {
            var terceroPadre = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);


            IQueryable<Identity.SolicitudesOperaciones> query;

            if (terceroPadre != null && terceroPadre.Comisionista)
            {
                query = _solicitudesOperacionesRepositorio.GetAll()
                    .Include(x => x.Tercero)
                    .Include(x => x.EstadoSol)
                    .Where(x =>
                        x.Tercero.IdComisionista == terceroPadre.IdTercero &&
                        (string.IsNullOrEmpty(tipoDocumento) || x.TipoDocumento == tipoDocumento) &&
                        x.IdOficina == idOficina &&
                        (idSolicitud == 0 || x.IdSolicitud == idSolicitud));
            }
            else
            {
                query = _solicitudesOperacionesRepositorio.GetAll()
                    .Include(x => x.Tercero)
                    .Include(x => x.EstadoSol)
                    .Where(x =>
                        (numeroTercero == 0 || x.Tercero.Id == numeroTercero) &&
                        (string.IsNullOrEmpty(tipoDocumento) || x.TipoDocumento == tipoDocumento) &&
                        x.IdOficina == idOficina &&
                        (idSolicitud == 0 || x.IdSolicitud == idSolicitud));
            }

            var total = await query.CountAsync();

            // Ordenamiento dinámico
            var orderMap = new Dictionary<string, Expression<Func<Identity.SolicitudesOperaciones, object>>>
            {
                [nameof(Identity.SolicitudesOperaciones.IdSolicitud)] = x => x.IdSolicitud,
                [nameof(Identity.SolicitudesOperaciones.FecSolicitud)] = x => x.FecSolicitud,
                [nameof(Identity.SolicitudesOperaciones.NumeroTercero)] = x => x.NumeroTercero,
                [nameof(Identity.SolicitudesOperaciones.EstadoSolicitud)] = x => x.EstadoSolicitud,
                [nameof(Identity.SolicitudesOperaciones.Tercero.RazonSocial)] = x => x.Tercero.RazonSocial
            };

            query = query.ApplyOrdering(
                sortField,
                sortOrder,
                orderMap,
                x => x.IdSolicitud);

            // Paginación
            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return new Tuple<List<Identity.SolicitudesOperaciones>, int>(await query.ToListAsync(), total);
        }

        public async Task<List<Identity.DetalleSolicitudes>> ObtenerDetalleSolicitudPorId(string tipoDocumento, int idOficina, int idSolicitudes)
        {
            return await _detalleSolicitudesManager.ObtDetalleSolicitudes(tipoDocumento, idOficina, idSolicitudes);
        }

        /// <summary>
        /// Genera un archivo Zip con los PDF (representación gráfica) de todos los títulos
        /// asociados a una solicitud de operación, a través de sus detalles de solicitud.
        /// </summary>
        public async Task<byte[]> ObtenerZipPdfTitulosSolicitud(string tipoDocumento, int idOficina, int idSolicitud)
        {
            var titulos = await _detalleSolicitudesManager.ObtTitulosFromDetalleSolicitudes(tipoDocumento, idOficina, idSolicitud, 0);

            if (titulos == null || !titulos.Any())
                throw new Exception("La solicitud no tiene títulos asociados");

            using (var compressedStream = new MemoryStream())
            {
                int entries = 0;
                using (var zip = new System.IO.Compression.ZipArchive(compressedStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    foreach (var titulo in titulos)
                    {
                        if (string.IsNullOrEmpty(titulo.RepresentacionGrafica))
                            continue;

                        var pdfBytes = _titulosManager.ObtenerPdfTitulo(titulo.RepresentacionGrafica);
                        var entry = zip.CreateEntry($"{titulo.IdTitulo}.pdf");
                        using (var entryStream = entry.Open())
                        {
                            await entryStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
                        }
                        entries++;
                    }
                }

                if (entries == 0)
                    throw new Exception("Ninguno de los títulos de la solicitud tiene un PDF asociado");

                return compressedStream.ToArray();
            }
        }

        public async Task<bool> ValidarCodigoTercero(Identity.SolicitudesOperaciones solicitudOperacion, int codigoTercero)
        {
            if (await _tercerosManager.VerificarCodigoTercero(solicitudOperacion.Tercero.IdTercero, codigoTercero))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// El estado de autorización ya no exise en la configuración de estados titulo
        /// </summary>
        /// <param name="solicitudOperacion"></param>
        /// <returns></returns>
        [Obsolete("Este metodo ya no se utiliza en la implementación de solicitudes")]
        public async Task<bool> AutorizarSolicitudOperacion(Identity.SolicitudesOperaciones solicitudOperacion, int estadoSolicitud)
        {
            var ActEstadoSolicitud = await ActualizarEstadoSolicitud(solicitudOperacion, estadoSolicitud, null);

            //var detalleSolicitud = _detalleSolicitudesManager.ObtDetalleSolicitudes(solicitudOperacion.TipoDocumento, solicitudOperacion.IdOficina, solicitudOperacion.IdSolicitud);
            //if (detalleSolicitud == null)
            //    return false;


            /*No atualizar estado titulo
            foreach (var _detalleSol in detalleSolicitud)
            {
                var titulo = _titulosManager.ObtenerTituloPorId(_detalleSol.NumeroTercero, _detalleSol.IdTitulo, _detalleSol.TipoTitulo);
                _titulosManager.ActualizarEstadoTitulo(titulo, (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable, null);  //PTE Aceptacion
            }
            */
            return ActEstadoSolicitud;//_tercerosManager.ActualizarConsecutivoAnexoTercero(solicitudOperacion.Tercero);
        }

        public async Task<bool> RechazarSolicitudOperacion(Identity.SolicitudesOperaciones solicitudOperacion)
        {
            if (await ActualizarEstadoSolicitud(solicitudOperacion, (int)EFactoringManagerConsts.EstadosSolicitudes.gNoAceptada, null))
            {
                #region Código comentado
                /*
                var detalleDocAnexoCR = _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(solicitudOperacion.Tercero.IdTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gAnexoConvenioRecaudo, solicitudOperacion.TipoDocumento, solicitudOperacion.IdOficina, solicitudOperacion.IdSolicitud);
                var detalleDocMandato = _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(solicitudOperacion.Tercero.IdTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gMandato, solicitudOperacion.TipoDocumento, solicitudOperacion.IdOficina, solicitudOperacion.IdSolicitud);
                var detalleDocEndoso = _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(solicitudOperacion.Tercero.IdTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gEndoso, solicitudOperacion.TipoDocumento, solicitudOperacion.IdOficina, solicitudOperacion.IdSolicitud);

                if (detalleDocAnexoCR != null && detalleDocMandato != null && detalleDocEndoso != null)
                {
                    if (System.IO.File.Exists(detalleDocAnexoCR.DirDocumento))
                    {
                        File.SetAttributes(detalleDocAnexoCR.DirDocumento, FileAttributes.Normal);
                        File.Delete(detalleDocAnexoCR.DirDocumento);
                    }
                    _detalleDocumentosManager.DelDetalleDocumento(detalleDocAnexoCR);
                    if (System.IO.File.Exists(detalleDocMandato.DirDocumento))
                    {
                        File.SetAttributes(detalleDocMandato.DirDocumento, FileAttributes.Normal);
                        File.Delete(detalleDocMandato.DirDocumento);
                    }
                    _detalleDocumentosManager.DelDetalleDocumento(detalleDocMandato);
                    if (System.IO.File.Exists(detalleDocEndoso.DirDocumento))
                    {
                        File.SetAttributes(detalleDocEndoso.DirDocumento, FileAttributes.Normal);
                        File.Delete(detalleDocEndoso.DirDocumento);
                    }
                    _detalleDocumentosManager.DelDetalleDocumento(detalleDocEndoso);
                }
                */
                #endregion

                var detalleSolicitud = await _detalleSolicitudesManager.ObtDetalleSolicitudes(solicitudOperacion.TipoDocumento, solicitudOperacion.IdOficina, solicitudOperacion.IdSolicitud);
                if (detalleSolicitud == null)
                    return false;

                foreach (var _detalleSol in detalleSolicitud)
                {
                    var titulo = await _titulosManager.ObtenerTituloPorId(_detalleSol.NumeroTercero, _detalleSol.IdTitulo, _detalleSol.TipoTitulo);
                    await _titulosManager.ActualizarEstadoTitulo(titulo, (int)EFactoringManagerConsts.EstadosTitulos.gRechazadaCli, null, solicitudOperacion);
                }
                return true;

            }
            else return false;
        }

        public async Task<bool> validarModificacionSolicitudes(int numeroTercero)
        {
            var solicitudes = await _solicitudesOperacionesRepositorio.GetAll().Where(a => a.NumeroTercero == numeroTercero && a.Actualiza == true).ToListAsync();
            if (solicitudes.Count > 0)
            {
                foreach (var solicitud in solicitudes)
                {
                    solicitud.Actualiza = false;
                    await _solicitudesOperacionesRepositorio.UpdateAsync(solicitud);
                }
                return true;
            }
            else return false;
        }

        public async Task<byte[]> GenerarPdfSolicitud(string tipoDoc, int idOficina, int idSolicitud)
        {

            var solicitudes = _solicitudesOperacionesRepositorio.ObtenerProyeccionDetalleSolicitudes(tipoDoc, idOficina, idSolicitud);
            var jsonData = JsonConvert.SerializeObject(solicitudes);

            ReportRequest reportRequest = new()
            {
                ReportName = "FormatoOperacionesCLientesSol.rpt",
                Data = jsonData,
                Format = ReportFormat.Pdf
            };

            return await _reportApiProxy.GenerateReportAsync(reportRequest);


        }

        

        public async Task<bool> CargarDocumentosAutorizacionSol(string tipoDocumento, int idOficina, int idSolicitud, string idTercero)
        {
            bool DocGenerados = true;
            var detalleDocAnexoCR = await _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gAnexoConvenioRecaudo, tipoDocumento, idOficina, idSolicitud);
            var detalleDocMandato = await _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gMandato, tipoDocumento, idOficina, idSolicitud);
            var detalleDocEndoso = await _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gEndoso, tipoDocumento, idOficina, idSolicitud);


            //var detalleDocCesion = _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipoSol(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gCesion, tipoDocumento, idOficina, idSolicitud);
            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);
            var oficina = await _oficinaRepositorio.GetAll().Where(a => a.IdOficina == idOficina).FirstOrDefaultAsync();
           
            string rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ", "_") + "\\Operaciones\\" + idSolicitud.ToString();

            if (!(Directory.Exists(rutaFichero)))
            {
                Directory.CreateDirectory(rutaFichero);
            }


            var solicitud = _solicitudesOperacionesRepositorio
                                                    .ObtenerProyeccionDetalleSolicitudes(tipoDocumento, idOficina, idSolicitud);

            var pagadoresTitulosSol = _solicitudesOperacionesRepositorio
                                                    .ObtenerPagadoresTitulosPorSolicitud(tipoDocumento, idOficina, idSolicitud);

           

            try
            {
                var archivo1 = await GenerarArchivoReporte(
                    "AnexoConvenio.rpt",
                    solicitud,
                    rutaFichero,
                    detalleDocAnexoCR,
                    idTercero);

                var archivo2 = await GenerarArchivoReporte(
                    "Mandato.rpt",
                    solicitud,
                    rutaFichero,
                    detalleDocMandato,
                    idTercero);

                var archivo3 = await GenerarArchivoReporte(
                    "EndosoPagador.rpt",
                    pagadoresTitulosSol,
                    rutaFichero,
                    detalleDocEndoso,
                    idTercero);
            }
            catch
            {
                DocGenerados = false;
            }
            #region Código Anterior
            /*
            //Documento Anexo Convenio
            ReportDocument rptConvenio = new ReportDocument();
            System.IO.FileInfo infoConvenio = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string DirectoryRoot = "C:\\WEB";
            //"C:\\Publish";

            if (string.IsNullOrEmpty(DirectoryRoot))
                throw new Exception("No se encuentra configurado la ruta de la aplicación en el archivo de configuración");

            rptConvenio.Load("C:\\WEB\\BackEnd\\Reportes\\AnexoConvenio.rpt", OpenReportMethod.OpenReportByTempCopy);

            var solicitud = _solicitudesOperacionesRepositorio.ObtenerProyeccionDetalleSolicitudes(tipoDocumento, idOficina, idSolicitud);

            rptConvenio.SetDataSource(solicitud);

            // System.IO.Stream streamConvenio = rptConvenio.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Text);
            fichero = rutaFichero + "\\" + string.Format(detalleDocAnexoCR.Documentos.TipoDocumento.NomDocumento + "_{0:ddMMyyyyHHmmss}.pdf", DateTime.Now);
            rptConvenio.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fichero);

            if (!System.IO.File.Exists(fichero))
            {
                DocGenerados = false;
            }
            else
            {

                if (System.IO.File.Exists(detalleDocAnexoCR.DirDocumento))
                {
                    File.SetAttributes(detalleDocAnexoCR.DirDocumento, FileAttributes.Normal);
                    File.Delete(detalleDocAnexoCR.DirDocumento);
                }

                detalleDocAnexoCR.DirDocumento = fichero;
                detalleDocAnexoCR.FecDocumento = DateTime.Now;
                //detalleDocAnexoCR.Firma = false;
                detalleDocAnexoCR.FecModifica = DateTime.Now;
                detalleDocAnexoCR.IdUsrMod = idTercero;
                _detalleDocumentosManager.ActualizarDirDocDetalleDocumentos(detalleDocAnexoCR);
            }

            //Documento Mandato
            ReportDocument rptMandato = new ReportDocument();
            System.IO.FileInfo infoMandato = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrEmpty(DirectoryRoot))
                throw new Exception("No se encuentra configurado la ruta de la aplicación en el archivo de configuración");

            rptMandato.Load("C:\\WEB\\BackEnd\\Reportes\\Mandato.rpt", OpenReportMethod.OpenReportByTempCopy);
            rptMandato.SetDataSource(solicitud);
            // System.IO.Stream streamMandato = rptMandato.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            fichero = rutaFichero + "\\" + string.Format(detalleDocMandato.Documentos.TipoDocumento.NomDocumento + "_{0:ddMMyyyyHHmmss}.pdf", DateTime.Now);
            rptMandato.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fichero);

            if (!System.IO.File.Exists(fichero))
            {
                DocGenerados = false;
            }
            else
            {
                if (System.IO.File.Exists(detalleDocMandato.DirDocumento))
                {
                    File.SetAttributes(detalleDocMandato.DirDocumento, FileAttributes.Normal);
                    File.Delete(detalleDocMandato.DirDocumento);
                }

                detalleDocMandato.DirDocumento = fichero;
                detalleDocMandato.FecDocumento = DateTime.Now;
                //detalleDocMandato.Firma = false;
                detalleDocMandato.FecModifica = DateTime.Now;
                detalleDocMandato.IdUsrMod = idTercero;
                _detalleDocumentosManager.ActualizarDirDocDetalleDocumentos(detalleDocMandato);
            }


            //Documento Endoso
            ReportDocument rptEndoso = new ReportDocument();
            System.IO.FileInfo infoEndoso = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrEmpty(DirectoryRoot))
                throw new Exception("No se encuentra configurado la ruta de la aplicación en el archivo de configuración");

            rptEndoso.Load("C:\\WEB\\BackEnd\\Reportes\\EndosoPagador.rpt", OpenReportMethod.OpenReportByTempCopy);
            var PagadoresTitulosSol = _solicitudesOperacionesRepositorio.ObtenerPagadoresTitulosPorSolicitud(tipoDocumento, idOficina, idSolicitud);
            rptEndoso.SetDataSource(PagadoresTitulosSol);

            //System.IO.Stream streamEndoso = rptEndoso.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            fichero = rutaFichero + "\\" + string.Format(detalleDocEndoso.Documentos.TipoDocumento.NomDocumento + "_{0:ddMMyyyyHHmmss}.pdf", DateTime.Now);
            rptEndoso.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fichero);

            if (!System.IO.File.Exists(fichero))
            {
                DocGenerados = false;
            }
            else
            {
                if (System.IO.File.Exists(detalleDocEndoso.DirDocumento))
                {
                    File.SetAttributes(detalleDocEndoso.DirDocumento, FileAttributes.Normal);
                    File.Delete(detalleDocEndoso.DirDocumento);
                }

                detalleDocEndoso.DirDocumento = fichero;
                detalleDocEndoso.FecDocumento = DateTime.Now;
                detalleDocEndoso.Firma = false;
                detalleDocEndoso.FecModifica = DateTime.Now;
                detalleDocEndoso.IdUsrMod = idTercero;
                _detalleDocumentosManager.ActualizarDirDocDetalleDocumentos(detalleDocEndoso);

            }

            //WIGELO:29/12/2021 PORQUE ESTA COMENTARIADO CON DIEGO
            //Documento Cesion
            /*ReportDocument rptCesion = new ReportDocument();
            System.IO.FileInfo infoCesion = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrEmpty(DirectoryRoot))
                throw new Exception("No se encuentra configurado la ruta de la aplicación en el archivo de configuración");

            rptCesion.Load("C:\\WEB\\BackEnd\\Reportes\\CartaCesion.rpt", OpenReportMethod.OpenReportByTempCopy);
            var PagadoresTitulosSolCesion = _solicitudesOperacionesRepositorio.ObtenerPagadoresTitulosPorSolicitud(tipoDocumento, idOficina, idSolicitud);
            rptCesion.SetDataSource(PagadoresTitulosSolCesion);
            string fechaActual = DateTime.Now.ToString("dd", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + " de " + DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + " de " + DateTime.Now.ToString("yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
            rptCesion.SetParameterValue("FechaActual", fechaActual);
            //System.IO.Stream streamEndoso = rptEndoso.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            fichero = rutaFichero + "\\" + string.Format(detalleDocCesion.Documentos.TipoDocumento.NomDocumento + "_{0:ddMMyyyyHHmmss}.pdf", DateTime.Now);
            rptCesion.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fichero);

            if (!System.IO.File.Exists(fichero))
            {
                DocGenerados = false;
            }
            else
            {
                if (System.IO.File.Exists(detalleDocCesion.DirDocumento))
                {
                    File.SetAttributes(detalleDocCesion.DirDocumento, FileAttributes.Normal);
                    File.Delete(detalleDocCesion.DirDocumento);
                }

                detalleDocCesion.DirDocumento = fichero;
                detalleDocCesion.FecDocumento = DateTime.Now;
                detalleDocCesion.Firma = false;
                detalleDocCesion.FecModifica = DateTime.Now;
                detalleDocCesion.IdUsrMod = idTercero;
                _detalleDocumentosManager.ActualizarDirDocDetalleDocumentos(detalleDocCesion);

            }*/
            #endregion

            return DocGenerados;

        }

        private async Task<string> GenerarArchivoReporte(
                                                string reportName,
                                                object data,
                                                string rutaFichero,
                                                dynamic detalleDocumento,
                                                string idTercero)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            var reportRequest = new ReportRequest
            {
                ReportName = reportName,
                Data = jsonData,
                Format = ReportFormat.Pdf
            };

            var pdfBytes = await _reportApiProxy.GenerateReportAsync(reportRequest);

            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new Exception($"No fue posible generar el reporte {reportName}");

            string fichero = Path.Combine(
                rutaFichero,
                $"{detalleDocumento.Documentos.TipoDocumento.NomDocumento}_{DateTime.Now:ddMMyyyyHHmmss}.pdf"
            );

            await File.WriteAllBytesAsync(fichero, pdfBytes);

            if (File.Exists(detalleDocumento.DirDocumento))
            {
                File.SetAttributes(detalleDocumento.DirDocumento, FileAttributes.Normal);
                File.Delete(detalleDocumento.DirDocumento);
            }

            detalleDocumento.DirDocumento = fichero;
            detalleDocumento.FecDocumento = DateTime.Now;
            detalleDocumento.FecModifica = DateTime.Now;
            detalleDocumento.IdUsrMod = idTercero;

            await _detalleDocumentosManager.ActualizarDirDocDetalleDocumentos(detalleDocumento);

            return fichero;
        }


        public async Task GenerarArchivoCufeSolicitud(string tipoDocumento, int idOficina, int idSolicitud)
        {
            var solicitudOperacion = await _solicitudesOperacionesRepositorio.GetAll().Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdSolicitud == idSolicitud).FirstOrDefaultAsync();


            if (solicitudOperacion == null)
                return;

            var estadoSolicitud = await _estadosSolicitudesManager.ObtenerEstadosSolicitudes(tipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_Cufes);

            solicitudOperacion.EstadoSolicitud = estadoSolicitud.Id;

            await _solicitudesOperacionesRepositorio.UpdateAsync(solicitudOperacion);


            if (_billyConfiguration.Token != null)
            {
                _backgroundJobManager.Enqueue<UploadInvoiceBillyJob, BillyJobArgs>(new BillyJobArgs()
                {
                    IdOficina = idOficina,
                    IdSolicitud = idSolicitud,
                    TipoDocumento = tipoDocumento
                });
            }

            else if (_eBillFtpConfiguration.UserName != null)
            {
                _backgroundJobManager.Enqueue<GenerarArchivoSolicitudJob, FileFTPJobArgs>(new FileFTPJobArgs() { IdOficina = idOficina, IdSolicitud = idSolicitud, TipoDocumento = tipoDocumento });
            }
        }

        public async Task CargarFacturaCufeXSolicitud(string tipoDocumento, int idOficina, int idSolicitud)
        {
            var solicitudOperacion = await _solicitudesOperacionesRepositorio.GetAll().Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdSolicitud == idSolicitud).FirstOrDefaultAsync();


            if (solicitudOperacion == null)
                return;

            var estadoSolicitud = await _estadosSolicitudesManager.ObtenerEstadosSolicitudes(tipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_Cufes);

            solicitudOperacion.EstadoSolicitud = estadoSolicitud.Id;

            await _solicitudesOperacionesRepositorio.UpdateAsync(solicitudOperacion);

            _backgroundJobManager.Enqueue<UploadInvoiceBillyJob, BillyJobArgs>(new BillyJobArgs()
            {
                IdOficina = idOficina,
                IdSolicitud = idSolicitud,
                TipoDocumento = tipoDocumento
            });
        }

        public async Task GenerarArchivoEndosoSolicitud(FileEndosoFTPJobArgs fileEndoso)
        {
            FileEndosoFTPJobArgs fileReEndoso = JsonConvert.DeserializeObject<FileEndosoFTPJobArgs>(JsonConvert.SerializeObject(fileEndoso));
            EndosoBillyJobArgs endosoBillyJob = new EndosoBillyJobArgs()
            {
                TipoDocumento = fileEndoso.TipoDocumento,
                IdSolicitud = fileEndoso.IdSolicitud,
                IdOficina = fileEndoso.IdOficina,
                Titulos = new List<string>()
            };

            //foreach (var tituloftp in fileEndoso.Titulos)
            var index = 0;
            while (index < fileEndoso.Titulos.Count)
            {
                var tituloftp = fileEndoso.Titulos[index];
                var titulo = await _titulosManager.ObtenerTituloPorId(tituloftp.NumeroTercero, tituloftp.IdTitulo, tituloftp.TipoTitulo);

                if (titulo == null)
                    continue;

                endosoBillyJob.Titulos.Add(titulo.CufeTitulo);

                if (titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gReendosable)
                {
                    fileEndoso.Titulos.Remove(tituloftp);
                    index--;
                }
                else
                {
                    fileReEndoso.Titulos.Remove(fileReEndoso.Titulos.Where(x => x.IdTitulo == tituloftp.IdTitulo).FirstOrDefault());
                }
                //titulo.EstadoTitulo = (int)EstadosTitulos.gEnProceso;
                titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gEnProceso;
                await _titulosManager.UpdateTitulo(titulo);
                index++;
            }

            var solicitud = await _solicitudesOperacionesRepositorio.GetAll()
                            .Where(a => a.TipoDocumento == endosoBillyJob.TipoDocumento &&
                                a.IdOficina == endosoBillyJob.IdOficina &&
                                a.IdSolicitud == endosoBillyJob.IdSolicitud).FirstOrDefaultAsync();

            if (_billyConfiguration.Token != null)
            {
                _backgroundJobManager.Enqueue<UploadEndosoBillyJob, EndosoBillyJobArgs>(endosoBillyJob);

                solicitud.EstadoSolicitud = (int)EFactoringManagerConsts.EstadosSolicitudes.gEndosos;
            }
            else if (_eBillFtpConfiguration.UserName != null)
            {
                if (fileEndoso.Titulos.Count() > 0)
                    _backgroundJobManager.Enqueue<GenerarArchivoEndosoJob, FileEndosoFTPJobArgs>(fileEndoso);

                if (fileReEndoso.Titulos.Count() > 0)
                {
                    solicitud = await _solicitudesOperacionesRepositorio.GetAll()
                                    .Where(a => a.TipoDocumento == fileReEndoso.TipoDocumento &&
                                                a.IdOficina == fileReEndoso.IdOficina &&
                                                a.IdSolicitud == fileReEndoso.IdSolicitud).FirstOrDefaultAsync();

                    solicitud.NumeroInversionista = fileReEndoso.IdInversionista;

                    _backgroundJobManager.Enqueue<GenerarArchivoReendosoJob, FileEndosoFTPJobArgs>(fileReEndoso);
                }
            }

            await _solicitudesOperacionesRepositorio.UpdateAsync(solicitud);
        }

        public async Task EliminarDetalleSolicitud(int idSolicitud, int tipoTitulo, int numeroTercero, string idTitulo)
        {

            await _detalleSolicitudesManager.Eliminar(idSolicitud, tipoTitulo, numeroTercero, idTitulo);
        }


       
    }
}
