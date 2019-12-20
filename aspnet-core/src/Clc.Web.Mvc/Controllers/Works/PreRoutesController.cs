using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.PreRoutes;
using System.Threading.Tasks;
using Clc.Fields;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class PreRoutesController : ClcControllerBase
    {
        private readonly IPreRouteAppService _preRouteAppService;
        private readonly IFieldAppService _fieldAppService;

        public PreRoutesController(IPreRouteAppService preRouteAppService, IFieldAppService fieldAppService)
        {
            _preRouteAppService = preRouteAppService;
            _fieldAppService = fieldAppService;
       }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VehicleWorkers()
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

        [DontWrapResult]
        public JsonResult GridDataVehicle()
        {
            var output = _fieldAppService.GetVehicleListItems();
            return Json( new { rows = output });
        }

    
        [DontWrapResult]
        public async Task<JsonResult> GridDataVehicleWorker(int id)
        {
            var sorting = GetSorting();
            var output = await _preRouteAppService.GetPreVehicleWorkers(id, GetSorting());
            return Json( new { rows = output });
        }
    }
}