using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Unosoft.EFactoringManager.Controllers
{
    public abstract class EFactoringManagerControllerBase : AbpController
    {
        protected EFactoringManagerControllerBase()
        {
            LocalizationSourceName = EFactoringManagerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
