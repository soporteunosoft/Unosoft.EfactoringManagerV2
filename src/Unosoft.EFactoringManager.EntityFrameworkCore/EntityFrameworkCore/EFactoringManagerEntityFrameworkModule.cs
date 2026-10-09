using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Unosoft.EFactoringManager.EntityFrameworkCore.Seed;

namespace Unosoft.EFactoringManager.EntityFrameworkCore;

[DependsOn(
    typeof(EFactoringManagerCoreModule),
    typeof(AbpZeroCoreEntityFrameworkCoreModule))]
public class EFactoringManagerEntityFrameworkModule : AbpModule
{
    /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
    public bool SkipDbContextRegistration { get; set; }

    public bool SkipDbSeed { get; set; }

    public override void PreInitialize()
    {
        if (!SkipDbContextRegistration)
        {
            Configuration.Modules.AbpEfCore().AddDbContext<EFactoringManagerDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    EFactoringManagerDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    EFactoringManagerDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerEntityFrameworkModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        if (!SkipDbSeed)
        {
            SeedHelper.SeedHostDb(IocManager);
        }
    }
}
