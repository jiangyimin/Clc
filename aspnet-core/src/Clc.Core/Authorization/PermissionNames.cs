namespace Clc.Authorization
{
    public static class PermissionNames
    {
        // Host管理
        public const string Pages_Host = "Pages.Host";
        
        // Admin 的菜单夹
        public const string Pages_Setup = "Pages.Setup";                // Admin-系统设置管理
        public const string Pages_Types = "Pages.Types";                // Admin-各种类型
        public const string Pages_Fields = "Pages.Fields";              // Admin-场地与资源
        public const string Pages_Clients = "Pages.Clients";            // Admin-客户端数据

        // Hrm 的菜单夹
        public const string Pages_Hrm = "Pages.Hrm";                    // Hrm-人事数据维护
        // Hrq 的菜单夹
        public const string Pages_Hrq = "Pages.Hrq";                    // Hrq-人事数据查询

        // Query 的菜单夹
        public const string Pages_Query = "Pages.Query";                // Query-各类业务数据查询

        // PlaceC (中央的远程门禁控制)
        public const string Pages_Monitor = "Pages.Monitor";              // 含线路、物品、箱总监控

        // 上面是总部权限（对应菜单夹），下面的是分部权限（对应菜单夹）
        // Captain 的菜单夹
        public const string Pages_PreArrange = "Pages.PreArrange";      // Captain-任务预排
        public const string Pages_Arrange = "Pages.Arrange";            // Captain-当天任务安排
        public const string Pages_Statistic = "Pages.Statistic";        // Captain-统计和日结
        public const string Pages_Aux = "Page.Aux";                     // Captian（Aux）-辅助(含监看线路、物和箱)

        // PlaceA(物）的菜单夹
        public const string Pages_Article = "Pages.Article";            // 含查物

        // PlaceB(箱）的菜单夹
        public const string Pages_Box = "Pages.Box";                    // 含查箱
    }
}
