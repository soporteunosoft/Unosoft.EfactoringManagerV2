using Abp.Application.Services;
using Unosoft.EFactoringManager.Sessions.Dto;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
