using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;
using Clc.Configuration;
using Clc.Localization;
using Clc.MultiTenancy;
using Clc.Timing;
using Clc.Types.Cache;

namespace Clc
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class ClcCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabled = false;
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            ClcLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = ClcConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ClcCoreModule).GetAssembly());

            //为特定的缓存配置有效期
            Configuration.Caching.Configure("CachedKeeper", cache =>
            {
                cache.DefaultSlidingExpireTime = System.TimeSpan.FromMinutes(10);
            });
            
            IocManager.Register<IWorkerTypeCache, WorkerTypeCache>();
            IocManager.Register<ITaskTypeCache, TaskTypeCache>();
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
