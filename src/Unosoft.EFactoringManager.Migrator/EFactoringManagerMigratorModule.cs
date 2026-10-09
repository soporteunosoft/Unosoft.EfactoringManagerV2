using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.EntityFrameworkCore;
using Unosoft.EFactoringManager.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace Unosoft.EFactoringManager.Migrator;

[DependsOn(typeof(EFactoringManagerEntityFrameworkModule))]
public class EFactoringManagerMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public EFactoringManagerMigratorModule(EFactoringManagerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(EFactoringManagerMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            EFactoringManagerConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
