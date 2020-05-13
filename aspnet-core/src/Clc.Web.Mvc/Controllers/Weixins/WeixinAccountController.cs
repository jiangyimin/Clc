using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Clc.Works;
using Clc.Configuration;
using Clc.Controllers;
using Clc.Web.Models.Weixin;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.OAuth2;
using Senparc.Weixin.Work.Containers;

namespace Clc.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class WeixinAccountController : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }

        private readonly IConfigurationRoot _appConfiguration;
        private readonly string _corpId;
        private string _secret;
        private string _agentId;
        
        static public readonly Dictionary<string, string> _appDict = new Dictionary<string, string>() {
            {"ApproveTempAskDoor", "App03"},
            {"ApproveEmergDoor", "App03"},
        };

        public WeixinAccountController(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _corpId = _appConfiguration["SenparcWeixinSetting:CorpId"];
        }

        public ActionResult Login(string returnUrl, string code) 
        {
            if (returnUrl == null) return null;
            // 根据 returnUrl 得到 AppName， 然后得到_secret和_agentId
            string appName = _appDict[GetActionOfUrl(returnUrl)];
            _secret = _appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", appName)];
            _agentId = _appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", appName)];

            if (string.IsNullOrEmpty(code))
            {
                // 备案
                return Redirect(OAuth2Api.GetCode(_corpId, AbsoluteUri(), "STATE", _agentId));
            }

            var vm = new LoginViewModel() {
                ReturnUrl = returnUrl
            };

            try {
                var accessToken = AccessTokenContainer.GetToken(_corpId, _secret);           
                GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);
                vm.WorkerCn = userInfo.UserId;
                vm.DeviceId = userInfo.DeviceId;
            }
            catch {
                Logger.Error("微信登录错误");
            }

            return View(vm);
        }
        
        public async Task Logout()  // Task<ActionResult> Logout()
        {
            //注销登录的用户，相当于ASP.NET中的FormsAuthentication.SignOut  
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // return RedirectToAction("Login", "WeixinAccount");
        }
        
        [HttpPost]
        public ActionResult Login(LoginViewModel vm)
        {
            int id = 0;
            string cn = null;
            if (vm.WorkerCn.Length >= 6) {
                var outlet = WorkManager.GetOutletByCn(vm.WorkerCn.Substring(0, 6));
                if (outlet == null || outlet.Password != vm.Password)
                {
                    ModelState.AddModelError("", "用户名或密码错误");
                    return View(vm);
                };
                id = outlet.Id;
                cn = vm.WorkerCn.Substring(0, 6);
            }
            else 
            {
                var worker = WorkManager.GetWorkerByCn(vm.WorkerCn);
                if (worker == null || worker.Password != vm.Password)
                {
                    ModelState.AddModelError("", "用户名或密码错误");
                    return View(vm);
                };
                id = worker.Id;
                cn = vm.WorkerCn;
            }
            var claims = new[] 
            { 
                new Claim("UserId", id.ToString()),
                new Claim("Cn", cn),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);

            Task.Run(async () =>
            {
                //登录用户，相当于ASP.NET中的FormsAuthentication.SetAuthCookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    user,
                    new AuthenticationProperties() {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(ClcConsts.CookieAuthExpireTime)
                    });
            }).Wait();

            string action = GetActionOfUrl(vm.ReturnUrl);
            return RedirectToAction(action, "Ww" + _appDict[action]);
            // return RedirectToPage(vm.ReturnUrl);
        }

        #region Utils

        private string GetActionOfUrl(string url)
        {
            int i = url.LastIndexOf('/');
            return url.Substring(i + 1, url.Length - i - 1);
        }

        #endregion
    }
}