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
            if (!affair.Alt && affair.AffairId == 0) {
                var vm = _workAppService.FindDutyAffair();
                return View(vm);
            }
            return View(affair);
        }

        public ActionResult GetPhoto(string id)
        {
            Worker w = WorkManager.GetWorkerByRfid(id);
            if (w != null && w.Photo != null)
                return File(w.Photo, "image/jpg");

            return File(new byte[0], "image/jpg");
        }

        [HttpPost]
        [DontWrapResult]
        public JsonResult AskOpenByRfid(string rfid, int affairId, int doorId)
        {
            var worker = WorkManager.GetWorkerByRfid(rfid);
            if (worker == null) 
                return Json(new { success = false, message = "此RFID无对应的人员" });

            var ret = WorkManager.AskOpenDoor(worker.Id, affairId, doorId, 0);
            if (ret.Item1)
            {
                var depotName = WorkManager.GetDepot(worker.DepotId).Name;
                var wpName = WorkManager.GetWorkplace(doorId).Name;
                _context.Clients.All.SendAsync("getMessage", "askOpenDoor " + string.Format("你有来自{0}({1})的常规申请", wpName, depotName));
            }

            return Json(new { success = ret.Item1, message = ret.Item2, worker = new { name = string.Format("{0} {1}", worker.Cn, worker.Name) }});
        }


        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _affairAppService.GetAffairWorkersAsync(id);
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