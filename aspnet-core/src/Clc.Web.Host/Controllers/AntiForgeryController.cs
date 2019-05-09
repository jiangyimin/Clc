using Microsoft.AspNetCore.Antiforgery;
using Clc.Controllers;

namespace Clc.Web.Host.Controllers
{
    public class AntiForgeryController : ClcControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
