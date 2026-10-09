using Abp.Web.Security.AntiForgery;
using Unosoft.EFactoringManager.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace Unosoft.EFactoringManager.Web.Host.Controllers
{
    public class AntiForgeryController : EFactoringManagerControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly IAbpAntiForgeryManager _antiForgeryManager;

        public AntiForgeryController(IAntiforgery antiforgery, IAbpAntiForgeryManager antiForgeryManager)
        {
            _antiforgery = antiforgery;
            _antiForgeryManager = antiForgeryManager;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }

        public void SetCookie()
        {
            _antiForgeryManager.SetCookie(HttpContext);
        }
    }
}
