using Abp.Application.Services;
using Unosoft.EFactoringManager.MultiTenancy.Dto;

namespace Unosoft.EFactoringManager.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

