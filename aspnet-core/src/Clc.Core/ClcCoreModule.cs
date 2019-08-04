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
using Clc.Fields.Cache;
using Clc.Clients.Cache;

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
            Configuration.Caching.Configure("CachedWorker", cache =>
            {
                cache.DefaultSlidingExpireTime = System.TimeSpan.FromMinutes(10);
            });
            
            // Cache for Types
            IocManager.Register<IAffairTypeCache, AffairTypeCache>();
            IocManager.Register<IArticleTypeCache, ArticleTypeCache>();
            IocManager.Register<IPostCache, PostCache>();
            IocManager.Register<IRouteTypeCache, RouteTypeCache>();
            IocManager.Register<ITaskTypeCache, TaskTypeCache>();
            IocManager.Register<IWorkRoleCache, WorkRoleCache>();
 
            // Cache for Fields
            IocManager.Register<IDepotCache, DepotCache>();
            IocManager.Register<IWorkplaceCache, WorkplaceCache>();
            IocManager.Register<IWorkerCache, WorkerCache>();
            IocManager.Register<IVehicleCache, VehicleCache>();
            IocManager.Register<IArticleCache, ArticleCache>();

            // Cache for Clients
            IocManager.Register<ICustomerCache, CustomerCache>();
            IocManager.Register<IOutletCache, OutletCache>();
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
