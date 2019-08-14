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
    [AbpMvcAuthorize(PermissionNames.Pages_Article)]
    public class ArticleWorkController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;

        public ArticleWorkController(IWorkAppService workAppService)
        {
            _workAppService = workAppService;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Lend()
        {
            return View();
        }

        public ActionResult Return()
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