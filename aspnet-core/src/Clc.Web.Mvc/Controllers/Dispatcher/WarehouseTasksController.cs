using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Warehouses;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_TodayArrange)]
    public class WarehouseTasksController : ClcControllerBase
    {
        private readonly IWarehouseAppService _warehouseAppService;
        public WarehouseTasksController(IWarehouseAppService warehouseAppService)
        {
            _warehouseAppService = warehouseAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WhAffairWorkersStat()
        {
            return View();
        }

/*         [DontWrapResult]
        public JsonResult GridData(int depotId, DateTime carryoutDate)
        {
            var output = _whAffairAppService.GetAffairs(depotId, carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public JsonResult WorkersGridData(int id)
        {
            var output = _whAffairAppService.GetAffairWorkers(id, GetSorting());
            return Json( new { rows = output });
        }
 */
    }
}