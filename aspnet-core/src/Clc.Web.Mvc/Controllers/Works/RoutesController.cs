using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Routes;
using System.Threading.Tasks;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange, PermissionNames.Pages_Aux)]
    public class RoutesController : ClcControllerBase
    {
        private readonly IRouteAppService _routeAppService;

        public RoutesController(IRouteAppService routeAppService)
        {
            _routeAppService = routeAppService;
       }

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult AuxArrange()
        {
            return View();
        }
        public ActionResult Query()
        {
            return View(0);
        }

        [DontWrapResult]
        public async Task<JsonResult> GridData(DateTime carryoutDate)
        {
            var output = await _routeAppService.GetRoutesAsync(carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> AuxGridData(DateTime carryoutDate, int workplaceId)
        {
            var output = await _routeAppService.GetAuxRoutesAsync(carryoutDate, workplaceId, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> QueryGridData(DateTime carryoutDate, int depotId)
        {
            var output = await _routeAppService.GetQueryRoutesAsync(carryoutDate, depotId, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _routeAppService.GetRouteWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(int id)
        {
            var output = await _routeAppService.GetRouteTasks(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataEvent(int id)
        {
            var output = await _routeAppService.GetRouteEvents(id);
            return Json( new { rows = output });
        }
        [DontWrapResult]
        public async Task<JsonResult> GridDataArticle(int id)
        {
            var output = await _routeAppService.GetRouteArticles(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataInBox(int id)
        {
            var output = await _routeAppService.GetRouteInBoxes(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataOutBox(int id)
        {
            var output = await _routeAppService.GetRouteOutBoxes(id, GetSorting());
            return Json( new { rows = output });
        }
    }
}