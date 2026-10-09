using Abp.Authorization;
using Abp.Runtime.Session;
using Unosoft.EFactoringManager.Configuration.Dto;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : EFactoringManagerAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
