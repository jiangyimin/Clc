using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_TodayArrange)]
    public class WarehouseTasksController : ClcControllerBase
    {
        private readonly IWarehouseAppService _warehosueAppService;
        public WhAffairsController(IWarehouseAppService whAffairAppService)
        {
            _whAffairAppService = whAffairAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WhAffairWorkersStat()
        {
            WhAffairsViewModel vm = new WhAffairsViewModel() {
                UserDepots = DomainManager.GetCurrentUserDepots()
            };
            return View(vm);
        }

        [DontWrapResult]
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

    }
}