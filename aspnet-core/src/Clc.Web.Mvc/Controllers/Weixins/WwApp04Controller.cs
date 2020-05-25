using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Works;
using Clc.DoorRecords;
using Microsoft.AspNetCore.SignalR;
using Clc.RealTime;
using Clc.Weixin.Dto;
using Clc.Fields;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Clc.Weixin;
using System.Collections.Generic;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class WwApp04Controller : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private IHubContext<MyChatHub> _context;
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        private readonly IWeixinAppService _wxAppService;

        public WwApp04Controller(IHostingEnvironment env, IHubContext<MyChatHub> context, IWeixinAppService wxAppService)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App04")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App04")];

            _context = context;
            _wxAppService = wxAppService;
        }

        public ActionResult Grids()
        {
            var cn = GetWeixinCn();
            return View(WorkManager.GetOutletByCn(cn));
        }

        public ActionResult Lookup()
        {
            var id = GetWeixinUserId();            
            // var outlet = WorkManager.GetOutlet(id);
            var tasks = _wxAppService.GetTodayTasks(id);
            
            return View(tasks);
        }

        public ActionResult Evaluate()
        {
            try
            {
                //var cn = GetWeixinCn();
                var outlet = WorkManager.GetOutletByCn("010102");
                var tasks = _wxAppService.GetTodayTasks(outlet.Id);
            
                return View(tasks);
            }
            catch (System.Exception e)
            {
                return Content(e.Message);
            }
        }

        public ActionResult LookupTask(int taskId, int routeId)
        {
            WxIdentifyDto dto = _wxAppService.GetLookupInfo(taskId, routeId);

            return View(dto);
        }

        public ActionResult EvaluateTask(int taskId, int routeId)
        {
            return View(taskId);
        }

        [HttpPost]
        public ActionResult SubmitEvaluateTask(int taskId, int rated, string info)
        {
            _wxAppService.UpdateTaskRate(taskId, rated, info);

            return RedirectToAction("WeixinNotify", "Error", new { Message = "成功提交评价" });
        }

        private string GetWeixinCn()
        {
            var claim = HttpContext.User.Claims.First(x => x.Type == "Cn");
            return claim.Value;
        }
        private int GetWeixinUserId()
        {
            var claim = HttpContext.User.Claims.First(x => x.Type == "UserId");
            int id = int.Parse(claim.Value);
            return id;
        }

    }
}