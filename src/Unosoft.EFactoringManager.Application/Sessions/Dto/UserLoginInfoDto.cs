using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Unosoft.EFactoringManager.Authorization.Users;

namespace Unosoft.EFactoringManager.Sessions.Dto;

[AutoMapFrom(typeof(User))]
public class UserLoginInfoDto : EntityDto<long>
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public string UserName { get; set; }

    public string EmailAddress { get; set; }

    public bool? Firma { get; set; }
    public int NumeroTercero { get; set; }
    public string IdTercero { get; set; }
    public string IdTerceroPadre { get; set; }
    public int? NumeroTerceroPadre { get; set; }
    public bool? Comisionista { get; set; }
    public bool? Cliente { get; set; }
}
