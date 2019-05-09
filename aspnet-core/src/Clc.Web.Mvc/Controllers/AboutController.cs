using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Clc.Controllers;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : ClcControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
