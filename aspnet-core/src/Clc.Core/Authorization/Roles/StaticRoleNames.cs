namespace Clc.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            // 总部角色
            public const string Admin = "Admin";
            public const string Hrm = "Hrm";            // 人事数据维护
            public const string Hrq = "Hrq";            // 人事数据查询
            public const string Query = "Query";        // 查询

            // Worker Roles，用户名: Worker+Cn, 密码：User.WorkerUserDefaultPassword
            public const string Captain = "Captain";    // 队长
            public const string Aux = "Aux";            // 调度辅助
            public const string Monitor = "Monitor";     // 监控员
            public const string Article = "Article";      // 仅领物
            public const string Box = "Box";               // 仅领箱
            public const string ArticleAndBox = "ArticleAndBox";   // 领物和领箱
        }
    }
}
