using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Weixin;
using Clc.Weixin.Dto;
using Clc.Extensions;
using Clc.Web.Models.Weixin;
using Clc.Runtime.Cache;
using Clc.RealTime;
using Clc.Works;
using Clc.VehicleRecords;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.AdvancedAPIs.OAuth2;
using Senparc.Weixin.Work.Helpers;
using System.Net;
using System.IO;
using System.Text;
using Clc.Web.Helpers;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class WwApp02Controller : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private readonly IHubContext<MyChatHub> _context;
        private readonly string _corpId;
        private readonly string _secret;
        private readonly string _agentId;

        private readonly IWeixinAppService _weixinAppService;
        private readonly IVehicleRecordAppService _recordAppService;
        private readonly IGasStationCache _gasStationCache;
        private readonly IOilTypeCache _oilTypeCache;
        private readonly IVehicleMTTypeCache _vehicleMTTypeCache;
        private readonly IVehicleCache _vehicleCache;

        private readonly IOutletCache _outletCache;

        public WwApp02Controller(
            IHostingEnvironment env, 
            IHubContext<MyChatHub> context, 
            IWeixinAppService weixinAppService,
            IVehicleRecordAppService recordAppService,
            IGasStationCache gasStationCache,
            IOilTypeCache oilTypeCache,
            IVehicleMTTypeCache vehicleMTTypeCache,
            IVehicleCache vehicleCache,
            IOutletCache outletCache)
        {
            var appConfiguration = env.GetAppConfiguration();
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App02")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App02")];

            _context = context;
            _weixinAppService = weixinAppService;
            _recordAppService = recordAppService;

            _gasStationCache = gasStationCache;
            _oilTypeCache = oilTypeCache;
            _vehicleMTTypeCache = vehicleMTTypeCache;
            _vehicleCache = vehicleCache;
            _outletCache = outletCache;
        }

        public ActionResult Identify(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                // return Redirect(OAuth2Api.GetCode(_corpId, AbsoluteUri(), "STATE", _agentId));
            }
            
            WxIdentifyDto dto = HttpContext.Session.GetObjectFromJson<WxIdentifyDto>("WxIdentify");
            
            if (dto == null)
            {
                LoginViewModel vm = new LoginViewModel() 
                {
                    ReturnUrl = AbsoluteUri()
                };
                try {
                    var accessToken = AccessTokenContainer.GetToken(_corpId, _secret);           
                    GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);
                    vm.WorkerCn = userInfo.UserId;
                    vm.DeviceId = userInfo.DeviceId;
                }
                catch {
                    Logger.Error("微信授权错误");
                }               
                return View("login", vm);
            }
            else
            {
                return View("TaskList", dto);
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
            var jsapiticket = JsApiTicketContainer.GetTicket(_corpId, _secret);
            ViewBag.appId = _corpId;
            ViewBag.noncestr = JSSDKHelper.GetNoncestr();
            ViewBag.timestamp = JSSDKHelper.GetTimestamp();
            ViewBag.signature = JSSDKHelper.GetSignature(jsapiticket, ViewBag.nonceStr, ViewBag.timestamp, AbsoluteUri());

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
                if (taskId != 0) {
                    _weixinAppService.SetIdentifyTime(taskId);
                    _context.Clients.All.SendAsync("getMessage", "keypoint " + string.Format("{0},{1}", dto.OutletName, dto.DepotId));
                }
                else {
                    string issuer = null;
                    foreach (var w in dto.Workers) {
                        issuer += $"{w.Cn} {w.Name},";
                    }
                    _weixinAppService.SetIdentifyEvent(dto.RouteId, $"{dto.OutletCn} {dto.OutletName}", issuer);
                }
                return View("Information2", dto);
            }
            else
            {
                ModelState.AddModelError("", "网点密码不符，请重新输入");
                return View("Identify", dto);
            }
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

        [HttpPost]
        public ActionResult SendLocation(int taskId, double lat, double lon)
        {
            var m = BaiduMapHelper.CoordinateToAddress(lat, lon);

            var addr = m.Result.AddressComponent.Street + m.Result.AddressComponent.Street_number;
            _weixinAppService.InsertRouteArriveEvent(taskId, addr);
            return Content(m.Result.AddressComponent.Street);
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

        #region Vehicle
        public ActionResult Oil(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(OAuth2Api.GetCode(_corpId, AbsoluteUri(), "STATE", _agentId));
            }

            string workerCn = null;
            try {
                var accessToken = AccessTokenContainer.GetToken(_corpId, _secret);           
                GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);
                workerCn = userInfo.UserId;
            }
            catch {
                Logger.Error("微信授权错误");
            }               

            if (workerCn == null) return Content("系统取不到你的微信标识号");
            //workerCn = "90005";
            var worker = WorkManager.GetWorkerByCn(workerCn);
            if (!worker.WorkRoleNames.Contains("司机")) return Content("需要司机角色");
            var depot = WorkManager.GetDepot(worker.DepotId);
            var vm = new OilViewModel();
            vm.WorkerId = worker.Id;

            foreach (var v in _vehicleCache.GetList().FindAll(x => x.DepotId == depot.Id))
                vm.Vehicles.Add(new ComboItemModel() { Id = v.Id, Name = v.Cn + v.License});
            foreach (var v in _gasStationCache.GetList().FindAll(x => string.IsNullOrEmpty(x.DepotList) || x.DepotList.Contains(depot.Name)))
                vm.GasStations.Add(new ComboItemModel() { Id = v.Id, Name = v.Name});
            foreach (var t in _oilTypeCache.GetList())
                vm.OilTypes.Add(new ComboItemModel() { Id = t.Id, Name = t.Name});
            
            return View(vm);
        }

        public ActionResult VehicleMT(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(OAuth2Api.GetCode(_corpId, AbsoluteUri(), "STATE", _agentId));
            }

            string workerCn = null;
            try {
                var accessToken = AccessTokenContainer.GetToken(_corpId, _secret);           
                GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);
                workerCn = userInfo.UserId;
            }
            catch {
                Logger.Error("微信授权错误");
            }               

            if (workerCn == null) return Content("系统取不到你的微信标识号");
            //workerCn = "90005";

            var worker = WorkManager.GetWorkerByCn(workerCn);
            if (!worker.WorkRoleNames.Contains("司机")) return Content("需要司机角色");
            var depot = WorkManager.GetDepot(worker.DepotId);
            var vm = new VehicleMTViewModel();
            vm.WorkerId = worker.Id;

            foreach (var v in _vehicleCache.GetList().FindAll(x => x.DepotId == depot.Id))
                vm.Vehicles.Add(new ComboItemModel() { Id = v.Id, Name = v.Cn + v.License});
            foreach (var t in _vehicleMTTypeCache.GetList())
                vm.VehicleMTTypes.Add(new ComboItemModel() { Id = t.Id, Name = t.Name});
            
            return View(vm);
        }

        [HttpPost]
        public ActionResult DoOil(OilViewModel vm)
        {
            _recordAppService.InsertOilRecord(vm.WorkerId, vm.VehicleId, vm.GasStationId, vm.OilTypeId, vm.Quantity, vm.Price, vm.Mileage, vm.Remark);
            return RedirectToAction("WeixinNotify", "Error", new { Message = "添加成功" });
        }

        [HttpPost]
        public ActionResult DoVehicleMT(VehicleMTViewModel vm)
        {
            _recordAppService.InsertVehicleMTRecord(vm.WorkerId, vm.VehicleId, vm.VehicleMTTypeId, vm.MTDate, vm.Content, vm.Price, vm.Remark);
            return RedirectToAction("WeixinNotify", "Error", new { Message = "添加成功" });
        }
        #endregion
    }
}