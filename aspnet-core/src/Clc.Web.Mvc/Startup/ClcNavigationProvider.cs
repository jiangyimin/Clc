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
            //ISettingManager settingManager =  IocManager.Instance.Resolve<ISettingManager>();
            //string title = settingManager.GetSettingValue(AppSettingNames.VI.DepotTitleName);

            context.Manager.MainMenu
                // Host
                .AddItem(new MenuItemDefinition(PermissionNames.Pages_Host, new FixedLocalizableString("配置"), icon: "fa fa-home", requiredPermissionName: PermissionNames.Pages_Host)
                    .AddItem(new MenuItemDefinition("Host_Tenants", new FixedLocalizableString("租户"), url: "Tenants"))
                    .AddItem(new MenuItemDefinition("Admin_Roles", new FixedLocalizableString("用户角色"), url: "Roles"))

                // Setup
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Setup, new FixedLocalizableString("配置"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Pages_Setup)
                    .AddItem(new MenuItemDefinition("Admin_Settings", new FixedLocalizableString("全局设置"), url: "TenantSettings"))
                    .AddItem(new MenuItemDefinition("Admin_Users", new FixedLocalizableString("用户"), url: "Users"))
                    .AddItem(new MenuItemDefinition("Admin_WorkerUsers", new FixedLocalizableString("工作人员用户"), url: "WorkerUsers"))
                // Types
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Types, new FixedLocalizableString("类型设置"), icon: "fa fa-list", requiredPermissionName: PermissionNames.Pages_Types)
                    .AddItem(new MenuItemDefinition("Type_ArticleTypes", new FixedLocalizableString("物品类型"), url: "ArticleTypes"))
                    .AddItem(new MenuItemDefinition("Type_Posts", new FixedLocalizableString("岗位"), url: "Posts"))
                    .AddItem(new MenuItemDefinition("Type_RouteTypes", new FixedLocalizableString("线路类型"), url: "RouteTypes"))
                    .AddItem(new MenuItemDefinition("Type_TaskTypes", new FixedLocalizableString("押运任务类型"), url: "TaskTypes"))
                    .AddItem(new MenuItemDefinition("Type_WorkRoles", new FixedLocalizableString("工作角色"), url: "WorkRoles"))
               // Fields
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Fields, new FixedLocalizableString("场地及资源"), icon: "fa fa-globe", requiredPermissionName: PermissionNames.Pages_Fields)
                    .AddItem(new MenuItemDefinition("Field_Depots", new FixedLocalizableString("运行中心"), url: "Depots"))
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
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Monitor, new FixedLocalizableString("中心监控"), icon: "fa fa-envelope", requiredPermissionName: PermissionNames.Pages_Monitor)
 
                // Arrange
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Arrange, new FixedLocalizableString("工作安排"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_Arrange)
                    .AddItem(new MenuItemDefinition("Arrange_PreRoutes", new FixedLocalizableString("预排线路"), url: "PreRoutes"))
                    .AddItem(new MenuItemDefinition("Arrange_Routes", new FixedLocalizableString("线路任务"), url: "Routes"))
                    .AddItem(new MenuItemDefinition("Arrange_Affairs", new FixedLocalizableString("内部任务"), url: "Affairs"))
                // 今日情况
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Arrange, new FixedLocalizableString("今日情况"), icon: "fa fa-file", requiredPermissionName: PermissionNames.Pages_Arrange)
                    .AddItem(new MenuItemDefinition("Arrange_Checkin", new FixedLocalizableString("签到"), url: "Today/Signins"))
                    .AddItem(new MenuItemDefinition("Arrange_Article", new FixedLocalizableString("物品"), url: "Today/ArticleList"))
                // Statistic
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Statistic, new FixedLocalizableString("统计查询"), icon: "fa fa-file", requiredPermissionName: PermissionNames.Pages_Statistic)
                // Aux
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Aux, new FixedLocalizableString("辅助调度"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_Aux)

                // Article
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Article, new FixedLocalizableString("库房操作"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_Article)
                    .AddItem(new MenuItemDefinition("Article_Lend", new FixedLocalizableString("领用"), url: "ArticleWork/Lend"))
                    .AddItem(new MenuItemDefinition("Article_Return", new FixedLocalizableString("归还"), url: "ArticleWork/Return"))
                    .AddItem(new MenuItemDefinition("Article_List", new FixedLocalizableString("物品清单"), url: "ArticleWork/List"))
                    .AddItem(new MenuItemDefinition("Article_Record", new FixedLocalizableString("物品记录查询"), url: "ArticleWork/RecordQuery"))
                
                // Box
                ).AddItem(new MenuItemDefinition(PermissionNames.Pages_Box, new FixedLocalizableString("金库尾箱操作"), icon: "fa fa-th-list", requiredPermissionName: PermissionNames.Pages_Box)
                    .AddItem(new MenuItemDefinition("Box_In", new FixedLocalizableString("入箱"), url: "BoxWork/InBox"))
                    .AddItem(new MenuItemDefinition("Box_Out", new FixedLocalizableString("出箱"), url: "BoxWork/OutBox"))
                    .AddItem(new MenuItemDefinition("Box_List", new FixedLocalizableString("尾箱清单"), url: "BoxWork/List"))
                    .AddItem(new MenuItemDefinition("Box_Record", new FixedLocalizableString("尾箱记录查询"), url: "BoxWork/RecordQuery"))
                 );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ClcConsts.LocalizationSourceName);
        }
    }
}
