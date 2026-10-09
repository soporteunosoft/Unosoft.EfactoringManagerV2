using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Unosoft.EfactoringManager.EBillServices;
using Unosoft.EfactoringManager.EBillServices.DTO;
using Unosoft.EFactoringManager.DetalleSolicitudes;
using Unosoft.EFactoringManager.EstadosSolicitudes;
using Unosoft.EFactoringManager.FTP;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Titulos;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.BackgroundJob
{



    public class ValidarArchivosFtpWroker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly FTPManager _ftpManager;
        private readonly FileEBillManager _archivoTipoCManager;
        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly TitulosManager _titulosManager;
        private readonly DetalleSolicitudesManager _detalleSolicitudesManager;
        private readonly EstadosSolicitudesManager _estadosSolicitudesManager;
        private readonly IOficinasRepositorio _oficinaRepositorio;


        public ValidarArchivosFtpWroker(AbpTimer timer, FTPManager ftpManager,
                                        FileEBillManager archivoTipoCManager,
                                        ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio,
                                        TitulosManager titulosManager,
                                        DetalleSolicitudesManager detalleSolicitudesManager,
                                        EstadosSolicitudesManager estadosSolicitudesManager,
                                        IOficinasRepositorio oficinaRepositorio
                                        )
        : base(timer)
        {

            Timer.Period = 10000;
            _ftpManager = ftpManager;
            _archivoTipoCManager = archivoTipoCManager;
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;
            _titulosManager = titulosManager;
            _detalleSolicitudesManager = detalleSolicitudesManager;
            _estadosSolicitudesManager = estadosSolicitudesManager;
            _oficinaRepositorio = oficinaRepositorio;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            Logger.Info("Procesando Archivos FTP EBILL");

            var oficinas = _oficinaRepositorio.GetAll().ToList();



            foreach (var oficina in oficinas)
            {
                try
                {
                    var ftpFiles = _ftpManager.DownloadFile($"/{oficina.DirEbillFTP}/OUT/CUFES");

                    try
                    {

                        if (ftpFiles != null && ftpFiles.Count > 0)
                        {
                            ProcesarArchivoTipoC(ftpFiles, TipoArchivosTipoC.ENDOSABLE, oficina.NitOficina);
                            ProcesarArchivoTipoC(ftpFiles, TipoArchivosTipoC.NOENDOSABLE, oficina.NitOficina);
                            ProcesarArchivosFAL(ftpFiles, oficina.IdOficina);
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.Error(ex.Message);
                    }
                    ftpFiles = _ftpManager.DownloadFile($"/{oficina.DirEbillFTP}/OUT/ENDOSO");

                    if (ftpFiles == null || ftpFiles.Count == 0)
                        continue;
                    Logger.Info("Procesando OUT- ENDOSO: " + ftpFiles.Count);
                    ProcesarArchivoTipoF(ftpFiles);
                    ProcesarArchivosFALEndoso(ftpFiles, oficina.IdOficina);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Logger.Error(ex.Message);
                    //throw;
                }
            }

        }

        /// <summary>
        /// Procesamiento del archivo de respuesta de CUFES
        /// </summary>
        /// <param name="ftpFiles"></param>
        /// <param name="tipoArchivo"></param>
        private void ProcesarArchivoTipoC(List<FtpFile> ftpFiles, string tipoArchivo, string selectNitOficina)
        {

            var endosables = ftpFiles.Where(x => x.FileName.ToLower().StartsWith(tipoArchivo));

            foreach (var file in endosables)
            {
                try
                {


                    Identity.SolicitudesOperaciones solicitud = null;
                    var facturas = _archivoTipoCManager.ProcesarArchivo<ArchivoTipoC>(file.FileStream);

                    foreach (var factura in facturas)
                    {

                        if (solicitud == null)
                            solicitud = GetSolicitud(factura.NombreArchivoProcesado);

                        if (solicitud == null)
                            continue;


                        var detalleSolicitud = solicitud.DetalleSolicitudes
                                      .Where(x => x.IdTitulo == (factura.Prefijo.Replace("-", "") + factura.NumeroFactura.Replace("-", "")))
                                        .Select(x => x).FirstOrDefault();
                        
                        if (detalleSolicitud == null)
                            continue;


                        var titulo = _titulosManager.ObtenerTituloPorId(detalleSolicitud.NumeroTercero,
                                                                                    detalleSolicitud.IdTitulo,
                                                                                    detalleSolicitud.TipoTitulo).GetAwaiter().GetResult();
                       

                        if (titulo == null) 
                            continue;

                 
                        //Actualización estado de titulo
                        /*
                         * facturas
                         */
                        titulo.EstadoTitulo = tipoArchivo == TipoArchivosTipoC.ENDOSABLE ? (int)EFactoringManagerConsts.EstadosTitulos.gEndosable : (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosable;

                        if (tipoArchivo == TipoArchivosTipoC.NOENDOSABLE && factura.EndosadoA == selectNitOficina) //_ftpManager.GetSettings().NitEmpresa)
                        {
                            titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gReendosable;
                        }

                        else if (tipoArchivo == TipoArchivosTipoC.NOENDOSABLE && solicitud.TipoEndoso == EFactoringManagerConsts.TipoEndoso.CesionOrdinaria) //Endoso en cesion ordinaria
                        {
                            titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gEndosable;
                        }


                        //C-VALIDACIONES EBILL

                        titulo.Vencida = factura.Vencida == "True";
                        titulo.DescripcionEstadoCUFE = factura.DescripcionEstadoCUFE;
                        titulo.FechaEntregaProductoServicio = factura.FechaEntregaProductoServicio != "";

                        titulo.AcuseRecibo = factura.AcuseRecibo == "True";
                        titulo.FecEntregaValidoNegociar = factura.FechaEntregaProductoServicioValidoNegociar == "True";
                        titulo.AceptacionTacita = factura.AceptacionTacita == "True";
                        titulo.AceptacionExpresa = factura.AceptacionExpresa == "True";
                        titulo.InscritaFEcomoTV = factura.InscritaFEcomoTV == "True";
                        titulo.Endoso = factura.Endoso == "True";
                        titulo.EndosadoA = factura.EndosadoA;
                        titulo.NumeroFactura = factura.NumeroFactura;
                        titulo.PrefijoFactura = factura.Prefijo;

                        _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();
                        _detalleSolicitudesManager.Actualizar(detalleSolicitud, (int)titulo.EstadoTitulo, 0).GetAwaiter().GetResult();


                    }
                    _ftpManager.UpdateFile(file);

                    //Actualizar la solicitud

                }
                catch
                {
                    try
                    {

                        var solicitud = GetSolicitud(file.FileName, true);

                        if (solicitud != null)
                        {
                            foreach (var detalle in solicitud.DetalleSolicitudes)
                            {
                                var titulo = detalle.Titulo;

                                if (titulo == null)
                                    continue;


                                //titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gError;
                                titulo.CodigoError = "ERRFOR";

                                _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();
                                _detalleSolicitudesManager.Actualizar(detalle, (int)titulo.EstadoTitulo, 0).GetAwaiter().GetResult();
                            }

                            var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(solicitud.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_ErrCufes);
                            solicitud.NomArchivoC = file.FileName;
                            solicitud.EstadoSolicitud = estadoSolicitud.Id;
                            _solicitudesOperacionesRepositorio.Update(solicitud);
                        }
                        _ftpManager.UpdateFile(file);
                    }
                    catch(Exception ex)
                    {

                        Logger.Error(ex.Message);
                    }
                }
            }

        }

        private void ProcesarArchivoTipoF(List<FtpFile> ftpFiles)
        {

            var endosables = ftpFiles.Where(x => !x.FileName.ToUpper().StartsWith("FAL"));

            foreach (var file in endosables)
            {
                try
                {



                    if (Path.GetExtension(file.FileName).ToLower() != ".zip")
                        continue;


                    Identity.SolicitudesOperaciones solicitud = null;
                    var facturas = ProcesarArchivoFromZip<ArchivoTipoF>(file.FileStream);

                    if (facturas == null)
                        continue;

                    facturas = facturas.GroupBy(x => x.NumeroFactura)
                                        .Select(g => g.First())
                                        .ToList();

                    foreach (var factura in facturas)
                    {
                        if (solicitud == null)
                            solicitud = GetSolicitud(file.FileName);

                        if (solicitud == null)
                            continue;


                        var detalleSolicitud = solicitud.DetalleSolicitudes
                                          .Where(x => x.IdTitulo == factura.NumeroFactura.Replace("-", ""))
                                            .Select(x => x).FirstOrDefault();

                        if (detalleSolicitud == null)
                            continue;

                        var titulo = _titulosManager.ObtenerTituloPorId(detalleSolicitud.NumeroTercero,
                                                                                    detalleSolicitud.IdTitulo,
                                                                                    detalleSolicitud.TipoTitulo).GetAwaiter().GetResult();


                        if (titulo == null)
                            continue;



                        if (factura.EstadoEndoso == "Exitoso")
                            titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gEndosada;
                        else
                            titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gNoEndosada;


                        titulo.CodigoError = factura.CodigoError.Length > 500 ? factura.CodigoError.Substring(0, 500) : factura.CodigoError;
                        titulo.MensajeProcesamiento = factura.Mensaje;
                        titulo.Vencida = factura.FacturaVencida.ToLower() == "verdadero";
                        titulo.CudeTitulo = factura.CUDE;

                        #region [Codigo Comentado]
                        //titulo.EstadoDian = factura.EstadoDian;
                        //titulo.Vencida = factura.Vencida;
                        //titulo.DescripcionEstadoCUFE = factura.DescripcionEstadoCUFE;
                        //titulo.AcuseRecibo = factura.AcuseRecibo;
                        //titulo.AceptacionTacita = factura.AceptacionTacita;
                        //titulo.AceptacionExpresa = factura.AceptacionExpresa;
                        //titulo.Endoso = factura.Endoso;
                        #endregion


                        _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();
                        _detalleSolicitudesManager.Actualizar(detalleSolicitud, (int)titulo.EstadoTitulo, 0).GetAwaiter().GetResult();
                    }
                    _ftpManager.UpdateFile(file);
                    //Actualizar la solicitud
                }
                catch(Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
        }

        private void ProcesarArchivosFAL(List<FtpFile> ftpFiles, int selectOficina, bool esEndoso = false)
        {
            var errores = ftpFiles.Where(x => x.FileName.ToUpper().StartsWith("FAL"));

            foreach (var file in errores)
            {
                try
                {


                    var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == selectOficina).FirstOrDefault();
                    string rutaFichero = oficina.DirRaizDocumentos + "\\EBILL\\FAL\\";

                    var solicitud = GetSolicitud(file.FileName, true);

                    if (solicitud == null)
                        continue;

                    if (!Directory.Exists(rutaFichero))
                    {
                        Directory.CreateDirectory(rutaFichero);
                    }

                    rutaFichero += $@"\{file.FileName}";

                    SaveFileStream(rutaFichero, file.FileStream);

                    var ListaErrores = _archivoTipoCManager.ProcesarArchivo<ErrorCUFE>(file.FileStream);

                    foreach (var error in ListaErrores)
                    {
                       

                        var titulo = _titulosManager.ObtenerTituloPorCufe(error.CUFE).GetAwaiter().GetResult();

                        if (titulo == null)
                            continue;


                        titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gError;
                        titulo.CodigoError = error.CodError;

                        _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();
                      

                    }


                    var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(solicitud.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_ErrCufes);
                    solicitud.NomArchivoC = rutaFichero;
                    solicitud.EstadoSolicitud = estadoSolicitud.Id;
                    _solicitudesOperacionesRepositorio.Update(solicitud);
                    _ftpManager.UpdateFile(file);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
        }

        private void ProcesarArchivosFALEndoso(List<FtpFile> ftpFiles, int selectOficina)
        {
            var errores = ftpFiles.Where(x => x.FileName.ToUpper().StartsWith("FAL"));

            foreach (var file in errores)
            {
                try
                {

                    var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == selectOficina).FirstOrDefault();
                    string rutaFichero = oficina.DirRaizDocumentos + "\\EBILL\\FALENDOSO\\";

                    var solicitud = GetSolicitud(file.FileName, true);

                    if (solicitud == null)
                        continue;

                    if (!Directory.Exists(rutaFichero))
                    {
                        Directory.CreateDirectory(rutaFichero);
                    }

                    rutaFichero += $@"\{file.FileName}";

                    SaveFileStream(rutaFichero, file.FileStream);

                    var ListaErrores = _archivoTipoCManager.ProcesarArchivo<ErrorEndoso>(file.FileStream);

                    foreach (var errorEndoso in ListaErrores)
                    {

                        if (string.IsNullOrEmpty(errorEndoso.NumeroFactura))
                        {
                            solicitud.DetalleSolicitudes.ToList().ForEach(x => x.Titulo.CodigoError = errorEndoso.CodError);
                            break;
                        }

                        var detalleSolicitud = solicitud.DetalleSolicitudes
                                          .Where(x => x.IdTitulo == $"{errorEndoso.Prefijo}{errorEndoso.NumeroFactura}")
                                          .Select(x => x).FirstOrDefault();


                        if (detalleSolicitud == null)
                            continue;

                        var titulo = _titulosManager.ObtenerTituloPorId(detalleSolicitud.NumeroTercero,
                                                                           detalleSolicitud.IdTitulo,
                                                                           detalleSolicitud.TipoTitulo).GetAwaiter().GetResult();


                        if (titulo == null)
                            continue;



                        titulo.EstadoTitulo = (int)EFactoringManagerConsts.EstadosTitulos.gError;
                        titulo.CodigoError = errorEndoso.CodError;

                    
                        _titulosManager.ActualizarTitulo(titulo).GetAwaiter().GetResult();
                        _detalleSolicitudesManager.Actualizar(detalleSolicitud, (int)titulo.EstadoTitulo, 0).GetAwaiter().GetResult();

                    }


                    var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(solicitud.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_ErrEndosos);
                    solicitud.NomArchivoC = rutaFichero;
                    solicitud.EstadoSolicitud = estadoSolicitud.Id;
                    _solicitudesOperacionesRepositorio.Update(solicitud);
                    _ftpManager.UpdateFile(file);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex.Message);
                }

            }
        }


        private Identity.SolicitudesOperaciones GetSolicitud(string fileName, bool archivoError = false)
        {
            //fileName = END_FINAMCO_SCU_1_2_27012023152811.zip.rev;

            var fileParts = fileName.Replace(" ", "").Split('_');

            var index = archivoError ? 1 : 0;

            if (fileParts.Length < 5 + index)
                return null;

            var tipoDocumento = fileParts[2 + index];

            int.TryParse(fileParts[3 + index], out int idOficina);
            int.TryParse(fileParts[4 + index], out int idSolicitud);


            var query = _solicitudesOperacionesRepositorio
                           .GetAll()
                           .Where(x => x.TipoDocumento == tipoDocumento &&
                                      x.IdOficina == idOficina &&
                                      x.IdSolicitud == idSolicitud);

            //Se elimina la obtención del titulo x error que genera el framework al realizar la consulta
            return query
                   .Include(x => x.DetalleSolicitudes)
                   //.ThenInclude(x => x.Titulo)
                   .Include(x => x.Tercero)
                   .FirstOrDefault();

        }


        private List<T> ProcesarArchivoFromZip<T>(Stream zipFile) where T : class
        {

            byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }

            ZipArchive archive = new ZipArchive(zipFile);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {

                if (Path.GetExtension(entry.Name).ToLower() == ".txt")
                    return _archivoTipoCManager.ProcesarArchivo<T>(new MemoryStream(ReadFully(entry.Open())));

            }

            return null;

        }

        private void SaveFileStream(string path, Stream stream)
        {
            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Close();
            fileStream.Dispose();

            var lines = File.ReadAllLines(path);

            string[] content = lines;

            for (int i = 1; i < lines.Length; i++)
            {
                var c = new[] { lines[i]
                .Split(',').Take(3)
                .ToArray()
                .Aggregate((current, next) => $"{current},{next}") };

                content[i] = c[0];
            }

            File.WriteAllLines(path, content.Skip(1).ToArray());
            //File.WriteAllLines(path, c);
        }
    }
}
