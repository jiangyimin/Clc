using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Weixin;
using Clc.Web.Models.Weixin;
using Clc.Works;
using System.Linq;
using Clc.DoorRecords;
using Microsoft.AspNetCore.SignalR;
using Clc.RealTime;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class WwApp03Controller : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private IHubContext<MyChatHub> _context;
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        private readonly IDoorRecordAppService _doorAppService;

        public WwApp03Controller(IHostingEnvironment env, IHubContext<MyChatHub> context, IDoorRecordAppService doorAppService)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App03")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App03")];

            _context = context;
            _doorAppService = doorAppService;
        }

        // 审批应急开门
        public ActionResult ApproveEmergDoor()
        {
            var workerId = GetWeixinUserId();

            if (workerId == 0 || WorkManager.WorkerHasDefaultWorkRoleName(workerId, "公司领导")) 
                throw new System.Exception("无此人或不是公司领导职务");

            ApproveEmergDoorRecordViewModel vm = new ApproveEmergDoorRecordViewModel();
            var record = _doorAppService.GetLastUnApproveEmergDoor(workerId).Result;
            if (record != null) 
            {
                vm.Id = record.Id;
                vm.IssueContent = record.IssueContent;
                vm.WorkplaceName = record.WorkplaceName;
                return View("ApproveEmergDoor", vm);
            }
            else 
                return RedirectToAction("WeixinNotify", "Error", new { Message = "你没有可审批的应急开门申请"});
        }

        [HttpPost]
        public ActionResult ApproveEmergDoor(ApproveEmergDoorRecordViewModel vm)
        {
            using (CurrentUnitOfWork.SetTenantId(1))
            {
                // _doorAppService.ApproveEmergDoor(vm.Id);
            }
            
            var name = WorkManager.GetWorkerName(GetWeixinUserId());
            _context.Clients.All.SendAsync("getMessage", "emergDoor " + name);
            return RedirectToAction("WeixinNotify", "Error", new { Message = "同意审批，并下达到监控室" });
        }


        private int GetWeixinUserId()
        {
            var claim = HttpContext.User.Claims.First(x => x.Type == "UserId");
            int id = int.Parse(claim.Value);
            return id;
        }

    }
}