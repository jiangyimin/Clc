using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
    public class WxApp01AccountController : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private readonly string _secret;
        private readonly string _agentId;
        private readonly string _corpId;

        public WxApp01AccountController(IHostingEnvironment env)
        {
            var appConfiguration = env.GetAppConfiguration();
            _secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", "App01")];
            _agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", "App01")];
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
        }

        public ActionResult Login(string returnUrl, string code) 
        {
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(OAuth2Api.GetCode(_corpId, AbsoluteUri(), "STATE", _agentId));
            }

            var vm = new LoginViewModel() {
                ReturnUrl = returnUrl
            };

            var accessToken = AccessTokenContainer.GetToken(_corpId, _secret);           
            try {
                GetUserInfoResult userInfo = OAuth2Api.GetUserId(accessToken, code);
                vm.WorkerCn = userInfo.UserId;
            }
            catch {
                Logger.Info(string.Format("accessKey={0} code={1}, corpId={2}, secret={3}", accessToken, code, _corpId, _secret));
            }

            return View(vm);
        }
        
        public async Task Logout()  // Task<ActionResult> Logout()
        {
            //注销登录的用户，相当于ASP.NET中的FormsAuthentication.SignOut  
            await HttpContext.SignOutAsync("WxApp01");  // CookieAuthenticationDefaults.AuthenticationScheme);
            // return RedirectToAction("InList", "Weixin");
        }
        
        [HttpPost]
        public ActionResult Login(LoginViewModel vm)
        {
            var worker = WorkManager.GetWorkerByCn(vm.WorkerCn);
            if (worker == null || worker.Password != vm.Password)
            {
                ModelState.AddModelError("", "用户名或密码错误");
                return View(vm);
            }

            var claims = new[] 
            { 
                new Claim("Cn", vm.WorkerCn),
                new Claim("App", "App01"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);

            Task.Run(async () =>
            {
                //登录用户，相当于ASP.NET中的FormsAuthentication.SetAuthCookie
                await HttpContext.SignInAsync(
                    "WxApp01",      //CookieAuthenticationDefaults.AuthenticationScheme, 
                    user,
                    new AuthenticationProperties() {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(ClcConsts.CookieAuthExpireTime)
                    });
            }).Wait();

            string action = GetActionOfReturnUrl(vm.ReturnUrl);
            return RedirectToAction(action, "Weixin");
        }

        #region Utils

        private string GetActionOfReturnUrl(string url)
        {
            int i = url.LastIndexOf('/');
            return url.Substring(i + 1, url.Length - i - 1);
        }

        #endregion
    }
}