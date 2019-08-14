using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Abp.UI;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using System.Threading.Tasks;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class SigninsController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;

        public SigninsController(IWorkAppService workAppService)
        {
            _workAppService = workAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridData(DateTime carryoutDate)
        {
            var output = await _workAppService.GetSigninsAsync(carryoutDate);
            return Json( new { rows = output });
        }

    }
}