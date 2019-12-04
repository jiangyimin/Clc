using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Castle.Logging.Log4Net;
using Clc.Authentication.JwtBearer;
using Clc.Configuration;
using Clc.Identity;
using Clc.RealTime;
using Clc.Web.Resources;
using Senparc.CO2NET;
using Senparc.Weixin;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.Entities;
using Microsoft.AspNetCore.Http;

namespace Clc.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );

            services.AddAuthentication()
                .AddCookie(options => {
                    options.LoginPath = new PathString("/WeixinAccount/Login/"); 
                });

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddScoped<IWebResourceManager, WebResourceManager>();
            services.AddSession(o => o.IdleTimeout = TimeSpan.FromSeconds(60));

            services.AddSignalR();

            // Weixin
            services.AddSenparcGlobalServices(_appConfiguration)            // Senparc.CO2NET
                    .AddSenparcWeixinServices(_appConfiguration);           // Senparc.Weixin

            // Configure Abp and Dependency Injection
            return services.AddAbp<ClcWebMvcModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, 
            IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            app.UseAbp(); // Initializes ABP framework.

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
                routes.MapHub<MyChatHub>("/signalr-myChatHub");
            });

            app.UseSession();       // must befor UseMvc.

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Start CO2NET register
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value)
                .UseSenparcGlobal(false, null);
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);

        }
    }
}
