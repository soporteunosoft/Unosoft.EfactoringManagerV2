using Abp.MultiTenancy;
using Unosoft.EFactoringManager.Editions;
using Unosoft.EFactoringManager.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Unosoft.EFactoringManager.EntityFrameworkCore.Seed.Tenants;

public class DefaultTenantBuilder
{
    private readonly EFactoringManagerDbContext _context;

    public DefaultTenantBuilder(EFactoringManagerDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateDefaultTenant();
    }

    private void CreateDefaultTenant()
    {
        // Default tenant

        var defaultTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == AbpTenantBase.DefaultTenantName);
        if (defaultTenant == null)
        {
            defaultTenant = new Tenant(AbpTenantBase.DefaultTenantName, AbpTenantBase.DefaultTenantName);

            var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                defaultTenant.EditionId = defaultEdition.Id;
            }

            _context.Tenants.Add(defaultTenant);
            _context.SaveChanges();
        }
    }
}
