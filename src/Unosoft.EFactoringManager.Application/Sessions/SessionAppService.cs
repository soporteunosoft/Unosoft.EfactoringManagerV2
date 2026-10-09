using Abp.Auditing;
using Unosoft.EFactoringManager.Sessions.Dto;
using Unosoft.EFactoringManager.Terceros;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Sessions;

public class SessionAppService : EFactoringManagerAppServiceBase, ISessionAppService
{
    private readonly TercerosManager _tercerosManager;

    public SessionAppService(TercerosManager tercerosManager)
    {
        _tercerosManager = tercerosManager;
    }

    [DisableAuditing]
    public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
    {
        var output = new GetCurrentLoginInformationsOutput
        {
            Application = new ApplicationInfoDto
            {
                Version = AppVersionHelper.Version,
                ReleaseDate = AppVersionHelper.ReleaseDate,
                Features = new Dictionary<string, bool>()
            }
        };

        if (AbpSession.TenantId.HasValue)
        {
            output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
        }

        if (AbpSession.UserId.HasValue)
        {
            output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            var tercero = await _tercerosManager.ObtenerTerceroSessionV2(output.User.UserName);
            if (tercero != null)
            {
                output.User.Firma = tercero.Firma;
                output.User.NumeroTercero = tercero.Id;
                output.User.IdTercero = tercero.IdTercero;
                output.User.IdTerceroPadre = tercero.TerceroPadre?.IdTercero;
                output.User.NumeroTerceroPadre = tercero.IdTerceroPadre;
                output.User.Comisionista = tercero.Comisionista;
                output.User.Cliente = tercero.Comisionista;
            }
        }

        return output;
    }
}
