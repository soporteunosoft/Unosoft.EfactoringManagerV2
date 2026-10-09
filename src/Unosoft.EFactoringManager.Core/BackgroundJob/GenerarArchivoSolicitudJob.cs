using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
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
    public class GenerarArchivoSolicitudJob : BackgroundJob<FileFTPJobArgs>, ITransientDependency
    {
        private readonly ISolicitudesOperacionesRepositorio _solicitudesOperacionesRepositorio;
        private readonly FTPManager _ftpManager;
        private readonly EstadosSolicitudesManager _estadosSolicitudesManager;
        private readonly IOficinasRepositorio _oficinaRepositorio;



        public GenerarArchivoSolicitudJob(ISolicitudesOperacionesRepositorio solicitudesOperacionesRepositorio,
            EstadosSolicitudesManager estadosSolicitudesManager,           
            FTPManager ftpManager,
            IOficinasRepositorio oficinaRepositorio)
        {
            _solicitudesOperacionesRepositorio = solicitudesOperacionesRepositorio;          
            _estadosSolicitudesManager = estadosSolicitudesManager;
            _ftpManager = ftpManager;
            _oficinaRepositorio = oficinaRepositorio;
        }

        [UnitOfWork]
        public override void Execute(FileFTPJobArgs args)
        {
            var solicitud = _solicitudesOperacionesRepositorio
                .GetAll()
                .Where(a => a.TipoDocumento == args.TipoDocumento && 
                            a.IdOficina == args.IdOficina && 
                            a.IdSolicitud == args.IdSolicitud )
                            //WIGELO:14/02/2023 SE AJUSTA PARA LA PRUEBA EN PRODUCCION
                            //&&
                            //a.EstadoSol.NomEstadoSol == EstadosSolicitudesTipoDocumentoSCU.SCU_Inicial)
                            //a.EstadoSol.NomEstadoSol == EstadosSolicitudesTipoDocumentoSCU.SCU_Inicial)
                //.Include(X => X.EstadoSol)
                .Include(x => x.DetalleSolicitudes)
                .ThenInclude(x => x.Titulo)              
                .FirstOrDefault();            


            if (solicitud == null )
                return;

            string archivo = string.Empty;
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false }))  //(writer, CultureInfo.InvariantCulture))
            {

                var data = solicitud.DetalleSolicitudes.Select(x => new
                {
                    CufeTitulo = x.Titulo.CufeTitulo

                }).ToList();
                csv.WriteRecords(data);
                writer.Flush();
                mem.Seek(0, SeekOrigin.Begin);

                var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == solicitud.IdOficina).FirstOrDefault();

                archivo = $"CUFE_{oficina.SiglaOficina}_{solicitud.TipoDocumento}_{solicitud.IdOficina}_{solicitud.IdSolicitud}_{DateTime.Now:ddMMyyyyHHmmss}.csv";
                //_ftpManager.UploadFile(mem, archivo, $"/{_ftpManager.GetSettings().UploadDirectory}/IN/CUFES/DIRECTOS");
                _ftpManager.UploadFile(mem, archivo, $"/{oficina.DirEbillFTP}/IN/CUFES/DIRECTOS");
            }

            
            var estadoSolicitud = _estadosSolicitudesManager.ObtenerEstadosSolicitudes(solicitud.TipoDocumento, EstadosSolicitudesTipoDocumentoSCU.SCU_Cufes);
            solicitud.NomArchivoC = archivo;
            solicitud.EstadoSolicitud = estadoSolicitud.Id;
            _solicitudesOperacionesRepositorio.Update(solicitud);
        }
    }
}
