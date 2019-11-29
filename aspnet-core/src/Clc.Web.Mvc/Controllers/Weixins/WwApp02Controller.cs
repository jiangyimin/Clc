using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Weixin;
using Clc.Weixin.Dto;
using Clc.Extensions;
using Clc.Web.Models.Weixin;
using Clc.Clients;
using Clc.Runtime.Cache;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class WwApp02Controller : ClcControllerBase
    {
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        private readonly IWeixinAppService _weixinAppService;

        private readonly IOutletCache _outletCache;

        public WwApp02Controller(IHostingEnvironment env, IWeixinAppService weixinAppService, IOutletCache outletCache)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App02")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App02")];

            _weixinAppService = weixinAppService;
            _outletCache = outletCache;
        }

        public ActionResult Identify()
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            
            if (dto == null)
            {
                LoginViewModel vm = new LoginViewModel() 
                {
                    ReturnUrl = AbsoluteUri()
                };
                
                return View("login", vm);
            }
            else
            {
                return View("TaskList", dto);
            }
        }

        [HttpPost]
        public ActionResult DoIdentify()
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            // clear something
            dto.TaskId = 0;
            dto.OutletCn = dto.OutletName = null;
            HttpContext.Session.SetObjectAsJson("WxIdentify", dto);                
            
            return View("Identify", dto);
        }

        [HttpPost]
        public ActionResult SelectTask(int taskId, int outletId)
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            dto.TaskId = taskId;
            SelectOutlet(outletId, dto);
            return View("Identify", dto);
        }


        [HttpPost]
        public ActionResult VerifyOutlet(int taskId, string password)
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            if (dto != null && !string.IsNullOrEmpty(password) && dto.OutletPassword == password)
            {
                _weixinAppService.SetIdentifyTime(taskId);
                return View("Information2", dto);
            }
            else
            {
                ModelState.AddModelError("", "网点密码不符，请重新输入");
                return View("Identify", dto);
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel vm)
        {
            WxIdentifyDto dto = _weixinAppService.Login(vm.WorkerCn, vm.Password, vm.DeviceId);
            if (!string.IsNullOrEmpty(dto.ErrorMessage))
            {
                ModelState.AddModelError("", dto.ErrorMessage);
                return View(vm);
            }

            HttpContext.Session.SetObjectAsJson("WxIdentify", dto);
            return Redirect(vm.ReturnUrl);
        }


        [HttpPost]
        public ActionResult SelectOutlet(string outletCn)
        {
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            if (string.IsNullOrWhiteSpace(outletCn))
            {
                ModelState.AddModelError("", "需要输入网点编号");
                return View("Identify", dto);
            }

            var outlet = _outletCache.GetList().FindLast(x => x.Cn == outletCn);
            if (outlet != null)
            {
                SelectOutlet(outlet.Id, dto);
                return View("Identify", dto);
            }
            else
            {
                ModelState.AddModelError("", "此编号没有对应的网点");
                return View("Identify", dto);
            }
        }

        private void SelectOutlet(int outletId, WxIdentifyDto dto)
        {
            var outlet = _outletCache[outletId];

            dto.OutletCn = outlet.Cn;
            dto.OutletName = outlet.Name;
            dto.OutletPassword = outlet.Password;
            dto.OutletCipertext = outlet.Ciphertext;
            HttpContext.Session.SetObjectAsJson("WxIdentify", dto);                
        }
    }
}