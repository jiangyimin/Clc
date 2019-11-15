using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Weixin;
using Clc.Weixin.Dto;
using Clc.Extensions;
using Clc.Web.Models.Weixin;
using Clc.Runtime.Cache;
using Clc.Works;
using System.Linq;
using System.Collections.Generic;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class WwApp01Controller : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        private readonly IWeixinAppService _weixinAppService;


        public WwApp01Controller(IHostingEnvironment env, IWeixinAppService weixinAppService)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App01")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App01")];

            _weixinAppService = weixinAppService;
        }

        // 申请应急开门
        public ActionResult EmergDoor()
        {
            var workerId = GetWeixinUserId();

            if (workerId == 0 || WorkManager.WorkerHasDefaultWorkRoleName(workerId, "队长")) 
                throw new System.Exception("无此人或不是队长职务");

            EmergDoorViewModel vm = new EmergDoorViewModel();
            var doors = WorkManager.GetDoors(workerId);
            vm.Workplaces = new List<ComboItemModel>();
            foreach (var door in doors)
                vm.Workplaces.Add(new ComboItemModel{ Id = door.Id, Name = door.Name });
            return View("EmergDoor", vm);
        }

        [HttpPost]
        public ActionResult EmergDoor(EmergDoorViewModel vm)
        {
            // _weixinAppService.ProcessEmergDoor(vm);

            return RedirectToAction("WeixinNotify", "Error", new { Message = "object" });
        }


        private int GetWeixinUserId()
        {
            var claim = HttpContext.User.Claims.First(x => x.Type == "UserId");
            int id = int.Parse(claim.Value);
            return id;
        }
    }
}