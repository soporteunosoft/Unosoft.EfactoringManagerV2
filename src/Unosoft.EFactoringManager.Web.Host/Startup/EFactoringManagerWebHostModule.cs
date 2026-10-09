using Abp.Modules;
using Abp.Reflection.Extensions;
using Unosoft.EFactoringManager.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Unosoft.EFactoringManager.Web.Host.Startup
{
    [DependsOn(
       typeof(EFactoringManagerWebCoreModule))]
    public class EFactoringManagerWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public EFactoringManagerWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerWebHostModule).GetAssembly());
        }
    }
}
