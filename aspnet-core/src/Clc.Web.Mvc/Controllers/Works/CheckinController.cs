using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Affairs;
using Clc.Fields;
using Clc.Works;
using Clc.Works.Dto;
using Microsoft.AspNetCore.SignalR;
using Clc.RealTime;
using Clc.Runtime;
using Clc.Configuration;
using Abp.Configuration;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article, PermissionNames.Pages_Box, PermissionNames.Pages_Monitor, PermissionNames.Pages_Aux)]
    public class CheckinController : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; } 
        private IHubContext<MyChatHub> _context;
        private readonly IAffairAppService _affairAppService;
        private readonly IWorkAppService _workAppService;

        public CheckinController(IHubContext<MyChatHub> context,
            IAffairAppService affairAppService,
            IWorkAppService workAppService)
        {
            _context = context;
            _affairAppService = affairAppService;
            _workAppService = workAppService;
        }

        public ActionResult Index(AffairWorkDto affair)
        {
            if (!affair.AltCheck && affair.AffairId == 0) {
                var vm = _workAppService.FindDutyAffair();
                return View(vm);
            }
            return View(affair);
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult AskOpen(int[] workers, int affairId, int doorId, string start, string end, int taskId, bool wait)
        {
            int minNum = int.Parse(SettingManager.GetSettingValue(AppSettingNames.Rule.MinWorkersOnDuty));
             if (workers.Length < minNum)
                return Json(new {success = false, message = "确认人数不够最低要求"});

            var wp = WorkManager.GetWorkplace(doorId);
            if (wp.AskOpenDeadline == 0) {
                if (!ClcUtils.NowInTimeZone(start, end)) return Json(new {success = false, message = "不在申请开门时段"});
            }
            else {
                if (!ClcUtils.NowInTimeZone(start, 0, wp.AskOpenDeadline)) return Json(new {success = false, message = "不在申请开门时段"});
            }

            var ret = WorkManager.AskOpenDoor(wp.DepotId, affairId, doorId, workers, wait);

            if (ret.Item1)
            {
                // set start Time for VaultTask
                if (taskId > 0) _affairAppService.SetTaskTime(taskId, true);
                
                var depotName = WorkManager.GetDepot(wp.DepotId).Name;
                _context.Clients.All.SendAsync("getMessage", "askOpenDoor " + string.Format("你有来自{0}{1}的任务开门申请", depotName, wp.Name));
            }

            return Json(new { success = true, message = ret.Item2 });
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult AskVaultGuard(int depotId)
        {
            var name = WorkManager.GetVaultName(depotId);
            
            if (string.IsNullOrEmpty(name)) 
            {
                return Json(new { success = false, message = "没有金库需要设防" });
            }
            else
            {
                _context.Clients.All.SendAsync("getMessage", "askVaultGuard " + string.Format("你有来自({0})的金库设防申请", name));
                return Json(new { success = true, message = "你的设防申请已发往监控中心" });
            }

        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _affairAppService.GetAffairWorkersAsync(id);
            if (output != null)
            {
                foreach (var w in output) {
                    var photo = WorkManager.GetWorker(w.WorkerId).Photo;
                    if (photo != null) w.PhotoString = Convert.ToBase64String(photo);
                }
            }
            return Json(new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(int id)
        {
            var output = await _affairAppService.GetAffairTasksAsync(id, GetSorting());
            return Json(new { rows = output });
        }
    }
}