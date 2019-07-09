namespace Clc.Authorization
{
    public static class PermissionNames
    {
        // Host管理
        public const string Pages_Host = "Pages.Host";
        
        // 系统管理工作
        public const string Pages_Setup = "Pages.Setup";                // 1. 系统设置管理
        public const string Pages_Types = "Pages.Types";                // 2. 各种类型

        public const string Pages_Fields = "Pages.Fields";              // 3. 场地与资源
        public const string Pages_Files = "Pages.Files";                // 4. 档案

        public const string Pages_Customers = "Pages.Customers";        // 5. 客户数据


        // 调度工作
        public const string Pages_PreArrange = "Page.PreArrange";       // 6. 任务预排
        public const string Pages_TodayArrange = "Pages.TodayArrange";  // 7. 当天任务运行
        public const string Pages_AuxDispatch = "Page.AuxDispatch";     // 8. 辅助调度工作

        // 库管工作
    }
}
