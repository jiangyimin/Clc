﻿using System;
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
using Clc.VehicleRecords;
using Microsoft.AspNetCore.SignalR;
using Clc.RealTime;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class TodayController : ClcControllerBase
    {
        private IHubContext<MyChatHub> _context;
        private readonly IWorkAppService _workAppService;
        private readonly IArticleRecordAppService _articleRecordAppService;
        private readonly IBoxRecordAppService _boxRecordAppService;
        private readonly IVehicleRecordAppService _vehicleRecordAppService;

        public TodayController(IHubContext<MyChatHub> context,
            IWorkAppService workAppService, 
            IArticleRecordAppService articleRecordAppService,
            IBoxRecordAppService boxRecordAppService,
            IVehicleRecordAppService vehicleRecordAppService)
        {
            _context = context;
            _workAppService = workAppService;
            _articleRecordAppService = articleRecordAppService;
            _boxRecordAppService = boxRecordAppService;
            _vehicleRecordAppService = vehicleRecordAppService;
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

        public ActionResult TaskList()
        {
            return View();
        }

        public ActionResult VehicleList()
        {
            return View();
        }

        public ActionResult DoorRecordQuery()
        {
            var me = _workAppService.GetMe();
            return RedirectToAction("RecordQuery", "OpenDoors", new { depotId = me.DepotId });
        }
        
        public async Task ReportArticleTo(string id)
        {
            var ret = _workAppService.GetReportToManagers();
            if (string.IsNullOrEmpty(ret.Item2)) return;

            var data = await _articleRecordAppService.GetReportData();
            string title = string.Format("{0}报告今日<{1}>物品领用情况", id, ret.Item1);
            string desc = null;
            foreach(var a in data)
            {
                desc += string.Format("{0}今领：{1}， 未还：{2}\n", a.Name, a.LendCount, a.UnReturnCount);
            }
            WeixinUtils.SendTextCard("App03", ret.Item2, title, desc);
            _workAppService.SetReportDate();

            await _context.Clients.All.SendAsync("getMessage", "askVaultGuard " + string.Format("你有来自({0})的设防申请", ret.Item1));

        }

        public void ReportTaskTo(string id) 
        {
            var ret = _workAppService.GetReportToManagers();
            if (string.IsNullOrEmpty(ret.Item2)) return;

            var data = _workAppService.GetTaskReportData();
            string title = string.Format("{0}报告今日<{1}>线路任务情况", id, ret.Item1);
            string desc = string.Format("线路数：{0} 条 (安排人员： {1}人）\n", data.Route.Count1, data.Route.Count2);
            desc += string.Format("内务数：{0} 条 (安排人员： {1}人）\n", data.Affair.Count1, data.Affair.Count2);
            desc += string.Format("收费中调数：{0} 个 (收费： {1}元）\n", data.Task.Count1, data.Task.Count2);
            WeixinUtils.SendTextCard("App03", ret.Item2, title, desc);
            _workAppService.SetReportDate();
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

        public async Task ReportVehicleTo(string id)
        {
            var ret = _workAppService.GetReportToManagers();
            if (string.IsNullOrEmpty(ret.Item2)) return;

            var data = await _vehicleRecordAppService.GetReportData();
            
            string title = string.Format("{0}报告今日<{1}>车辆加油维修情况", id, ret.Item1);;
            string desc = string.Format("加油次数：{0} (量：{1}}升   费用:{2}元\n", data.OilCount, data.OilQuantity, data.OilPrice);
            desc += string.Format("车辆维修次数：{0} (费用： {1}元)", data.MTCount, data.MTPrice);

            WeixinUtils.SendTextCard("App04", ret.Item2, title, desc);
            _workAppService.SetReportDate();
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

        [DontWrapResult]
        public async Task<JsonResult> GridDataOil()
        {
            var output = await _vehicleRecordAppService.GetOilRecordsAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataMT()
        {
            var output = await _vehicleRecordAppService.GetVehicleMTRecordsAsync(GetPagedInput());
            return Json(new { total = output.TotalCount, rows = output.Items });
        }
    }
}