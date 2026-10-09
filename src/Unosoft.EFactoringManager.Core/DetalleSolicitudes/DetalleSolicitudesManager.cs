using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.DetalleSolicitudes.DTO;
using Unosoft.EFactoringManager.IRepositorios;
using UnoSoft.EFactoringManager.Services;

namespace Unosoft.EFactoringManager.DetalleSolicitudes
{
    public class DetalleSolicitudesManager : IDomainService
    {
        #region Atributos
        private readonly IDetalleSolicitudesRepositorio _detalleSolicitudesRepositorio;
        private readonly ITituloRepositorio _tituloRepositorio;
        private readonly IEstadosTitulosRepositorio _estadosTitulosRepositorio;
        #endregion

        public DetalleSolicitudesManager(IDetalleSolicitudesRepositorio detalleSolicitudesRepositorio, ITituloRepositorio tituloRepositorio, IEstadosTitulosRepositorio estadosTitulosRepositorio)
        {
            _detalleSolicitudesRepositorio = detalleSolicitudesRepositorio;
            _tituloRepositorio = tituloRepositorio;
            _estadosTitulosRepositorio = estadosTitulosRepositorio;
        }


        public async Task Insertar(string tipoDocumento, int idOficina, int IdSolicitud, IList<Identity.Titulos> titulos, Identity.Terceros tercero)
        {
            //WIGELO:19/01/2022 REVISAR LA UTILIZACION DE LAS VARIABLES
            decimal? vrNegociacion = 0;
            decimal? vrCobertura = 0;
            decimal? vrPresente = 0;
            decimal? vrInteres = 0;
            decimal? vrFuturo = 0;
            decimal vrGao = 0;
            decimal ivaGao = 0;


            //FOR DO:WIGELO:03/01/2022 NO SE REQUIERE

            for (int i = 0; i < titulos.Count; i++)
            {
                //Identity.Titulos _titulo = titulos[i];
                Identity.Titulos _titulo = await
                    _tituloRepositorio.GetAll()
                        .Where(t => t.IdTitulo == titulos[i].IdTitulo && t.NumeroPagador == titulos[i].NumeroPagador)
                        .Include(x => x.EstadosTitulos)
                        .FirstOrDefaultAsync();

                Identity.DetalleSolicitudes _detalleSolicitudes = new Identity.DetalleSolicitudes();

                vrNegociacion = _titulo.VrNegociacion;
                vrCobertura = _titulo.VrNetoTitulo - vrNegociacion;

                vrFuturo = vrNegociacion;



                //vrDescuento = vrFuturo - vrInteres;
                //vrNetoRecibir = vrFuturo - vrRetefuenteIntereses - vrReteIcaIntereses;
                //_detalleSolicitudes.TipoDocumento = EFactoringManagerConsts.gSolicitud;

                //FOR DO:WIGELO:03/01/2023 EL TIPO DE DOCUMENTO SE DEBE SE INCORPORAR DE LA SOLICITUD
                _detalleSolicitudes.TipoDocumento = tipoDocumento; ////EFactoringManagerConsts.gSolicitudCufe;

                //FOR DO:WIGELO:03/01/2023 LA OFICINA SE DEBE SE INCORPORAR DE LA SOLICITUD
                _detalleSolicitudes.IdOficina = (byte)idOficina; //tercero.IdOficina;
                _detalleSolicitudes.IdSolicitud = IdSolicitud;

                _detalleSolicitudes.NumeroTercero = tercero.Id;
                _detalleSolicitudes.IdTitulo = _titulo.IdTitulo;
                _detalleSolicitudes.TipoTitulo = _titulo.TipoTitulo;

                _detalleSolicitudes.NumeroPagador = _titulo.Pagador.Id;

                _detalleSolicitudes.FecInicial = _titulo.FecInicial;
                _detalleSolicitudes.FecFinal = _titulo.FecFinal;
                _detalleSolicitudes.FecEmision = _titulo.FecEmision;
                _detalleSolicitudes.FecProbable = _titulo.FecProbable;
                _detalleSolicitudes.DiasOperacion = (short)_titulo.Dias;

                _detalleSolicitudes.VrPresente = (decimal)vrPresente;
                _detalleSolicitudes.VrFuturo = (decimal)vrFuturo;
                _detalleSolicitudes.VrNegociacion = (decimal)_titulo.VrNegociacion;
                _detalleSolicitudes.VrIntereses = (decimal)vrInteres;

                _detalleSolicitudes.ReteFuenteIntereses = 0;
                _detalleSolicitudes.VrAdmon = vrGao;
                _detalleSolicitudes.IvaAdmon = ivaGao;
                _detalleSolicitudes.VrNetoRecibir = (decimal)vrFuturo;

                //_detalleSolicitudes.BaseCapitalInteres = (decimal)vrFuturo;
                _detalleSolicitudes.BaseCapitalInteres = 0;

                _detalleSolicitudes.PorcNegociacion = (float)tercero.PorcNegociacion;

                _detalleSolicitudes.PorcNegociacion = (float)_titulo.PorcNegociacion;

                //_detalleSolicitudes.vrne = (float)tercero.PorcNegociacion;
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

                _detalleSolicitudes.VrEntregarCliente = 0;
                _detalleSolicitudes.ConceptoDetalleSolicitud = _titulo.EstadosTitulos.NomEstadoTitulo;
                _detalleSolicitudes.IdUsrMod = tercero.IdTercero;
                _detalleSolicitudes.FecModifica = DateTime.Now;

                await Insertar(_detalleSolicitudes);
            }
        }

        public async Task Insertar(Identity.DetalleSolicitudes detalleSolicitudes)
        {
            await _detalleSolicitudesRepositorio.InsertAsync(detalleSolicitudes);

        }

        public async Task<List<Identity.DetalleSolicitudes>> ObtDetalleSolicitudes(string tipoDocumento, int idOficina, int idSolicitudes)
        {
            return await _detalleSolicitudesRepositorio.GetAll()
                                                 .Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdSolicitud == idSolicitudes)
                                                 .Include(x => x.Pagador)
                                                 .OrderBy(m => m.NumeroPagador).ToListAsync();
        }

        public async Task<List<Identity.Titulos>> ObtTitulosFromDetalleSolicitudes(string tipoDocumento, int idOficina, int idSolicitud, int numeroTercero)
        {
            var result = await _detalleSolicitudesRepositorio.GetAll()
                                                 //.Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdSolicitud == idSolicitud)
                                                 .Where(a => a.TipoDocumento == tipoDocumento && a.IdOficina == idOficina && a.IdSolicitud == idSolicitud && (numeroTercero == 0 || numeroTercero == 1 || a.NumeroTercero == numeroTercero || a.NumeroPagador == numeroTercero))
                                                 .Include(x => x.Titulo)
                                                 .ThenInclude(x => x.Tercero)
                                                 .Include(x => x.Titulo)
                                                 .ThenInclude(x => x.EstadosTitulos)
                                                 .Include(x => x.Titulo)
                                                 .ThenInclude(x => x.Pagador)
                                                 .OrderBy(x => x.Titulo.EstadoTitulo)
                                                 .ToListAsync();

            //.Select(x => x.Titulo).ToList();

            return result.Select(x => x.Titulo).ToList();
        }

        public async Task<List<Identity.DetalleSolicitudes>> ObtDetalleSolicitudesGrupoPagador(string tipoDocumento, int idOficina, int idSolicitudes)
        {
            var resultado = _detalleSolicitudesRepositorio.ObtenerDetalleSolicitudesPagador(tipoDocumento, idOficina, idSolicitudes);
            return resultado;
        }

        public async Task Eliminar(int idSolicitud, int tipoTitulo, int numeroTercero, string idTitulo)
        {
            var detSolicitudList = await _detalleSolicitudesRepositorio.GetAll()
                                                 .Where(a => a.IdSolicitud == idSolicitud && a.NumeroTercero == numeroTercero /*&& a.TipoTitulo == tipoTitulo && a.IdTitulo == idTitulo*/)
                                                 .Include(x => x.Titulo)
                                                 .Include(s => s.SolicitudOperacion)
                                                .ToListAsync();

            if (detSolicitudList == null)
                return;

            var detSolicitud = detSolicitudList.Where(a => a.TipoTitulo == tipoTitulo && a.IdTitulo == idTitulo).FirstOrDefault();

            if (detSolicitud == null)
                return;

            if (detSolicitud.SolicitudOperacion.EstadoSolicitud == (int)EFactoringManagerConsts.EstadosSolicitudes.gErrcufes
                || detSolicitud.SolicitudOperacion.EstadoSolicitud == (int)EFactoringManagerConsts.EstadosSolicitudes.gErrend)
            {
                if (!detSolicitudList.Exists(a => a.Titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gError
                && a.Titulo.IdTitulo != idTitulo))
                {
                    if (detSolicitudList.Exists(a => a.Titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gEndosable
                         || a.Titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable
                         || a.Titulo.EstadoTitulo == (int)EFactoringManagerConsts.EstadosTitulos.gReendosable))
                    {
                        //var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(detSolicitudList.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_ErrCufes);
                        //detSolicitudList.EstadoSolicitud = estadoSolicitud.Id;
                        //_solicitudesOperacionesRepositorio.Update(solicitud);

                        //Validar con el APPSETTING tenga la variable ActSolicitud en true
                    }
                }

            }

            _detalleSolicitudesRepositorio.DeleteObj(detSolicitud);

            var detSolTituloList = await _detalleSolicitudesRepositorio.GetAll()
                                        .Where(a => a.NumeroTercero == numeroTercero &&
                                        a.TipoTitulo == tipoTitulo && a.IdTitulo == idTitulo)
                                        .Include(x => x.Titulo)
                                        .Include(s => s.SolicitudOperacion)
                                        .AsNoTracking()
                                        .CountAsync();


            if (detSolTituloList <= 1)
            {
                await _tituloRepositorio.DeleteAsync(detSolicitud.Titulo);
            }
        }

        public async Task<bool> Actualizar(Identity.DetalleSolicitudes detalleSolicitudes, int idEstadoTitulo, decimal VrNetoPagar)
        {
            var _detalleSolicitudes = await _detalleSolicitudesRepositorio.GetAll().Where(a => a.IdSolicitud == detalleSolicitudes.IdSolicitud &&
                                        a.NumeroTercero == detalleSolicitudes.NumeroTercero && a.IdTitulo == detalleSolicitudes.IdTitulo).FirstOrDefaultAsync();

            if (_detalleSolicitudes == null)
                return false;


            var estadoTitulo = await _estadosTitulosRepositorio.GetAll().Where(a => a.Id == idEstadoTitulo).FirstOrDefaultAsync();

            _detalleSolicitudes.ConceptoDetalleSolicitud = estadoTitulo.NomEstadoTitulo;
            if (VrNetoPagar > 0) _detalleSolicitudes.VrNetoTitulo = VrNetoPagar;
            await _detalleSolicitudesRepositorio.UpdateAsync(_detalleSolicitudes);
            return true;

            //throw new MissingMethodException();
        }


        public async Task<List<Identity.SolicitudesOperaciones>> ObtenerSolicitudesOpePorTercero(Identity.Terceros tercero)
        {


            var result = await _detalleSolicitudesRepositorio.GetAll().Where(a => a.SolicitudOperacion.NumeroTercero == tercero.TerceroPadre.Id ||
                                                                         a.Titulo.NumeroPagador == tercero.TerceroPadre.Id)
                                                                .Include(x => x.SolicitudOperacion)
                                                                .ThenInclude(x => x.Tercero)
                                                                .Include(x => x.SolicitudOperacion)
                                                                .ThenInclude(x => x.EstadoSol)
                                                                .OrderByDescending(x => x.IdSolicitud)
                                                                //.Select(x => x.SolicitudOperacion)
                                                                //.Distinct()
                                                                .ToListAsync();

            return result.Select(x => x.SolicitudOperacion).Distinct().ToList();
        }

        public async Task<List<DetalleSolicitudDTO>> ObtDetalleSolicitudesXPagador(int numeroTercero, string tipoDocumento, int producto, DateTime? fechaInical, DateTime? fechaFinal, int estadoTitulo, int idSolicitud, string idTitulo)
        {

            var query = _detalleSolicitudesRepositorio.GetAll()
                .Include(x => x.Titulo)
                .Where(x => x.SolicitudOperacion.Producto == producto);

            if (tipoDocumento != "")
                query = query.Where(x => x.SolicitudOperacion.TipoDocumento == tipoDocumento);

            if (fechaInical != null && fechaInical != fechaFinal)
                query = query.Where(x => x.SolicitudOperacion.FecSolicitud >= fechaInical);

            if (fechaFinal != null)
                query = query.Where(x => x.SolicitudOperacion.FecSolicitud <= fechaFinal);

            if (idSolicitud != 0)
                query = query.Where(x => x.SolicitudOperacion.IdSolicitud == idSolicitud);


            if (!string.IsNullOrEmpty(idTitulo))
                query = query.Where(x => x.Titulo.IdTitulo == idTitulo);

            if (estadoTitulo >= 0)
                query = query.Where(x => x.Titulo.EstadoTitulo == estadoTitulo);

            if (numeroTercero >= 0)
                if (producto == 1)
                    query = query.Where(x => x.Titulo.NumeroPagador == numeroTercero);
                else
                    query = query.Where(x => x.Titulo.NumeroTercero == numeroTercero);


            return await query.Select(x => new DetalleSolicitudDTO
            {
                TipoDocumento = x.TipoDocumento,
                IdSolicitud = x.IdSolicitud,
                Emisor = x.SolicitudOperacion.Tercero.NomTercero,
                Pagador = x.Titulo.Pagador.NomTercero,
                IdTitulo = x.IdTitulo,
                NumeroTercero = x.Titulo.NumeroTercero,
                FecSolicitud = x.SolicitudOperacion.FecSolicitud,
                FecEmision = x.FecEmision,
                FecInicial = x.FecInicial,
                FecFinal = x.FecFinal,
                FecProbable = x.Titulo.FecProbable,
                Dias = x.DiasOperacion,
                VrNetoTitulo = x.VrNetoTitulo,
                EstadoTitulo = x.Titulo.EstadosTitulos.NomEstadoTitulo,
                RepresentacionGrafica = x.Titulo.RepresentacionGrafica

            }).ToListAsync();
        }
    }
}
