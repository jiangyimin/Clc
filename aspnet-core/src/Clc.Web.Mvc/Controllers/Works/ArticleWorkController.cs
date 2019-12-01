using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using Clc.Routes;
using Clc.ArticleRecords;
using Clc.RealTime;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article)]
    public class ArticleWorkController : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private IHubContext<MyChatHub> _context;

        private readonly IWorkAppService _workAppService;
        private readonly IRouteAppService _routeAppService;

        private readonly IArticleRecordAppService _recordAppService;

        public ArticleWorkController(IHubContext<MyChatHub> context,
            IArticleRecordAppService recordAppService, IWorkAppService workAppService, IRouteAppService routeAppService)
        {
            _context = context;
            _recordAppService = recordAppService;
            _routeAppService = routeAppService;
            _workAppService = workAppService;
        }

        public ActionResult List()
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

        public ActionResult TempArticle()
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
            var output = _workAppService.GetCachedRoutes(wpId, carryoutDate, depotId, affairId);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _routeAppService.GetRouteWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataArticle(int wpId)
        {
            var output = await _recordAppService.GetWorkplaceArticlesAsync(wpId, GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult AskOpen(int depotId, int routeId, int affairId, int doorId, string askWorkers)
        {
            WorkManager.RouteAskOpenDoor(routeId, affairId, doorId, askWorkers);

            var depotName = WorkManager.GetDepot(depotId).Name;
            var wpName = WorkManager.GetWorkplace(doorId).Name;
            _context.Clients.All.SendAsync("getMessage", "askOpenDoor " + string.Format("你有来自{0}({1})的线路开门申请", wpName, depotName));

            return Json(new {});
        }

    }
}