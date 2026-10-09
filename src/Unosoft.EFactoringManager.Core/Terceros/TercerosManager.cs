using Abp.Authorization;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Common.Extension;
using Unosoft.EFactoringManager.IRepositorios;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.Terceros
{

    public class ResumenTercero
    {
        public int Id { get; set; }
        public int? IdTerceroPadre { get; set; }
        public string RazonSocial { get; set; }
    }

    public class TercerosManager : IDomainService
    {

        #region Atributos
        private readonly ITerceroRepositorio _terceroRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;


        #endregion

        public TercerosManager(ITerceroRepositorio terceroRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _terceroRepositorio = terceroRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }


        public async Task<int> Insertar(Identity.Terceros tercero)
        {
            var result = await _terceroRepositorio.InsertAsync(tercero);
            return result.Id;
        }

        public async Task<bool> ActualizarTercero(Identity.Terceros tercero)
        {
            var _tercero = await _terceroRepositorio.GetAll().Where(a => a.Id == tercero.Id).FirstOrDefaultAsync();
            if (_tercero == null)
                return false;

            _tercero.RazonSocial = tercero.RazonSocial;
            //_tercero.RazonSocial = tercero.NomTercero+ tercero.ApeTercero;
            _tercero.DirOficina = tercero.DirOficina;
            _tercero.TelOficina = tercero.TelOficina;
            _tercero.TelDomicilio = tercero.TelDomicilio;
            _tercero.TelMovil = tercero.TelMovil;
            _tercero.Email = tercero.Email;

            await _terceroRepositorio.UpdateAsync(_tercero);
            return true;


        }

        public async Task<bool> ActualizarConsecutivoAnexoTercero(Identity.Terceros tecero)
        {
            var _tercero = await _terceroRepositorio.GetAll().Where(a => a.IdTercero == tecero.IdTercero).FirstOrDefaultAsync();
            if (_tercero == null)
                return false;


            _tercero.ConsecutivoAnexo = _tercero.ConsecutivoAnexo + 1;
            await _terceroRepositorio.UpdateAsync(_tercero);
            return true;


        }

        public async Task<List<Identity.Terceros>> ObtenerTerceros()
        {
            return await _terceroRepositorio.GetAll().ToListAsync();
        }

        [AbpAuthorize]
        public async Task<List<Identity.Terceros>> ObtenerOrdenantes(string idTerceroPadre = null)
        {
            return await _terceroRepositorio.GetAll().Include(t => t.TerceroPadre)
                                                     .Include(t => t.TiposIdentificacion)
                                                     .Include(t => t.TiposOrdenantes)
                                                     .Include(t => t.Ciudades)
                                                     .ThenInclude(t => t.Pais)
                                                     .Where(t => t.Firma &&
                                                      t.TerceroPadre.IdTercero == idTerceroPadre)
                                                     .ToListAsync();
        }

        [AbpAuthorize]
        public List<Identity.TercerosComerciantes> ObtenerTercerosComerciante(string idTercero, int tipoTecero)
        {

            return _terceroRepositorio.ObtenerTercerosComerciante(idTercero, tipoTecero);
        }

        #region Version Anterior ObtenerTercerosConFirma
        /*
        public Tuple<List<Identity.Terceros>, int> ObtenerTercerosConFirma(string idTercero = null,
                                                               bool cargarHermanos = false,
                                                               int tipoTercero = 1,
                                                               int skip = 0,
                                                               int limit = 0,
                                                               string sortField = "",
                                                               int sortOrder = -1)
        {

            var orderMap = new Dictionary<string, Expression<Func<Identity.Terceros, object>>>
            {
                [nameof(Identity.Terceros.IdTercero)] = x => x.IdTercero,
                [nameof(Identity.Terceros.RazonSocial)] = x => x.RazonSocial,
                [nameof(Identity.Terceros.Id)] = x => x.Id

            };

            IQueryable<Identity.Terceros> query;


            query = _terceroRepositorio.GetAll()
               .Include(t => t.TerceroPadre)
               .Include(t => t.TiposIdentificacion)
               .Include(t => t.TiposOrdenantes);

            // -------------------------------
            // Tipo 1: Ordenantes
            // -------------------------------
            if (tipoTercero == (int)TipoTercero.gOrdenante)
            {
                List<int> listTerceroPadre = null;

                if (cargarHermanos && !string.IsNullOrEmpty(idTercero))
                {
                    listTerceroPadre = query
                        .Where(x => x.IdTercero == idTercero && x.IdTerceroPadre.HasValue)
                        .Select(x => x.IdTerceroPadre.Value)
                        .ToList();
                }

                query = query
                    .Where(t =>
                        t.Activa &&
                        t.IdTerceroPadre != null &&
                        (idTercero == null || t.IdTercero == idTercero) &&
                        (listTerceroPadre == null || listTerceroPadre.Contains(t.IdTerceroPadre.Value))
                    );


            }
            else
            // -------------------------------
            // Tipo 3: Pagador
            // -------------------------------
            if (tipoTercero == (int)TipoTercero.gPagador)
            {
                query = query
                     .Where(t => t.Pagador)
                    ;


            }
            else
            {
                // -------------------------------
                // Otros: Empresas
                // -------------------------------
                List<int> listaIdsInternosPadres = new List<int>();
                List<string> listaIdTercerosPadres = new List<string>();

                if (!string.IsNullOrEmpty(idTercero))
                {
                    var tercerosPadres = query
                        .Where(x => x.Activa && x.IdTercero == idTercero && x.IdTerceroPadre.HasValue)
                        .Select(x => new
                        {
                            IdInternoPadre = x.IdTerceroPadre.Value,
                            IdTerceroPadre = x.TerceroPadre.IdTercero
                        })
                        .ToList();

                    listaIdsInternosPadres = tercerosPadres.Select(x => x.IdInternoPadre).ToList();
                    listaIdTercerosPadres = tercerosPadres.Select(x => x.IdTerceroPadre).ToList();
                }

                query = query
                    .Where(t =>
                        (idTercero == null || listaIdsInternosPadres.Contains(t.Id)) &&
                        t.IdTerceroPadre == null
                    );
                ;

                if (listaIdTercerosPadres != null && listaIdTercerosPadres.Count > 0)
                {
                    var extendidos = query
                        .Where(t =>
                            (
                                (listaIdTercerosPadres.Contains(t.IdComisionista) || listaIdTercerosPadres.Contains(t.IdCorredor)) &&
                                t.IdTerceroPadre == null &&
                                t.Cliente
                            )
                            ||
                            (tipoTercero == (int)TipoTercero.gComisionista && t.Pagador)
                        );

                    // Combinar los resultados de query y extendidos usando Union para mantener IQueryable
                    query = query.Union(extendidos);
                }
            }

            var total = query.Count();

            query = query.ApplyOrdering(
               sortField,
               sortOrder,
               orderMap,
               x => x.RazonSocial);

            // Paginación
            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return new Tuple<List<Identity.Terceros>, int>([.. query], total);

            //return baseResult
            //    .GroupBy(x => x.Id) // evitar duplicados
            //    .Select(g => g.First())
            //    .ToList();
        }
         */
        #endregion

        public async Task<(List<Identity.Terceros> Data, int Total)> ObtenerTercerosConFirma(
                                                                                    string idTercero = null,
                                                                                    bool cargarHermanos = false,
                                                                                    int tipoTercero = 1,
                                                                                    int skip = 0,
                                                                                    int limit = 0,
                                                                                    string sortField = "",
                                                                                    int sortOrder = -1,
                                                                                    string filter = "")
        {
            var orderMap = new Dictionary<string, Expression<Func<Identity.Terceros, object>>>
            {
                [nameof(Identity.Terceros.IdTercero)] = x => x.IdTercero,
                [nameof(Identity.Terceros.RazonSocial)] = x => x.RazonSocial,
                [nameof(Identity.Terceros.Id)] = x => x.Id
            };

            var query = BaseQuery();

            query = tipoTercero switch
            {
                (int)TipoTercero.gOrdenante => await FiltrarOrdenantes(query, idTercero, cargarHermanos),
                (int)TipoTercero.gPagador => FiltrarPagadores(query),
                _ => await FiltrarEmpresas(query, idTercero, tipoTercero)
            };

            if (!string.IsNullOrEmpty(filter?.Trim()))
            {
                query = query
                            .Where(x => x.RazonSocial.Contains(filter.Trim()) ||
                                        x.IdTercero.Contains(filter.Trim()));
            }

            var total = await query.CountAsync();

            query = query.ApplyOrdering(
                sortField,
                sortOrder,
                orderMap,
                x => x.RazonSocial);

            if (skip > 0)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return (await query.ToListAsync(), total);
        }

        private IQueryable<Identity.Terceros> BaseQuery()
        {
            return _terceroRepositorio.GetAll()
                .Include(t => t.TerceroPadre)
                .Include(t => t.TiposIdentificacion)
                .Include(t => t.TiposOrdenantes);
        }

        private async Task<IQueryable<Identity.Terceros>> FiltrarOrdenantes(
                                                                IQueryable<Identity.Terceros> query,
                                                                string idTercero,
                                                                bool cargarHermanos)
        {
            List<int> listTerceroPadre = null;

            if (cargarHermanos && !string.IsNullOrEmpty(idTercero))
            {
                listTerceroPadre = await query
                    .Where(x => x.IdTercero == idTercero && x.IdTerceroPadre.HasValue)
                    .Select(x => x.IdTerceroPadre.Value)
                    .ToListAsync();
            }

            return query.Where(t =>
                t.Activa &&
                t.IdTerceroPadre != null &&
                (idTercero == null || t.IdTercero == idTercero) &&
                (listTerceroPadre == null || listTerceroPadre.Contains(t.IdTerceroPadre.Value))
            );
        }

        private IQueryable<Identity.Terceros> FiltrarPagadores(
                                                                IQueryable<Identity.Terceros> query)
        {
            return query.Where(t => t.Pagador);
        }

        private async Task<IQueryable<Identity.Terceros>> FiltrarEmpresas(
    IQueryable<Identity.Terceros> query,
    string idTercero,
    int tipoTercero)
        {
            List<int> listaIdsInternosPadres = new();
            List<string> listaIdTercerosPadres = new();

            if (!string.IsNullOrEmpty(idTercero))
            {
                var tercerosPadres = await query
                    .Where(x => x.Activa && x.IdTercero == idTercero && x.IdTerceroPadre.HasValue)
                    .Select(x => new
                    {
                        IdInternoPadre = x.IdTerceroPadre.Value,
                        IdTerceroPadre = x.TerceroPadre.IdTercero
                    })
                    .ToListAsync();

                listaIdsInternosPadres = tercerosPadres.Select(x => x.IdInternoPadre).ToList();
                listaIdTercerosPadres = tercerosPadres.Select(x => x.IdTerceroPadre).ToList();
            }

            query = query.Where(t =>
                (idTercero == null || listaIdsInternosPadres.Contains(t.Id)) &&
                t.IdTerceroPadre == null
            );

            if (listaIdTercerosPadres.Count > 0)
            {
                var extendidos = query.Where(t =>
                    (
                        (listaIdTercerosPadres.Contains(t.IdComisionista) ||
                         listaIdTercerosPadres.Contains(t.IdCorredor)) &&
                        t.IdTerceroPadre == null &&
                        t.Cliente
                    )
                    ||
                    (tipoTercero == (int)TipoTercero.gComisionista && t.Pagador)
                );

                query = query.Union(extendidos);
            }

            return query;
        }


        public async Task<List<Identity.Terceros>> ObtenerEmpleadosConFirma()
        {
            return await _terceroRepositorio.GetAll().Include(t => t.TerceroPadre)
                                                     .Include(t => t.TiposIdentificacion)
                                                     .Include(t => t.TiposOrdenantes)
                                                     .Include(t => t.Ciudades)
                                                     .ThenInclude(t => t.Pais)
                                                     .Where(t => t.Firma && t.Empleado)
                                                     .ToListAsync();
        }

        //17/01/2022 OJO Revisar este metodo
        public async Task<List<Identity.Terceros>> ObtenerTerceroId(string IdTercero)
        {
            return await _terceroRepositorio.GetAll()
                                      .Include(x => x.TerceroPadre)
                                      .ThenInclude(x => x.TiposIdentificacion)
                                      .Include(x => x.TerceroPadre)
                                      .ThenInclude(x => x.Ciudades)
                                      .Include(x => x.Ciudades)
                                      .Include(x => x.TiposOrdenantes)
                                      .Include(x => x.TiposIdentificacion)
                                      .Where(a => a.IdTercero == IdTercero).ToListAsync();
        }

        public async Task<List<Identity.Terceros>> ObtenerTerceroPorId(List<string> IdTercero)
        {
            return await _terceroRepositorio.GetAll()
                                      .Include(x => x.TiposIdentificacion)
                                      .Include(x => x.Ciudades)
                                      .ThenInclude(c => c.Pais)
                                      .Where(a => IdTercero.Contains(a.IdTercero)).ToListAsync();
        }

        public async Task<List<Identity.Terceros>> ObtenerTerceroPorId(List<int> IdTercero)
        {
            return await _terceroRepositorio.GetAll()
                                      .Include(x => x.TiposIdentificacion)
                                      .Include(x => x.Ciudades)
                                      .ThenInclude(c => c.Pais)
                                      .Where(a => IdTercero.Contains(a.Id)).ToListAsync();
        }

        public async Task<List<Identity.Terceros>> ObtenerInversionistas()
        {
            return await _terceroRepositorio.GetAll()
                                      .Include(x => x.TiposIdentificacion)
                                      .Include(x => x.Ciudades)
                                      .ThenInclude(c => c.Pais)
                                      .Where(a => a.Inversionista).ToListAsync();
        }

        public async Task<Identity.Terceros> ObtenerTerceroPorId(string idTercero)
        {
            return await _terceroRepositorio.GetAll()
                .Include(x => x.TerceroPadre)
                .ThenInclude(x => x.TiposIdentificacion)
                .Include(x => x.TerceroPadre)
                .ThenInclude(x => x.Ciudades)
                .Include(x => x.Ciudades)
                .Include(x => x.TiposOrdenantes)
                .Include(x => x.TiposIdentificacion)
                .Where(x => x.IdTercero == idTercero).FirstOrDefaultAsync();
        }

        public async Task<Identity.Terceros> ObtenerTerceroPorId(int numeroTercero)
        {
            return await _terceroRepositorio.GetAll()
                .Include(x => x.TerceroPadre)
                .ThenInclude(x => x.TiposIdentificacion)
                .Include(x => x.TerceroPadre)
                .ThenInclude(x => x.Ciudades)
                .Include(x => x.Ciudades)
                .Include(x => x.TiposOrdenantes)
                .Include(x => x.TiposIdentificacion)
                .Where(x => x.Id == numeroTercero).FirstOrDefaultAsync();
        }


        public async Task<List<ResumenTercero>> ObtenerIdsTercero(string idTercero)
        {
            return await _terceroRepositorio.GetAll()
                .Where(x => x.IdTercero == idTercero)
                .Select(x => new ResumenTercero()
                {
                    Id = x.Id,
                    IdTerceroPadre = x.IdTerceroPadre,
                    RazonSocial = x.TerceroPadre.RazonSocial

                }).ToListAsync();
        }


        public async Task<Identity.Terceros> ObtenerTerceroPorId(string idTercero, string idTerceroPadre)
        {
            return await _terceroRepositorio.GetAll()
                .Include(x => x.TerceroPadre)
                .ThenInclude(x => x.TiposIdentificacion)
                .Include(x => x.TerceroPadre)
                .ThenInclude(x => x.Ciudades)
                .Include(x => x.Ciudades)
                .Include(x => x.TiposOrdenantes)
                .Include(x => x.TiposIdentificacion)
                .Where(x => x.IdTercero == idTercero && x.TerceroPadre.IdTercero == idTerceroPadre).FirstOrDefaultAsync();
        }

        public async Task<Identity.Terceros> ObtenerTerceroSession(string idTercero)
        {
            return await _terceroRepositorio.GetAll()
                                       .Include(x => x.TerceroPadre)
                                       .Where(x => x.IdTercero == idTercero)
                                       .Select(t => new Identity.Terceros()
                                       {
                                           Id = t.Id,
                                           IdTercero = t.IdTercero,
                                           Firma = t.Firma,
                                           IdTerceroPadre = t.IdTerceroPadre,
                                           TerceroPadre = new Identity.Terceros() { IdTercero = t.TerceroPadre.IdTercero }
                                       }

                                       ).FirstOrDefaultAsync();


        }

        public async Task<Identity.Terceros> ObtenerTerceroSessionV2(string idTercero)
        {
            var terceros = await _terceroRepositorio.GetAll()
                                       .Include(x => x.TerceroPadre)
                                       .Where(x => x.IdTercero == idTercero)
                                       .Select(t => new Identity.Terceros()
                                       {
                                           Id = t.Id,
                                           IdTercero = t.IdTercero,
                                           Firma = t.Firma,
                                           IdTerceroPadre = t.IdTerceroPadre,
                                           TerceroPadre = new Identity.Terceros() { IdTercero = t.TerceroPadre.IdTercero },
                                           Comisionista = t.Comisionista,
                                           Cliente = t.Cliente

                                       }

                                       ).ToListAsync();
            if (terceros == null || terceros.Count <= 0)
                return null;

            if (terceros.Count == 1)
                return terceros[0];

            var tercero = terceros[0];
            tercero.Firma = terceros.Any(x => x.Firma);
            tercero.Cliente = terceros.Any(x => x.Cliente);
            tercero.Comisionista = terceros.Any(x => x.Comisionista);

            return tercero;


        }

        public async Task<bool> GenerarCodigoTercero(string idTercero, int? tipoTercero)
        {


            Random rnd = new Random();
            int codigo = rnd.Next(0000, 9999);
            string emailTercero;
            int _tipoTercero;
            var _tercero = await _terceroRepositorio.GetAll().Where(a => a.IdTercero == idTercero).FirstOrDefaultAsync();
            if (_tercero == null)
            {
                return false;
            }
            else
            {
                _tercero.Codigo = codigo;
                _tercero.FecCodigo = DateTime.Now;
                emailTercero = _tercero.Email;
                await _terceroRepositorio.UpdateAsync(_tercero);
                if (tipoTercero == null)
                {
                    _tipoTercero = 0;
                }
                else _tipoTercero = tipoTercero.Value;

                if (_terceroRepositorio.InsertarNotificacionCodigoTercero(_tercero.IdTercero, _tipoTercero, codigo))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public async Task<bool> ValidarTercero(string idTercero)
        {
            var _tercero = await _terceroRepositorio.GetAll().Where(a => a.IdTercero == idTercero).FirstOrDefaultAsync();
            if (_tercero == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> VerificarCodigoTercero(string idTercero, int codigoTercero)
        {
            var _tercero = await _terceroRepositorio.GetAll().Where(a => a.IdTercero == idTercero).FirstOrDefaultAsync();
            if (_tercero == null)
            {
                return false;
            }
            else
            {
                var days = (int)DateTime.Now.Subtract((DateTime)_tercero.FecCodigo).TotalDays;
                var hours = (int)DateTime.Now.Subtract((DateTime)_tercero.FecCodigo).TotalHours;
                var minutes = (int)DateTime.Now.Subtract((DateTime)_tercero.FecCodigo).TotalMinutes;
                if (days == 0 && hours == 0 && minutes <= 15 && _tercero.Codigo == codigoTercero)
                {
                    return true;
                }
                else return false;
            }
        }

        public async Task<(List<Identity.Terceros> Data, int Total)> ObtenerTercerosUsuarios(bool esUsuario,
                                                                bool mustHaveFather = false,
                                                               int skip = 0,
                                                                 int limit = 20,
                                                                 string sortField = "",
                                                                 int sortOrder = 1,
                                                                 string filter = "")
        {

            var orderMap = new Dictionary<string, Expression<Func<Identity.Terceros, object>>>
            {
                [nameof(Identity.Terceros.IdTercero)] = x => x.IdTercero,
                [nameof(Identity.Terceros.RazonSocial)] = x => x.RazonSocial,
                [nameof(Identity.Terceros.TerceroPadre) + "." + nameof(Identity.Terceros.RazonSocial)] = x => x.TerceroPadre.RazonSocial,
                [nameof(Identity.Terceros.Id)] = x => x.Id
            };

            var queryAnonima =
                from t in _terceroRepositorio.GetAll()
                                                           .Include(x => x.TerceroPadre)
                                                            .ThenInclude(x => x.TiposIdentificacion)
                                                            .Include(x => x.TerceroPadre)
                                                            .ThenInclude(x => x.Ciudades)
                                                            .Include(x => x.Ciudades)
                                                            .Include(x => x.TiposOrdenantes)
                                                            .Include(x => x.TiposIdentificacion)


                        join u in _usuarioRepositorio.GetAll()
                          on t.IdTercero equals u.UserName into usuarios
                        from users in usuarios.DefaultIfEmpty()
                        select new
                        {
                            Tercero = t,
                            TieneUsuario = users != null
                        };

            queryAnonima = queryAnonima.Where(x => x.TieneUsuario == esUsuario);

            if (mustHaveFather)
            {
                queryAnonima = queryAnonima.Where(x => x.Tercero.IdTerceroPadre >0);
            }

            if (!string.IsNullOrEmpty(filter?.Trim()))
            {
                var trimmed = filter.Trim();
                queryAnonima = queryAnonima
                            .Where(x => x.Tercero.RazonSocial.Contains(trimmed) ||
                                        x.Tercero.IdTercero.Contains(trimmed));
            }

            // proyectar a Identity.Terceros para que ApplyOrdering funcione con orderMap
            var query = queryAnonima.Select(x => x.Tercero);

            var total = await query.CountAsync();

            query = query.ApplyOrdering(
                sortField,
                sortOrder,
                orderMap,
                x => x.RazonSocial);

            if (skip > 0)
                query = query.Skip(skip);

            if (limit > 0)
                query = query.Take(limit);

            return (await query.ToListAsync(), total);
        }

        public Identity.AbpUsers ObtUserId(string idTercero)
        {
            var idUser = _terceroRepositorio.ObtIdUsuario(idTercero);
            return idUser;
        }


        public async Task Actualizar(Identity.Terceros tercero)
        {
            await _terceroRepositorio.UpdateAsync(tercero);

        }
    }



}


