using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Clc.Authorization
{
    public class ClcAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Host, L("Host"), multiTenancySides: MultiTenancySides.Host);

            // Admin
            context.CreatePermission(PermissionNames.Pages_Setup, L("Setup"));
            context.CreatePermission(PermissionNames.Pages_Types, L("Types"));
            context.CreatePermission(PermissionNames.Pages_Fields, L("Fields"));
            context.CreatePermission(PermissionNames.Pages_Clients, L("Clients"));

            // Hrm
            context.CreatePermission(PermissionNames.Pages_Hrm, L("Hrm"));
            // Hrq
            // context.CreatePermission(PermissionNames.Pages_Hrq, L("Hrq"));
            // Qurey
            context.CreatePermission(PermissionNames.Pages_Query, L("Query"));
            // PlaceC
            context.CreatePermission(PermissionNames.Pages_Monitor, L("Monitor"));

            // Captain
            context.CreatePermission(PermissionNames.Pages_Arrange, L("Arrange"));
            context.CreatePermission(PermissionNames.Pages_Statistic, L("Statistic"));
            context.CreatePermission(PermissionNames.Pages_Aux, L("Aux"));

            // PlaceA
            context.CreatePermission(PermissionNames.Pages_Article, L("Article"));
            // PlaceB
            context.CreatePermission(PermissionNames.Pages_Box, L("Box"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ClcConsts.LocalizationSourceName);
        }
    }
}
