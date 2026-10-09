# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

```powershell
# Restore & build
dotnet restore
dotnet build

# Run the API (port 44311)
dotnet run --project src/Unosoft.EFactoringManager.Web.Host

# Run tests
dotnet test test/Unosoft.EFactoringManager.Tests
dotnet test test/Unosoft.EFactoringManager.Web.Tests

# Run a single test
dotnet test test/Unosoft.EFactoringManager.Tests --filter "FullyQualifiedName~TestClassName"

# Apply database migrations
dotnet run --project src/Unosoft.EFactoringManager.Migrator

# Add EF Core migration
dotnet ef migrations add <MigrationName> --project src/Unosoft.EFactoringManager.EntityFrameworkCore

# Docker build and run (backend + Angular frontend)
.\build\build-with-ng.ps1
cd docker/ng && .\up.ps1    # start
cd docker/ng && .\down.ps1  # stop
```

## Architecture

This is an **ABP Framework v10.3.0** application (layered N-tier) targeting **.NET 9.0** with an Angular frontend (separate repo, expected at localhost:4200).

### Layer Dependencies

```
Web.Host → Web.Core → Application → Core
                    ↘ EntityFrameworkCore → Core
Core ← [BillyServices, Certificados, Dispapeles, EBillServices,
         ExcelReportService, CrystalReportService, RadianDispapeles]
```

### Projects

| Project | Purpose |
|---|---|
| `Core` | Domain entities, business rules, domain services |
| `Application` | Application services, DTOs (AutoMapper), business logic orchestration |
| `EntityFrameworkCore` | EF Core DbContext, SQL Server, migrations |
| `Web.Core` | Web infrastructure, ABP configuration |
| `Web.Host` | ASP.NET Core API entry point — Swagger, CORS, JWT, SignalR |
| `Migrator` | Standalone CLI for running EF migrations |
| `Utils` | Shared utility helpers |
| `Dispapeles` | REST integration with Dispapeles electronic document platform (AWS) |
| `Certificados` | WCF/SOAP client for Andes digital certificates |
| `EBillServices` | Electronic billing via FTP |
| `BillyServices` | REST integration with Billy accounting/ERP |
| `RadianDispapeles` | Radian (Colombian tax interchange) document format |
| `ExcelReportService` | Excel report generation (ClosedXML) |
| `CrystalReportService` | Crystal Reports generation |

### External Integrations

- **Dispapeles (AWS)**: REST — electronic document management; config key `DispapelesSettings`
- **Andes**: SOAP/WCF — digital signatures & timestamping; config keys `AndesSettings`, `AndesFESettings`
- **Billy**: REST — accounting/invoicing integration; config key `BillySettings`
- **eBill**: FTP — electronic invoicing; config key `EBillSettings`
- **Certificados**: WCF — digital certificate management
- **Radian**: Colombian DIAN tax document standardization

## Key Technologies

- **ABP Framework v10.3.0** — IoC (Castle Windsor), multi-tenancy, auditing, logging (Log4Net)
- **Entity Framework Core 9.0.14** — SQL Server; repository pattern via ABP
- **JWT Bearer** — auth; issuer/audience `EFactoringManager`
- **SignalR** — real-time updates
- **Swagger/Swashbuckle** — API docs at `/swagger`
- **AutoMapper** — DTO mappings in `Application`
- **xUnit + NSubstitute + Shouldly** — testing

## Configuration

**Development connection string** (`appsettings.json`):
```
Server=localhost\SQLEXPRESS; Database=EFactoringManager; Trusted_Connection=True; TrustServerCertificate=True;
```

**Ports**: API → 44311 (HTTPS / Docker 80), Angular → 4200

NuGet sources include the ABP nightly feed (`myget.org/F/abp-nightly`) configured in `NuGet.Config`.

## ABP Conventions

- Application services inherit from `AsyncCrudAppService` or `ApplicationService`
- Entities inherit from `Entity` or `FullAuditedEntity`
- Repositories are injected as `IRepository<TEntity>` — do not create custom DbContext queries unless necessary
- Authorization uses ABP permission system (`PermissionNames` constants in `Core`)
- Module registration happens in `*Module.cs` classes via `DependsOn` attributes
