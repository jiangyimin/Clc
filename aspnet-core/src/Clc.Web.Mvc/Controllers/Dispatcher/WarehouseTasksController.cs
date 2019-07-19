using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Warehouses;
using Clc.Web.Models.Today;
using Clc.Today;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_TodayArrange)]
    public class WarehouseTasksController : ClcControllerBase
    {
        private readonly IWarehouseAppService _warehouseAppService;
        private readonly TodayManager _todayManager;

        public WarehouseTasksController(IWarehouseAppService warehouseAppService, TodayManager todayManager)
        {
            _warehouseAppService = warehouseAppService;
            _todayManager = todayManager;
        }

        public ActionResult Index()
        {
            WarehouseTasksViewModel vm = new WarehouseTasksViewModel() {
                Today = _todayManager.ToDayString,
                DepotId = _todayManager.DepotId,
                WarehouseNames = new List<string>()
            };
            return View(vm);
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