using Abp.Authorization;
using Unosoft.EFactoringManager.Authorization.Roles;
using Unosoft.EFactoringManager.Authorization.Users;

namespace Unosoft.EFactoringManager.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
