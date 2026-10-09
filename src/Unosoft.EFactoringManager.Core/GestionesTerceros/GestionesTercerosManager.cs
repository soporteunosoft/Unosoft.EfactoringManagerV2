using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.IRepositorios;
using Unosoft.EFactoringManager.Authorization.Users;
using UnoSoft.EFactoringManager.Services;
using System.IO;

namespace Unosoft.EFactoringManager.GestionesTerceros
{
    public class GestionesTercerosManager : IDomainService
    {
        #region Atributos
        private readonly IGestionesTercerosRepositorio _gestionesTercerosRepositorio;
        private readonly IOficinasRepositorio _oficinaRepositorio;
        private readonly ITerceroRepositorio _terceroRepositorio;
        private readonly IDocumentosGestionRepositorio _documentosGestionRepositorio;
        private readonly UserManager _userManager;
        #endregion

        public GestionesTercerosManager(IGestionesTercerosRepositorio gestionesTercerosRepositorio,
                                UserManager userManager,
                IOficinasRepositorio oficinaRepositorio,
                ITerceroRepositorio terceroRepositorio,
                IDocumentosGestionRepositorio documentosGestionRepositorio)
        {
            _gestionesTercerosRepositorio = gestionesTercerosRepositorio;
            _userManager = userManager;
            _oficinaRepositorio = oficinaRepositorio;
            _terceroRepositorio = terceroRepositorio;
            _documentosGestionRepositorio = documentosGestionRepositorio;
        }

        public async Task<List<Identity.GestionesTerceros>> ObtenerGestionesTerceros(int idGestion, int oficina, int? numTercero, string idTercero, int tipoTercero = 1)
        {
            var ordenantes = await _terceroRepositorio.GetAll()
                .Where(x => x.IdTercero == idTercero && x.IdTerceroPadre != null)
                .Select(x => new
                {
                    IdTerceroPadre = x.IdTerceroPadre.Value,
                    IdTercero = x.TerceroPadre.IdTercero
                })
                .ToListAsync();

            var listTerceroPadre = ordenantes.Select(x => x.IdTercero).ToList();

            return await _gestionesTercerosRepositorio.GetAll()
                .Where(x => (x.Id == idGestion || idGestion == 0) && x.IdOficina == oficina && (x.NumeroTercero == numTercero || numTercero == null || numTercero == 0) && (listTerceroPadre.Contains(x.Tercero.IdComisionista) || tipoTercero == 1) && x.Activa)
                .OrderByDescending(o => o.Id)
                .Include(x => x.Tercero)
                .Include(x => x.DocumentosGestion)
                .ToListAsync();
        }

        public async Task<Identity.GestionesTerceros> ObtenerGestionesTercerosPorId(int? idGestion, int oficina)
        {
            return await _gestionesTercerosRepositorio.GetAll()
                .Where(a => a.Id == idGestion && a.IdOficina == oficina)
                .Include(x => x.Tercero)
                .Include(x => x.DocumentosGestion)
                .FirstOrDefaultAsync();
        }

        public async Task<Identity.GestionesTerceros> Insertar(Identity.GestionesTerceros gestionesTerceros, long idUsuario)
        {
            User user = await _userManager.GetUserByIdAsync(idUsuario);
            gestionesTerceros.IdUsrMod = user.UserName;

            _gestionesTercerosRepositorio.InsertAndGetId(gestionesTerceros);

            return gestionesTerceros;
        }

        public async Task<bool> ActualizarGestiones(Identity.GestionesTerceros gestionesTerceros, long idUsuario)
        {
            User user = await _userManager.GetUserByIdAsync(idUsuario);
            gestionesTerceros.IdUsrMod = user.UserName;

            _gestionesTercerosRepositorio.Update(gestionesTerceros);

            return true;
        }

        public bool SubirDocumento(int idOficina, int idGestion, int numTercero, int? idDocumento, byte[] file, string fileName)
        {
            var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == idOficina).FirstOrDefault();
            var tercero = _terceroRepositorio.GetbyId(numTercero);

            if (oficina == null || tercero == null)
                return false;

            //fileName = fileName + DateTime.Now;

            string rutaFichero = oficina.DirRaizDocumentos + "\\DOCUMENTOS_CLIENTES_PLATAFORMA\\" + tercero.NomTercero.Replace(" ", "_") + "\\Documentos_Gestion";
            //Identity.DocumentosGestion documentoGestion = new Identity.DocumentosGestion();
            if (!(Directory.Exists(rutaFichero)))
            {
                Directory.CreateDirectory(rutaFichero);
            }

            System.IO.File.WriteAllBytes(rutaFichero + "\\" + fileName, file);

            if(idDocumento!=0 && idDocumento!=null)
            {
                var documento = _documentosGestionRepositorio.GetAll().Where(a => a.Id == idDocumento).FirstOrDefault();

                documento.DirDocumento = rutaFichero + "\\" + fileName;
                
                _documentosGestionRepositorio.Update(documento);
            }
            else
            {
                _documentosGestionRepositorio.InsertAndGetId(new Identity.DocumentosGestion()
                {
                    IdOficina = idOficina,
                    IdGestion = idGestion,
                    DirDocumento = rutaFichero + "\\" + fileName
                });
            }

            return true;
        }

        public bool EliminarDocumento(int idOficina, int idGestion, int idDocumento)
        {
            var oficina = _oficinaRepositorio.GetAll().Where(a => a.IdOficina == idOficina).FirstOrDefault();

            _documentosGestionRepositorio.Delete(new Identity.DocumentosGestion()
            {
                Id = idDocumento,
                IdOficina = idOficina,
                IdGestion = idGestion
            });

            return true;
        }

        public async Task<List<Identity.DocumentosGestion>> ObtDocumentosFromGestion(int? idGestion, int idOficina, int numeroTercero)
        {
            return await _documentosGestionRepositorio.GetAll()
                                                 .Where(a => a.IdGestion == idGestion && a.IdOficina == idOficina)
                                                 .ToListAsync();
        }
    }
}