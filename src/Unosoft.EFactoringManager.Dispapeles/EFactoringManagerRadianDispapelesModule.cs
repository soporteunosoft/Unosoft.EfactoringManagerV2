using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Unosoft.EFactoringManager.Dispapeles.Configuration;

namespace Unosoft.EfactoringManager.RadianDispapeles
{
    public class EFactoringManagerRadianDispapelesModule : AbpModule
    {

    


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerRadianDispapelesModule).GetAssembly());
            IocManager.Register<DispapelesService, DispapelesService>(DependencyLifeStyle.Transient);
        }
    }
}