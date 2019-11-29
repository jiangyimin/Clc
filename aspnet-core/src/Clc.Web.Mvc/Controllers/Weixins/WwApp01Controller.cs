using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Weixin;
using Clc.Web.Models.Weixin;
using Clc.Works;
using System.Linq;
using System;
using Microsoft.AspNetCore.SignalR;
using Clc.RealTime;
using Clc.DoorRecords;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class WwApp01Controller : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private IHubContext<MyChatHub> _context;
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        // private readonly IDoorRecordAppService _doorRecordAppService;


        public WwApp01Controller(IHostingEnvironment env, IHubContext<MyChatHub> context, 
            IDoorRecordAppService doorRecordAppService)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App01")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App01")];

            _context = context;
            // _doorRecordAppService = doorRecordAppService;
        }

        // // 申请开门
        // public ActionResult AskDoor()
        // {
        //     var workerId = GetWeixinUserId();
        //     var depotId = WorkManager.GetWorkerDepotId(workerId);

        //     AskDoorViewModel vm = new AskDoorViewModel();
        //     var ret = _doorRecordAppService.GetLastUnApproveAskDoor(DateTime.Now.Date, depotId).Result;
        //     if (ret == null) 
        //         return RedirectToAction("WeixinNotify", "Error", new { Message = "你目前没有申请开门" });

        //     vm.RecordId = ret.Id;
        //     vm.WorkplaceName = ret.WorkplaceName;
        //     vm.WorkerId = workerId;

        //     return View("AskDoor", vm);
        // }

        private int GetWeixinUserId()
        {
            var claim = HttpContext.User.Claims.First(x => x.Type == "UserId");
            int id = int.Parse(claim.Value);
            return id;
        }
    }
}