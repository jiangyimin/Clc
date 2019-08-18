using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Abp.UI;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using System.Threading.Tasks;
using Clc.ArticleRecords;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class TodayController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IArticleRecordAppService _recordAppService;

        public TodayController(IWorkAppService workAppService, IArticleRecordAppService recordAppService)
        {
            _workAppService = workAppService;
            _recordAppService = recordAppService;
        }

        public ActionResult Signins()
        {
            return View();
        }

        public ActionResult ArticleList()
        {
            return View();
        }
        
        [DontWrapResult]
        public async Task<JsonResult> GridData(DateTime carryoutDate)
        {
            var output = await _workAppService.GetSigninsAsync(carryoutDate);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataArticle()
        {
            var output = await _recordAppService.GetArticlesAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }
    }
}