using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Unosoft.EFactoringManager.EntityFrameworkCore;
using Unosoft.EFactoringManager.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Unosoft.EFactoringManager.Web.Tests;

[DependsOn(
    typeof(EFactoringManagerWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class EFactoringManagerWebTestModule : AbpModule
{
    public EFactoringManagerWebTestModule(EFactoringManagerEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(EFactoringManagerWebMvcModule).Assembly);
    }
}