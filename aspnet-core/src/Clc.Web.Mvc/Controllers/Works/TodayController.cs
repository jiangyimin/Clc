using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using System.Threading.Tasks;
using Clc.ArticleRecords;
using Clc.Web.MessageHandlers;
using Clc.BoxRecords;
using Microsoft.AspNetCore.Http;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class TodayController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IArticleRecordAppService _articleRecordAppService;
        private readonly IBoxRecordAppService _boxRecordAppService;

        public TodayController(IWorkAppService workAppService, 
            IArticleRecordAppService articleRecordAppService,
            IBoxRecordAppService boxRecordAppService)
        {
            _workAppService = workAppService;
            _articleRecordAppService = articleRecordAppService;
            _boxRecordAppService = boxRecordAppService;
        }

        public ActionResult Signins()
        {
            return View();
        }

        public ActionResult ArticleList()
        {
            return View();
        }
        public ActionResult BoxList()
        {
            return View();
        }
        
        public async Task ReportArticleTo()
        {
            var toUser = _workAppService.GetReportToManagers();
            if (string.IsNullOrEmpty(toUser)) return;

            var data = await _articleRecordAppService.GetReportData();
            foreach(var a in data)
            {
                string title = string.Format("今日<{0}>领用情况", a.Name);
                string desc = string.Format("已领数量：{0}， 未还数量：{1}", a.LendCount, a.ReturnCount);
                WeixinUtils.SendTextCard("app03", toUser, title, desc);
            }
        }
        public async Task ReportBoxTo()
        {
            // var toUser = _workAppService.GetReportToManagers();
            var data = await _boxRecordAppService.GetReportData();

            foreach(var a in data)
            {
                string title = a.OutletName;
                string desc = string.Format("贵行的{0}于{1}入库", a.BoxName, a.InTime);
                if (!string.IsNullOrEmpty(a.ToUser))
                    WeixinUtils.SendTextCard("app04", a.ToUser, title, desc);
                //WeixinUtils.SendTextCard("app03", toUser, title, desc);
            }
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
            var output = await _articleRecordAppService.GetArticlesAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataBox()
        {
            var output = await _boxRecordAppService.GetBoxesAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }
    }
}