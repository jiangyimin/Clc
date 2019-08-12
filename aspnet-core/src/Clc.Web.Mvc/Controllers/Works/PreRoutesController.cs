using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.PreRoutes;
using System.Threading.Tasks;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class PreRoutesController : ClcControllerBase
    {
        private readonly IPreRouteAppService _preRouteAppService;

        public PreRoutesController(IPreRouteAppService preRouteAppService)
        {
            _preRouteAppService = preRouteAppService;
       }

        public ActionResult Index()
        {
            return View();
        }

       [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var output = await _preRouteAppService.GetPreRoutesAsync(GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var sorting = GetSorting();
            var output = await _preRouteAppService.GetPreRouteWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(int id)
        {
            var output = await _preRouteAppService.GetPreRouteTasks(id, GetSorting());
            return Json( new { rows = output });
         }
    }
}