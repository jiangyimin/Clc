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

            context.CreatePermission(PermissionNames.Pages_Setup, L("Setup"));
            context.CreatePermission(PermissionNames.Pages_Types, L("Types"));
            context.CreatePermission(PermissionNames.Pages_Fields, L("Fields"));
            context.CreatePermission(PermissionNames.Pages_Customers, L("Customers"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ClcConsts.LocalizationSourceName);
        }
    }
}
