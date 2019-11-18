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

        private readonly IWeixinAppService _weixinAppService;


        public WwApp01Controller(IHostingEnvironment env, IHubContext<MyChatHub> context, IWeixinAppService weixinAppService)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App01")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App01")];

            _context = context;
            _weixinAppService = weixinAppService;
        }

        // 申请开门
        public ActionResult AskDoor()
        {
            var workerId = GetWeixinUserId();
            var depotId = WorkManager.GetWorkerDepotId(workerId);

            AskDoorViewModel vm = new AskDoorViewModel();
            var ret = WorkManager.GetDoorsForAsk(DateTime.Now.Date, depotId, workerId);
            if (ret.Item1 == 0) 
                return RedirectToAction("WeixinNotify", "Error", new { Message = "你目前不能申请开门" });

            foreach (var door in ret.Item2)
                vm.Workplaces.Add(new ComboItemModel{ Id = door.Id, Name = door.Name });
            return View("AskDoor", vm);
        }

        [HttpPost]
        public ActionResult AskDoor(AskDoorViewModel vm)
        {
            var ret = WorkManager.AskOpenDoor(GetWeixinUserId(), vm.AffairId, vm.WorkplaceId, 1);
            if (ret.Item1 == true) {
                var name = WorkManager.GetWorkerName(GetWeixinUserId());
                _context.Clients.All.SendAsync("getMessage", "emergDoor " + name);
            }           
            return RedirectToAction("WeixinNotify", "Error", new { Message = ret.Item2 });
        }

        private int GetWeixinUserId()
        {
            var claim = HttpContext.User.Claims.First(x => x.Type == "UserId");
            int id = int.Parse(claim.Value);
            return id;
        }
    }
}