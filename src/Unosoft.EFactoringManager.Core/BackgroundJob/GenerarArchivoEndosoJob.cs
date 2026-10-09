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
using System.Text.RegularExpressions;
using Unosoft.EFactoringManager.BackgroundJob.DTO;
using Unosoft.EFactoringManager.EstadosSolicitudes;
using Unosoft.EFactoringManager.FTP;
using Unosoft.EFactoringManager.IRepositorios;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.BackgroundJob
{
    public class GenerarArchivoEndosoJob : BackgroundJob<FileEndosoFTPJobArgs>, ITransientDependency
    {

        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly FTPManager _ftpManager;
        private readonly EstadosSolicitudesManager _estadosSolicitudesManager;
        private readonly IOficinasRepositorio _oficinaRepositorio;

        private readonly Dictionary<int, string> TiposEndoso;


        public GenerarArchivoEndosoJob(ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio,
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
            //var solicitud = _solicitudesOperacionesManager.ObtenerSolicitudOpePorId(args.TipoDocumento, args.IdOficina, args.IdSolicitud);

            var solicitud = _solicitudesOperacionesRepositorio
              .GetAll()
              .Where(a => a.TipoDocumento == args.TipoDocumento && a.IdOficina == args.IdOficina && a.IdSolicitud == args.IdSolicitud)
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
                                                                      //ds.Titulo.EstadoTitulo == (int)EstadosTitulos.gEndosable
                                                                      //WIGELO:14/02/2023 SE MODIFICA PARA PRODUCCIÓN
                                                                      ds.Titulo.EstadoTitulo == (int)EstadosTitulos.gEnProceso))
                                            .Where(detalle => detalle != null)
                                    );


            //foreach (var titulo in args.Titulos)
            //{
            //    var detalle = solicitud.DetalleSolicitudes.Where(ds => ds.Titulo.NumeroTercero == titulo.NumeroTercero &&
            //                                                           ds.Titulo.IdTitulo == titulo.IdTitulo &&
            //                                                           ds.Titulo.TipoTitulo == titulo.TipoTitulo &&
            //                                                           //ds.Titulo.EstadoTitulo== (int)EstadosTitulos.gEndosable
            //                                                           //WIGELO:14/02/2023 SE MODIFICA PARA PARA PRODUCCION
            //                                                           ds.Titulo.EstadoTitulo == (int)EstadosTitulos.gEnProceso
            //                                                     ).FirstOrDefault();


            //    if (detalle != null)
            //        detalleSolicitudes.Add(detalle);




            //}


            var culture = new CultureInfo("es-CO", false);
            culture.NumberFormat.NumberDecimalDigits = 0;

            string archivo = string.Empty;
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(culture) { HasHeaderRecord = false, Delimiter = "|", TrimOptions = TrimOptions.Trim }))
            {
                #region Get Data Endoso
                var data = detalleSolicitudes.Select(ds =>
                {
                    var (prefijoFactura, numeroFactura) = ObtenerPrefijoYNumero(ds.Titulo.IdTitulo);
                    return new
                    {

                        TipoIdentificacionEmisor = ds.Titulo.Tercero.TiposIdentificacion.TipoIdentificacionEBill,
                        NumeroIdentificacionEmisor = ds.Titulo.Tercero.IdTercero.Trim(),
                        DvDelEmisor = ds.Titulo.Tercero.DigitoVerificacion,
                        NombreEmisor = RemoveSpecialCharacters(TruncateString(ds.Titulo.Tercero.RazonSocial, 50)),

                        TipoIdentificacionPagador = ds.Titulo.Pagador.TiposIdentificacion.TipoIdentificacionEBill,
                        NumeroIdentificacionPagador = ds.Titulo.Pagador.IdTercero.Trim(),
                        DvDelPagador = ds.Titulo.Pagador.DigitoVerificacion,
                        NombrePagador = RemoveSpecialCharacters(TruncateString(ds.Titulo.Pagador.RazonSocial, 50)),

                        Modalidad = "",
                        Producto = solicitud.Producto == 1 ? "FACTORING" : "CONFIRMING",
                        TipoTasaDescuento = solicitud.TipoTasa.TipoTasaInteresEBill,



                        Tasa = String.Format(culture, "{0:0#.000000}", solicitud.TasaInteresCliente),

                        DiasAdicionales = "0",
                        DiasDePago = "",



                        PorcentajeDeNegociacion = String.Format(culture, "{0:0#.0000}", solicitud.PorcNegociacion),


                        CorreoElectronico1 = "",
                        CorreoElectronico2 = "",



                        NumeroDeFactura = ds.Titulo.NumeroFactura ?? numeroFactura,
                        PrefijoDeFactura = ds.Titulo.PrefijoFactura ?? prefijoFactura,

                        //WIGELO:FOR DO:ACTUALMENTE ESTA EN CERO
                        ValorAPagar = String.Format(culture, "{0:#}", ds.Titulo.VrNegociacion),

                        ValorFacturaConImpuestos = String.Format(culture, "{0:#}", ds.Titulo.VrNetoTitulo), // ds.Titulo.VrNetoTitulo,

                        MedioDePago = "",

                        // //WIGELO:FOR DO:ACTUALMENTE ESTA EN 1 que graba 371 o 372
                        //EndosoConOSinResponsabilidad = solicitud.TipoEndoso,
                        EndosoConOSinResponsabilidad = TiposEndoso[solicitud.TipoEndoso.Value], // == 1 ? "371" : solicitud.TipoEndoso == 2 ? "372" : "047",
                                                                                                //EndosoConOSinResponsabilidad = solicitud.TipoEndoso == 1 ? "371" : "372",

                        Moneda = ds.Titulo.TipoMoneda,





                    };
                }).ToList();

                #endregion
                csv.WriteRecords(data);
                writer.Flush();
                mem.Seek(0, SeekOrigin.Begin);

                var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == solicitud.IdOficina).FirstOrDefault();

                archivo = $"END_{oficina.SiglaOficina}_{solicitud.TipoDocumento}_{solicitud.IdOficina}_{solicitud.IdSolicitud}_{DateTime.Now:ddMMyyyyHHmmss}.txt";

                _ftpManager.UploadFile(mem, archivo, $"/{oficina.DirEbillFTP}/IN/ENDOSO");

            }

            var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(solicitud.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_Endosos);
            solicitud.NomArchivoF = archivo;
            solicitud.EstadoSolicitud = estadoSolicitud.Id;
            _solicitudesOperacionesRepositorio.Update(solicitud);


        }
        public static string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, "[\",]", "");
        }

        public static string TruncateString(string input, int length)
        {
            return input.Length > length ? input.Substring(0, length) : input;
        }

        static (string, string) ObtenerPrefijoYNumero(string numeroFactura)
        {
            // Usar una expresión regular para separar letras del número
            Match match = Regex.Match(numeroFactura, @"^([A-Za-z]*)(\d+)$");
            if (match.Success)
            {
                string prefijo = match.Groups[1].Value; // Grupo de letras
                string numero = match.Groups[2].Value;  // Grupo de números
                return (prefijo, numero);
            }
            else
            {
                string prefijo = ""; // Grupo de letras
                string numero = numeroFactura;  // Grupo de números
                return (prefijo, numero);
            }
        }

    }

}
