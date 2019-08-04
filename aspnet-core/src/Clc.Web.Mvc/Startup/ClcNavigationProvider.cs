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
                // Types
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Types, new FixedLocalizableString("类型设置"), icon: "fa fa-list", requiredPermissionName: PermissionNames.Pages_Types)
                    .AddItem(new MenuItemDefinition("Type_AffairTypes", new FixedLocalizableString("内务类型"), url: "AffairTypes"))
                    .AddItem(new MenuItemDefinition("Type_ArticleTypes", new FixedLocalizableString("物品类型"), url: "ArticleTypes"))
                    .AddItem(new MenuItemDefinition("Type_Posts", new FixedLocalizableString("岗位"), url: "Posts"))
                    .AddItem(new MenuItemDefinition("Type_RouteTypes", new FixedLocalizableString("线路类型"), url: "RouteTypes"))
                    .AddItem(new MenuItemDefinition("Type_TaskTypes", new FixedLocalizableString("押运任务类型"), url: "TaskTypes"))
                    .AddItem(new MenuItemDefinition("Type_WorkRoles", new FixedLocalizableString("工作角色"), url: "WorkRoles"))
               // Fields
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Fields, new FixedLocalizableString("场地及资源"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Pages_Fields)
                    .AddItem(new MenuItemDefinition("Field_Depots", new FixedLocalizableString(title), url: "Depots"))
                    .AddItem(new MenuItemDefinition("Field_Workplaces", new FixedLocalizableString("内务地点"), url: "Workplaces"))
                    .AddItem(new MenuItemDefinition("Field_Workers", new FixedLocalizableString("工作人员"), url: "Workers"))
                    .AddItem(new MenuItemDefinition("Field_Vehicles", new FixedLocalizableString("车辆"), url: "Vehicles"))
                    .AddItem(new MenuItemDefinition("Field_Articles", new FixedLocalizableString("物品"), url: "Articles"))
                // Clients
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Clients, new FixedLocalizableString("客户"), icon: "fa fa-th-large", requiredPermissionName: PermissionNames.Pages_Clients)
                    .AddItem(new MenuItemDefinition("Client_Customers", new FixedLocalizableString("客户"), url: "Customers"))
                    .AddItem(new MenuItemDefinition("Client_Outlets", new FixedLocalizableString("网点"), url: "Outlets"))
                    .AddItem(new MenuItemDefinition("Client_Boxes", new FixedLocalizableString("尾箱"), url: "Boxes"))
                 // Hrm
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Hrm, new FixedLocalizableString("档案管理"), icon: "fa fa-file", requiredPermissionName: PermissionNames.Pages_Hrm)
                    .AddItem(new MenuItemDefinition("Hrm_Workers", new FixedLocalizableString("人员档案"), url: "WorkerFiles"))
                    .AddItem(new MenuItemDefinition("Hrm_Documents", new FixedLocalizableString("文件档案"), url: "Documents"))
                // Hrq
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Hrq, new FixedLocalizableString("档案管理"), icon: "fa fa-file", requiredPermissionName: PermissionNames.Pages_Hrq)
                    .AddItem(new MenuItemDefinition("Hrm_Workers", new FixedLocalizableString("人员档案"), url: "WorkerFiles"))
                    .AddItem(new MenuItemDefinition("Hrm_Documents", new FixedLocalizableString("文件档案"), url: "Documents"))
               // Query
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Query, new FixedLocalizableString("配置"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Pages_Query)
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                    .AddItem(new MenuItemDefinition("Dispatcher_DaySettlesQuery", new FixedLocalizableString("日结查询"), url: "DaySettles/DaySettlesQuery"))
               // PlaceC
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_PlaceC, new FixedLocalizableString("中心监控"), icon: "fa fa-envelope", requiredPermissionName: PermissionNames.Pages_PlaceC)
 
                // PreArrange
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_PreArrange, new FixedLocalizableString("预排"), icon: "fa fa-list", requiredPermissionName: PermissionNames.Pages_PreArrange)
                    .AddItem(new MenuItemDefinition("PreArrange_Routes", new FixedLocalizableString("预排线路"), url: "PreRoutes"))
                    .AddItem(new MenuItemDefinition("PreArrange_Workers", new FixedLocalizableString("预排人员"), url: "PreWorkers"))
                    .AddItem(new MenuItemDefinition("PreArrange_VehicleWorkers", new FixedLocalizableString("应急交接人员"), url: "VehicleWorkers"))
                // Arrange
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Arrange, new FixedLocalizableString("当日任务"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_Arrange)
                    .AddItem(new MenuItemDefinition("Arrange_Affairs", new FixedLocalizableString("内部任务"), url: "Affairs"))
                    .AddItem(new MenuItemDefinition("Arrange_Routes", new FixedLocalizableString("线路任务"), url: "Routes"))
                    .AddItem(new MenuItemDefinition("Dispatcher_DaySettle", new FixedLocalizableString("日结"), url: "DaySettles"))
                // Statistic
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Statistic, new FixedLocalizableString("统计查询"), icon: "fa fa-file", requiredPermissionName: PermissionNames.Pages_Statistic)
                    .AddItem(new MenuItemDefinition("Statistic_Affairs", new FixedLocalizableString("内部任务查询"), url: "Routes/RouteTasksStat"))
                    .AddItem(new MenuItemDefinition("Statistic_Routes", new FixedLocalizableString("线路任务查询"), url: "Routes/RouteTasksStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteWorkersStat", new FixedLocalizableString("线路工作量统计"), url: "Routes/RouteWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_VtAffairWorkersStat", new FixedLocalizableString("金库工作量统计"), url: "VtAffairs/VtAffairWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_WhAffairWorkersStat", new FixedLocalizableString("库房工作量统计"), url: "WhAffairs/WhAffairWorkersStat"))
                    .AddItem(new MenuItemDefinition("Dispatcher_SigninsStat", new FixedLocalizableString("签到统计"), url: "Signins/SigninsStat"))
                // Aux
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Aux, new FixedLocalizableString("辅助调度"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_Aux)
                    .AddItem(new MenuItemDefinition("AuxDispatcher_TaskOrder", new FixedLocalizableString("临时任务处理"), url: "TaskOrder"))
                    .AddItem(new MenuItemDefinition("AuxDispatcher_RouteMonitor", new FixedLocalizableString("线路监控"), url: "RouteMonitor"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                    .AddItem(new MenuItemDefinition("Dispatcher_RouteTasksQuery", new FixedLocalizableString("线路任务查询"), url: "Routes/RouteTasksQuery"))

                // PlaceA
                ).AddItem(new MenuItemDefinition("KeeperPages_Inquire", new FixedLocalizableString("库房查询"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_PlaceA)
                    .AddItem(new MenuItemDefinition("Keeper_WhAffairsCheck", new FixedLocalizableString("库房人员自签"), url: "Keeper/WhAffairsCheck"))
                    .AddItem(new MenuItemDefinition("Keeper_Articles", new FixedLocalizableString("物品清单"), url: "Keeper/Index"))
                    .AddItem(new MenuItemDefinition("Keeper_ArticleRecords", new FixedLocalizableString("物品领用记录查询"), url: "Keeper/ArticleRecords"))
                
                // PlaceB
                ).AddItem(new MenuItemDefinition("KeeperPages_Operate", new FixedLocalizableString("库房操作"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_PlaceB)
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
