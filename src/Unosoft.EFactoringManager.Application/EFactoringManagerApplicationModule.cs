using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Unosoft.EFactoringManager.Authorization;

namespace Unosoft.EFactoringManager;

[DependsOn(
    typeof(EFactoringManagerCoreModule),
    typeof(AbpAutoMapperModule))]
public class EFactoringManagerApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<EFactoringManagerAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(EFactoringManagerApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
