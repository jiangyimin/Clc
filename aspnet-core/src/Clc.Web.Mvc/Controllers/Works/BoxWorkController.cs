using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using Clc.Routes;
using Clc.BoxRecords;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Box)]
    public class BoxWorkController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IRouteAppService _routeAppService;

        private readonly IBoxRecordAppService _recordAppService;

        public BoxWorkController(IBoxRecordAppService recordAppService, IWorkAppService workAppService, IRouteAppService routeAppService)
        {
            _recordAppService = recordAppService;
            _routeAppService = routeAppService;
            _workAppService = workAppService;
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult InBox()
        {
            return View();
        }

        public ActionResult OutBox()
        {
            return View();
        }

        public ActionResult RecordQuery()
        {
            return View();
        }
        
        [DontWrapResult]
        public JsonResult GridData(int wpId, DateTime carryoutDate, int depotId, int affairId)
        {
            var output = _workAppService.GetActiveRoutes(wpId, carryoutDate, depotId, affairId);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(int id)
        {
            var output = await _routeAppService.GetRouteTasks(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataBox(bool isReturn)
        {
            var output = await _recordAppService.GetBoxesAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }
    }
}