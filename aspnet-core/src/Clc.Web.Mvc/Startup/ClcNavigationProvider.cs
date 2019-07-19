using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using Clc.Authorization;
using Clc.Configuration;

namespace Clc.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class ClcNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            ISettingManager settingManager =  IocManager.Instance.Resolve<ISettingManager>();
            string title = settingManager.GetSettingValue(AppSettingNames.VI.DepotTitleName);

            context.Manager.MainMenu
                // Host
                .AddItem(new MenuItemDefinition(PermissionNames.Pages_Host, new FixedLocalizableString("配置"), icon: "fa fa-home", requiredPermissionName: PermissionNames.Pages_Host)
                    .AddItem(new MenuItemDefinition("Host_Tenants", new FixedLocalizableString("租户"), url: "Tenants"))
                    .AddItem(new MenuItemDefinition("Admin_Roles", new FixedLocalizableString("用户角色"), url: "Roles"))

                // Setup
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Setup, new FixedLocalizableString("配置"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Pages_Setup)
                    .AddItem(new MenuItemDefinition("Admin_Settings", new FixedLocalizableString("全局设置"), url: "TenantSettings"))
                    .AddItem(new MenuItemDefinition("Admin_Users", new FixedLocalizableString("用户"), url: "Users"))
                    .AddItem(new MenuItemDefinition("Admin_LoginAttempts", new FixedLocalizableString("登录日志"), url: "LoginAttempts"))
                    .AddItem(new MenuItemDefinition("Admin_ErrorLoginAttempts", new FixedLocalizableString("错误登录日志"), url: "ErrorLoginAttempts"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                    .AddItem(new MenuItemDefinition("Dispatcher_DaySettlesQuery", new FixedLocalizableString("日结查询"), url: "DaySettles/DaySettlesQuery"))
                // Types
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Types, new FixedLocalizableString("类型设置"), icon: "fa fa-list", requiredPermissionName: PermissionNames.Pages_Types)
                    .AddItem(new MenuItemDefinition("Type_WorkerTypes", new FixedLocalizableString("人员类型"), url: "WorkerTypes"))
                    .AddItem(new MenuItemDefinition("Type_WorkRoles", new FixedLocalizableString("工作角色"), url: "WorkRoles"))
                    .AddItem(new MenuItemDefinition("Type_TaskTypes", new FixedLocalizableString("任务类型"), url: "TaskTypes"))
                    .AddItem(new MenuItemDefinition("Type_ArticleTypes", new FixedLocalizableString("物品类型"), url: "ArticleTypes"))
                    .AddItem(new MenuItemDefinition("Type_RouteTypes", new FixedLocalizableString("线路类型"), url: "RouteTypes"))
                    .AddItem(new MenuItemDefinition("Type_VaultTypes", new FixedLocalizableString("金库操作类型"), url: "VaultTypes"))
                    .AddItem(new MenuItemDefinition("Type_VaultRoles", new FixedLocalizableString("金库操作角色"), url: "VaultRoles"))
                // Fields
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Fields, new FixedLocalizableString("场地及资源"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Pages_Fields)
                    .AddItem(new MenuItemDefinition("Admin_DepotSignins", new FixedLocalizableString("签到时段"), url: "DepotSignins"))
                    .AddItem(new MenuItemDefinition("Field_Depots", new FixedLocalizableString(title), url: "Depots"))
                    .AddItem(new MenuItemDefinition("Admin_Warehouses", new FixedLocalizableString("库房"), url: "Warehouses"))
                    .AddItem(new MenuItemDefinition("Admin_Vaults", new FixedLocalizableString("金库"), url: "Vaults"))
                    .AddItem(new MenuItemDefinition("Field_Workers", new FixedLocalizableString("工作人员"), url: "Workers"))
                    .AddItem(new MenuItemDefinition("Admin_Vehicles", new FixedLocalizableString("车辆"), url: "Vehicles"))
                    .AddItem(new MenuItemDefinition("Admin_Articles", new FixedLocalizableString("物品"), url: "Articles"))
                    .AddItem(new MenuItemDefinition("Admin_Managers", new FixedLocalizableString("公司管理人员"), url: "Managers"))
                // Files
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Files, new FixedLocalizableString("档案管理"), icon: "fa fa-file", requiredPermissionName: PermissionNames.Pages_Files)
                    .AddItem(new MenuItemDefinition("Files_Workers", new FixedLocalizableString("人员档案"), url: "WorkerFiles"))
                    .AddItem(new MenuItemDefinition("Files_Documents", new FixedLocalizableString("文件档案"), url: "Documents"))
                // Customers
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Customers, new FixedLocalizableString("客户"), icon: "fa fa-th-large", requiredPermissionName: PermissionNames.Pages_Customers)
                    .AddItem(new MenuItemDefinition("Admin_Outlets", new FixedLocalizableString("网点"), url: "Outlets"))

                // PreArrange
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_PreArrange, new FixedLocalizableString("预排"), icon: "fa fa-list", requiredPermissionName: PermissionNames.Pages_PreArrange)
                    .AddItem(new MenuItemDefinition("PreArrange_Routes", new FixedLocalizableString("预排线路"), url: "PreRoutes"))
                    .AddItem(new MenuItemDefinition("PreArrange_Workers", new FixedLocalizableString("预排人员"), url: "PreWorkers"))
                    .AddItem(new MenuItemDefinition("PreArrange_VehicleWorkers", new FixedLocalizableString("应急交接人员"), url: "VehicleWorkers"))
                // TodayArrange
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_TodayArrange, new FixedLocalizableString("当日任务"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_TodayArrange)
                    .AddItem(new MenuItemDefinition("TodayArrange_Warehouse", new FixedLocalizableString("库房任务"), url: "WarehouseTasks"))
                    .AddItem(new MenuItemDefinition("TodayArrange_Vault", new FixedLocalizableString("金库任务"), url: "VaultTasks"))
                    .AddItem(new MenuItemDefinition("TodayArrange_Routes", new FixedLocalizableString("线路任务"), url: "RouteTasks"))
                // AuxDispatcher
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_AuxDispatch, new FixedLocalizableString("辅助调度"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_AuxDispatch)
                    .AddItem(new MenuItemDefinition("AuxDispatcher_TaskOrder", new FixedLocalizableString("临时任务处理"), url: "TaskOrder"))
                    .AddItem(new MenuItemDefinition("AuxDispatcher_RouteMonitor", new FixedLocalizableString("线路监控"), url: "RouteMonitor"))
                
                ).AddItem(new MenuItemDefinition("DispatcherPages_Report", new FixedLocalizableString("日结统计"), icon: "fa fa-envelope") //, requiredPermissionName: PermissionNames.DispatcherPages)
                    .AddItem(new MenuItemDefinition("Dispatcher_DaySettle", new FixedLocalizableString("日结"), url: "DaySettles"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteTasksQuery", new FixedLocalizableString("线路任务查询"), url: "Routes/RouteTasksQuery"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteTasksStat", new FixedLocalizableString("线路任务统计"), url: "Routes/RouteTasksStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_SigninsStat", new FixedLocalizableString("签到统计"), url: "Signins/SigninsStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteWorkersStat", new FixedLocalizableString("线路工作量统计"), url: "Routes/RouteWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_VtAffairWorkersStat", new FixedLocalizableString("金库工作量统计"), url: "VtAffairs/VtAffairWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_WhAffairWorkersStat", new FixedLocalizableString("库房工作量统计"), url: "WhAffairs/WhAffairWorkersStat"))

                // Keeper
                ).AddItem(new MenuItemDefinition("KeeperPages_Inquire", new FixedLocalizableString("库房查询"), icon: "fa fa-th-list") //, requiredPermissionName: PermissionNames.KeeperPages)
                    .AddItem(new MenuItemDefinition("Keeper_WhAffairsCheck", new FixedLocalizableString("库房人员自签"), url: "Keeper/WhAffairsCheck"))
                    .AddItem(new MenuItemDefinition("Keeper_Articles", new FixedLocalizableString("物品清单"), url: "Keeper/Index"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                ).AddItem(new MenuItemDefinition("KeeperPages_Operate", new FixedLocalizableString("库房操作"), icon: "fa fa-th-list") //, requiredPermissionName: PermissionNames.KeeperPages)
                    .AddItem(new MenuItemDefinition("Keeper_VtAffairsCheck", new FixedLocalizableString("金库人员核实"), url: "Keeper/VtAffairsCheck"))
                    .AddItem(new MenuItemDefinition("Keeper_RoutesArticle", new FixedLocalizableString("押运任务处理"), url: "Keeper/RoutesArticle"))
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ClcConsts.LocalizationSourceName);
        }
    }
}
