using Unosoft.EFactoringManager.Configuration.Dto;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
