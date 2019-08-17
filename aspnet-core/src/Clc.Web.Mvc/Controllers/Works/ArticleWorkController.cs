using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Abp.UI;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using System.Threading.Tasks;
using Clc.Routes;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article)]
    public class ArticleWorkController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IRouteAppService _routeAppService;

        public ArticleWorkController(IWorkAppService workAppService, IRouteAppService routeAppService)
        {
            _routeAppService = routeAppService;
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
        public async Task<JsonResult> GridData(DateTime carryoutDate, int affairId)
        {
            var output = await _workAppService.GetRoutesForLendAsync(carryoutDate, affairId);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _routeAppService.GetRouteWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

    }
}