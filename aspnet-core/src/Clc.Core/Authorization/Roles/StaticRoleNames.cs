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

            // 带非实际登录用户的角色（同名，密码由AppSettingProvider中提供）
            public const string Captain = "Captain";    // 队长
            public const string Aux = "Aux";            // 调度辅助
            public const string PlaceC = "PlaceC";      // 中央控制
            public const string PlaceA = "PlaceA";      // 仅领物
            public const string PlaceB = "PlaceB";      // 仅领箱
            public const string PlaceAB = "PlaceAB";   // 领物和领箱

        }
    }
}
