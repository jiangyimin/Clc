using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using Clc.Routes;
using Clc.ArticleRecords;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article)]
    public class ArticleWorkController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IRouteAppService _routeAppService;

        private readonly IArticleRecordAppService _recordAppService;

        public ArticleWorkController(IArticleRecordAppService recordAppService, IWorkAppService workAppService, IRouteAppService routeAppService)
        {
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
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _routeAppService.GetRouteWorkers(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataArticle(bool isReturn)
        {
            var output = await _recordAppService.GetArticlesAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }
    }
}