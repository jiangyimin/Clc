using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Warehouses;
using Clc.Works;
using Clc.Web.Models.Works;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class AffairsController : ClcControllerBase
    {
        private readonly IAffairAppService _warehouseAppService;
        private readonly WorkManager _workManager;

        public AffairsController(IAffairAppService warehouseAppService, WorkManager workManager)
        {
            _warehouseAppService = warehouseAppService;
            _workManager = workManager;
        }

        public ActionResult Index()
        {
            AffairsViewModel vm = new AffairsViewModel() {
                Today = _workManager.ToDayString,
                DepotId = _workManager.DepotId,
                PlaceNames = new List<string>()
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