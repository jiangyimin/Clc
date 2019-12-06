using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Works;
using Clc.DoorRecords;
using Microsoft.AspNetCore.SignalR;
using Clc.RealTime;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    // [Authorize(AuthenticationSchemes = "Cookies")]
    public class WwApp04Controller : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private IHubContext<MyChatHub> _context;
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        private readonly IDoorRecordAppService _doorAppService;

        public WwApp04Controller(IHostingEnvironment env, IHubContext<MyChatHub> context, IDoorRecordAppService doorAppService)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App04")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App04")];

            _context = context;
            _doorAppService = doorAppService;
        }

        public ActionResult Index(string id, string code)
        {
            return View();
        }
        
        public ActionResult Grids()
        {
            return View();
        }

        public ActionResult QrCode()
        {
            return View();
        }

        public ActionResult ReceiveBox(int id)
        {
            // var dto = new WxOutletDto();

            // using (CurrentUnitOfWork.SetTenantId(id))
            // using (AbpSession.Use(id, null))
            // {
            //     Worker worker = DomainManager.GetWorkerByCn("62536");
            //     dto.SetWorker(worker);
            //     dto.SetWorker2(DomainManager.GetWorkerByCn("62563"));
            //     dto.SetVehice(DomainManager.GetVehicles(worker.DepotId)[0]);

            //     dto.TaskBoxs.Add(new WeixinTaskBoxDto("07", "7号柜员箱"));
            //     dto.TaskBoxs.Add(new WeixinTaskBoxDto("12", "12号柜员箱"));
            //     dto.TaskBoxs.Add(new WeixinTaskBoxDto("Q1", "1号清分箱"));
            // }
            return View();
        }
    }
}