using Abp.MultiTenancy;
using Unosoft.EFactoringManager.Authorization.Users;

namespace Unosoft.EFactoringManager.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
