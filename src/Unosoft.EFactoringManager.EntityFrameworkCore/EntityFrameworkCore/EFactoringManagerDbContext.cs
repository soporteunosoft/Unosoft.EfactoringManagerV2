using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using Unosoft.EFactoringManager.Authorization.Roles;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.MultiTenancy;


namespace Unosoft.EFactoringManager.EntityFrameworkCore
{
    public class EFactoringManagerDbContext : AbpZeroDbContext<Tenant, Role, User, EFactoringManagerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Identity.Menu> Menus { get; set; }
        public DbSet<Identity.Terceros> Terceros { get; set; }
        public DbSet<Identity.Titulos> Titulos { get; set; }
        public DbSet<Identity.TiposTitulo> TiposTitulo { get; set; }
        public DbSet<Identity.Cupos> Cupos { get; set; }
        public DbSet<Identity.DetalleSolicitudes> DetalleSolicitudes { get; set; }
        public DbSet<Identity.ModalidadesOperacion> ModalidadesOperacion { get; set; }
        public DbSet<Identity.SolicitudesOperaciones> SolicitudesOperaciones { get; set; }
        public DbSet<Identity.TiposOperaciones> TiposOperaciones { get; set; }
        public DbSet<Identity.TasasInteres> TasasInteres { get; set; }
        public DbSet<Identity.TiposTasaInteres> TiposTasaInteres { get; set; }
        public DbSet<Identity.ImpDocumentosOperativos> ImpDocumentosOperativos { get; set; }
        public DbSet<Identity.ConsecutivosDocumentos> ConsecutivosDocumentos { get; set; }
        public DbSet<Identity.CNSProyeccionDetalleSolicitudes> CNSProyeccionDetalleSolicitudes { get; set; }
        public DbSet<Identity.PagadoresTitulosSol> PagadoresTitulosSol { get; set; }
        public DbSet<Identity.Vinculaciones> Vinculaciones { get; set; }
        public DbSet<Identity.DetalleVinculaciones> DetalleVinculaciones { get; set; }
        public DbSet<Identity.ObtenerTercerosUsuarios> ObtenerTercerosUsuarios { get; set; }
        public DbSet<Identity.Ciudades> Ciudades { get; set; }
        public DbSet<Identity.Departamentos> Departamentos { get; set; }
        public DbSet<Identity.Paises> Paises { get; set; }
        public DbSet<Identity.DocumentosJuridicos> DocumentosJuridicos { get; set; }
        public DbSet<Identity.SeguridadPreguntasTerceros> SeguridadPreguntasTerceros { get; set; }
        public DbSet<Identity.TiposDocumentos> TiposDocumentos { get; set; }
        public DbSet<Identity.Documentos> Documentos { get; set; }
        public DbSet<Identity.DetalleDocumentos> DetalleDocumentos { get; set; }
        public DbSet<Identity.DocumentosGestion> DocumentosGestion { get; set; }
        public DbSet<Identity.SeguridadRespuestasTerceros> SeguridadRespuestasTerceros { get; set; }

        public DbSet<Identity.Oficinas> Oficinas { get; set; }
        public DbSet<Identity.AbpUsers> AbpUsers { get; set; }
        public DbSet<Identity.Reportes> Reportes { get; set; }
        public DbSet<Identity.EstadosTitulos> EstadosTitulos { get; set; }
        public DbSet<Identity.ValidarOperacion> ValidarOperaciones { get; set; }

        public EFactoringManagerDbContext(DbContextOptions<EFactoringManagerDbContext> options)
            : base(options)
        {
            //base.OnModelCreating( );
        }

        public DbSet<Identity.TiposEventosRadian> TiposEventosRadian { get; set; }

        public DbSet<Identity.TiposOperacionesRadian> TiposOperacionesRadian { get; set; }
        public DbSet<Identity.TiposOperacionesRadian> ValidarEventosRadian { get; set; }

        public DbSet<Identity.TiposNotasRadian> TiposNotasRadian { get; set; }

        public DbSet<Identity.TiposIdentificacion> TiposIdentificacion { get; set; }

        public DbSet<Identity.TiposIdentificacion> TiposContribuyente { get; set; }
        public DbSet<Identity.TiposRolesRadian> TiposRolesRadian { get; set; }
        public DbSet<Identity.RolesEventosRadian> RolesEventosRadian { get; set; }

        public DbSet<Identity.NotasEventosRadian> NotasEventosRadian { get; set; }
        public DbSet<Identity.OrdenantesTiposCertificados> OrdenantesTiposCertificados { get; set; }

        public DbSet<Identity.FirmasDetalleDocumentos> FirmasDetalleDocumentos { get; set; }

        public DbSet<Identity.TiposFirmas> TiposFirmas { get; set; }

        public DbSet<Identity.RespuestaConsultaReporte> RespuestaConsultaReporte { get; set; }

        public DbSet<Identity.GestionesTerceros> GestionesTerceros { get; set; }

        public DbSet<Identity.ConsolidadoOperaciones> ConsolidadoOperaciones { get; set; }

        public DbSet<Identity.HistorialCuposPagadores> HistorialCuposPagadores { get; set; }

        public DbSet<Identity.ResumenRecaudos> ResumenRecaudos { get; set; }

        public DbSet<Identity.DetalleRecaudos> DetalleRecaudos { get; set; }
        public DbSet<Identity.ProcesoEndosoBilly> ProcesoEndosoBilly { get; set; }
        public DbSet<Identity.TercerosComerciantes> TercerosComerciantes { get; set; }


        public DbSet<Identity.ErroresEbill> ErroresEbill { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Identity.DetalleDocumentos>()
                        .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Identity.SolicitudesOperaciones>()
                       .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Identity.Terceros>()
                       .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<User>()
            .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Identity.AbpUsers>()
                       .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Identity.DetalleDocumentos>()
                       .ToTable(tb => tb.UseSqlOutputClause(false));

            modelBuilder.Entity<Identity.SolicitudesOperaciones>()
                       .ToTable(tb => tb.UseSqlOutputClause(false));


            modelBuilder.Entity<Identity.Cupos>()
                .HasKey(c => new { c.NumeroTercero, c.TipoMoneda, c.TipoOperacion });

            modelBuilder.Entity<Identity.SolicitudesOperaciones>()
                .HasKey(c => new { c.TipoDocumento, c.IdOficina, c.IdSolicitud });

            modelBuilder.Entity<Identity.ModalidadesOperacion>()
                .HasKey(c => new { c.TipoOperacion, c.ModalidadOperacion });

            modelBuilder.Entity<Identity.ImpDocumentosOperativos>()
               .HasKey(c => new { c.TipoDocumento, c.IdOficina, c.IdConcepto, c.IdImpuesto });

            modelBuilder.Entity<Identity.Titulos>()
               .HasKey(c => new { c.NumeroTercero, c.IdTitulo, c.TipoTitulo });

            modelBuilder.Entity<Identity.DetalleSolicitudes>()
               .HasKey(c => new { c.TipoDocumento, c.IdOficina, c.IdSolicitud, c.NumeroTercero, c.IdTitulo, c.TipoTitulo });

            modelBuilder.Entity<Identity.ConsecutivosDocumentos>()
               .HasKey(c => new { c.TipoDocumento, c.IdOficina });

            modelBuilder.Entity<Identity.DetalleVinculaciones>()
              .HasKey(c => new { c.TipoDocumento, c.Identificacion, c.IdentificacionTer });

            modelBuilder.Entity<Identity.SeguridadRespuestasTerceros>()
                .HasKey(c => new { c.IdTercero, c.IdPregunta });

            //modelBuilder.Entity<Identity.OrdenantesTerceros>()
            //    .HasKey(c => new { c.IdOrdenanteTercero, c.IdTercero });

            modelBuilder.Entity<Identity.ValidarEventosRadian>()
           .HasKey(c => new { c.TipoEvento, c.TipoObjeto, c.NombreAtributo });

            //Organizar llavez que van a quedar (Optimizar Documentos)
            modelBuilder.Entity<Identity.FirmasDetalleDocumentos>()
         .HasKey(c => new { /*c.IdTercero,*/ c.IdDetDoc, c.NumeroTercero });

            modelBuilder.Entity<Identity.OrdenantesTiposCertificados>()
            .HasKey(c => new { c.TipoDoc, c.TipoOrdenante });

            modelBuilder.Entity<Identity.NotasEventosRadian>().HasKey(x => new { x.TipoEvento, x.TipoNota });

            modelBuilder.Entity<Identity.Terceros>()
                .HasIndex(t => new { t.IdTercero });


            modelBuilder.Entity<Identity.EstadosSolicitudes>()
             .HasKey(e => new { e.TipoDocumento, e.Id });


            modelBuilder.Entity<Identity.SolicitudesOperaciones>()
             .HasOne(so => so.EstadoSol)
             .WithMany(es => es.SolicitudeOperaciones)
             .HasForeignKey(nc => new { nc.TipoDocumento, nc.EstadoSolicitud });

            modelBuilder.Entity<Identity.SolicitudesOperaciones>()
            .HasMany(so => so.DetalleSolicitudes)
            .WithOne(es => es.SolicitudOperacion)
            .HasForeignKey(so => new { so.TipoDocumento, so.IdOficina, so.IdSolicitud });



            modelBuilder.Entity<Identity.NotasEventosRadian>()
                .HasOne(nc => nc.TiposNotasRadian)
                .WithMany(te => te.TiposEvento)
                .HasForeignKey(nc => nc.TipoNota);

            modelBuilder.Entity<Identity.ResumenRecaudos>()
                .HasKey(rr => new { rr.oficina, rr.no_operacion, rr.recibo, rr.no_id_emisor });

            modelBuilder.Entity<Identity.DetalleRecaudos>()
                .HasKey(dr => new { dr.oficina, dr.recibo, dr.no_operacion, dr.no_id_emisor, dr.no_titulo });

            modelBuilder.Entity<Identity.ConsolidadoOperaciones>()
                .HasKey(co => new { co.no_titulo, co.no_id_emisor });

            modelBuilder.Entity<Identity.CNSProyeccionDetalleSolicitudes>(entity =>
            {
                entity.Property(e => e.TotalVrNetoRecibir).HasPrecision(18, 4);
                entity.Property(e => e.TotalVrNetoTitulo).HasPrecision(18, 4);
                entity.Property(e => e.VrAdmon).HasPrecision(18, 4);
                entity.Property(e => e.VrComision).HasPrecision(18, 4);
                entity.Property(e => e.VrFuturo).HasPrecision(18, 4);
                entity.Property(e => e.VrIntereses).HasPrecision(18, 4);
                entity.Property(e => e.VrNetoTitulo).HasPrecision(18, 4);
                entity.Property(e => e.VrPresente).HasPrecision(18, 4);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(
                    message => Debug.WriteLine(message),
                    new[] { DbLoggerCategory.Database.Command.Name },
                    Microsoft.Extensions.Logging.LogLevel.Information
                )
                .EnableSensitiveDataLogging();

            optionsBuilder.ConfigureWarnings(w =>
                    w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

    }
}
