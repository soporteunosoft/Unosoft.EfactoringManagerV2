using Abp.Authorization;
using Abp.Domain.Uow;
using Unosoft.EFactoringManager.Authorization.Roles;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Unosoft.EFactoringManager.Identity;

public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
{
    public SecurityStampValidator(
        IOptions<SecurityStampValidatorOptions> options,
        SignInManager signInManager,
        ILoggerFactory loggerFactory,
        IUnitOfWorkManager unitOfWorkManager)
        : base(options, signInManager, loggerFactory, unitOfWorkManager)
    {
    }
}
