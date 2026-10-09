using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Threading.BackgroundWorkers;
using Unosoft.EFactoringManager.BackgroundJob;
using Unosoft.EFactoringManager.Billy;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Unosoft.EfactoringManager.EBillServices;
using Unosoft.EfactoringManager.RadianDispapeles;
using Unosoft.EFactoringManager.Authorization.Roles;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.Configuration;
using Unosoft.EFactoringManager.Localization;
using Unosoft.EFactoringManager.MultiTenancy;
using Unosoft.EFactoringManager.Timing;
using UnoSoft.EFactoringManager.CrystalReportService;

namespace Unosoft.EFactoringManager;

[DependsOn(typeof(AbpZeroCoreModule), 
           typeof(EFactoringManagerRadianDispapelesModule), 
           typeof(EFactoringManagerEBillServiceModule), 
           typeof(EFactoringManagerCrystalReportServiceModule))]
public class EFactoringManagerCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;

        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        EFactoringManagerLocalizationConfigurer.Configure(Configuration.Localization);

        // Enable this line to create a multi-tenant application.
        Configuration.MultiTenancy.IsEnabled = EFactoringManagerConsts.MultiTenancyEnabled;

        // Configure roles
        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        Configuration.Settings.Providers.Add<AppSettingProvider>();

        Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));

        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = EFactoringManagerConsts.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = EFactoringManagerConsts.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;

        var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
        var configuration = AppConfigurations.Get(typeof(EFactoringManagerCoreModule).GetAssembly().GetDirectoryPathOrNull());
#if !DEBUG
        var host = configuration["EBillSettings:Host"];
        var username = configuration["EBillSettings:UserName"];
        var password = configuration["EBillSettings:Password"];

        if (!string.IsNullOrWhiteSpace(host)
            && !string.IsNullOrWhiteSpace(username)
            && !string.IsNullOrWhiteSpace(password))
        {
            workManager.Add(IocManager.Resolve<ValidarArchivosFtpWroker>());
        }

        var urlBase = configuration["BillySettings:UrlBase"];
        var token = configuration["BillySettings:Token"];
        var market = configuration["BillySettings:Market"];

        if (!string.IsNullOrWhiteSpace(urlBase)
            && !string.IsNullOrWhiteSpace(token)
            && !string.IsNullOrWhiteSpace(market))
        {
            workManager.Add(IocManager.Resolve<ValidInvoinceBillyWorker>());
        }
#endif
    }
}
