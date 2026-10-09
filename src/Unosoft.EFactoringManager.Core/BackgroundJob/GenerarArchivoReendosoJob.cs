using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Unosoft.EFactoringManager.BackgroundJob.DTO;
using Unosoft.EFactoringManager.EstadosSolicitudes;
using Unosoft.EFactoringManager.FTP;
using Unosoft.EFactoringManager.IRepositorios;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.BackgroundJob
{
    public class GenerarArchivoReendosoJob : BackgroundJob<FileEndosoFTPJobArgs>, ITransientDependency
    {

        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly FTPManager _ftpManager;
        private readonly EstadosSolicitudesManager _estadosSolicitudesManager;
        private readonly IOficinasRepositorio _oficinaRepositorio;

        private readonly Dictionary<int, string> TiposEndoso;


        public GenerarArchivoReendosoJob(ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio,
            EstadosSolicitudesManager estadosSolicitudesManager,
            FTPManager ftpManager,
            IOficinasRepositorio oficinaRepositorio)
        {
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;
            _estadosSolicitudesManager = estadosSolicitudesManager;
            _ftpManager = ftpManager;
            _oficinaRepositorio = oficinaRepositorio;

            TiposEndoso = new Dictionary<int, string>(){
                { TipoEndoso.ConResposabilidad, CodigosEndoso.ConResposabilidad },
                { TipoEndoso.SinResponsabilidad, CodigosEndoso.SinResponsabilidad },
                { TipoEndoso.CesionOrdinaria, CodigosEndoso.CesionOrdinaria }

            };
        }


        [UnitOfWork]
        public override void Execute(FileEndosoFTPJobArgs args)
        {


            var solicitud = _solicitudesOperacionesRepositorio
              .GetAll()
              .Where(a => a.TipoDocumento == args.TipoDocumento && a.IdOficina == args.IdOficina && a.IdSolicitud == args.IdSolicitud)
                .Include(x => x.Inversionista)
                .ThenInclude(x => x.TiposIdentificacion)
                .Include(x => x.TipoTasa)
                .Include(x => x.DetalleSolicitudes)
                .ThenInclude(x => x.Titulo)
                .ThenInclude(x => x.Pagador)
                .Include(x => x.DetalleSolicitudes)
                .ThenInclude(x => x.Titulo)
                .ThenInclude(x => x.Tercero)
                .Include(x => x.Tercero)
                .ThenInclude(x => x.TiposIdentificacion)
                .Include(x => x.EstadoSol)
                .FirstOrDefault();

            List<Identity.DetalleSolicitudes> detalleSolicitudes = new List<Identity.DetalleSolicitudes>();



            detalleSolicitudes.AddRange(
                                        args.Titulos
                                            .Select(titulo => solicitud.DetalleSolicitudes
                                                .FirstOrDefault(ds => ds.Titulo.NumeroTercero == titulo.NumeroTercero &&
                                                                      ds.Titulo.IdTitulo == titulo.IdTitulo &&
                                                                      ds.Titulo.TipoTitulo == titulo.TipoTitulo &&
                                                                      ds.Titulo.EstadoTitulo == (int)EstadosTitulos.gEnProceso))
                                            .Where(detalle => detalle != null)
                                    );

            var culture = new CultureInfo("es-CO", false);
            culture.NumberFormat.NumberDecimalDigits = 0;

            string archivo = string.Empty;
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(culture) { HasHeaderRecord = false, Delimiter = "|" }))
            {
                #region Get Data Endoso
                var data = detalleSolicitudes.Select(ds => new
                {

                    TipoIdentificacionEmisor = _ftpManager.GetSettings().TipoIdentificacion,
                    NumeroIdentificacionEmisor = _ftpManager.GetSettings().NitEmpresa,
                    DvDelEmisor = _ftpManager.GetSettings().DigitoVerificacion,
                    NombreEmisor = _ftpManager.GetSettings().RazonSocial,

                    TipoIdentificacionPagador = solicitud.Inversionista.TiposIdentificacion.TipoIdentificacionEBill,
                    NumeroIdentificacionPagador = solicitud.Inversionista.IdTercero,
                    DvDelPagador = solicitud.Inversionista.DigitoVerificacion,
                    NombrePagador = GenerarArchivoEndosoJob.RemoveSpecialCharacters(
                            GenerarArchivoEndosoJob.TruncateString(solicitud.Inversionista.RazonSocial, 50)),

                    Modalidad = "",
                    Producto = solicitud.Producto == 1 ? "FACTORING" : "CONFIRMING",
                    TipoTasaDescuento = solicitud.TipoTasa.TipoTasaInteresEBill,
                    Tasa = String.Format(culture, "{0:0#.000000}", solicitud.TasaInteresCliente),

                    DiasAdicionales = "0",
                    DiasDePago = "",

                    PorcentajeDeNegociacion = String.Format(culture, "{0:0#.0000}", solicitud.PorcNegociacion),
                    CorreoElectronico1 = "",
                    CorreoElectronico2 = "",
                    NumeroDeFactura = ds.Titulo.NumeroFactura,
                    PrefijoDeFactura = ds.Titulo.PrefijoFactura,

                    //WIGELO:FOR DO:ACTUALMENTE ESTA EN CERO
                    ValorAPagar = String.Format(culture, "{0:#}", ds.Titulo.VrNegociacion),
                    ValorFacturaConImpuestos = String.Format(culture, "{0:#}", ds.Titulo.VrNetoTitulo), // ds.Titulo.VrNetoTitulo,

                    MedioDePago = "",
                    //EndosoConOSinResponsabilidad = solicitud.TipoEndoso == 1 ? "371" : "372",
                    EndosoConOSinResponsabilidad = TiposEndoso[solicitud.TipoEndoso.Value],
                    Moneda = ds.Titulo.TipoMoneda,
                    NIT = ds.Titulo.Tercero.IdTercero


                }).ToList();

                #endregion
                csv.WriteRecords(data);
                writer.Flush();
                mem.Seek(0, SeekOrigin.Begin);

                var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == solicitud.IdOficina).FirstOrDefault();

                archivo = $"REEND_{oficina.SiglaOficina}_{solicitud.TipoDocumento}_{solicitud.IdOficina}_{solicitud.IdSolicitud}_{DateTime.Now:ddMMyyyyHHmmss}.txt";
              
                _ftpManager.UploadFile(mem, archivo, $"/{oficina.DirEbillFTP}/IN/ENDOSO");
            }

            var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(solicitud.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_Endosos);

          
            solicitud.NomArchivoF = archivo;
            solicitud.EstadoSolicitud = estadoSolicitud.Id;
            _solicitudesOperacionesRepositorio.Update(solicitud);

        }

      
    }

   
}
