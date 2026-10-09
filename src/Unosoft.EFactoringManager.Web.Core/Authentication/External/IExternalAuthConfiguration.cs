using System.Collections.Generic;

namespace Unosoft.EFactoringManager.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
