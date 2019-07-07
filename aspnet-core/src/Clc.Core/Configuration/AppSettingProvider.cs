using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace Clc.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            List<SettingDefinition> lst = new List<SettingDefinition>();
            lst.AddRange(GetVISettingDefinitions(context));
            lst.AddRange(GetConstSettingDefinitions(context));
            // return new[]
            // {
            //     new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            // };
            return lst;
        }

        private IEnumerable<SettingDefinition> GetVISettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
            {
                new SettingDefinition(
                    AppSettingNames.VI.CompanyName, 
                    "XX押运", 
                    new FixedLocalizableString("公司名"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.VI.CompanyImageName, 
                    "user.png", 
                    new FixedLocalizableString("公司图标名"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.VI.DepotTitleName, 
                    "中心", 
                    new FixedLocalizableString("物流中心外显名称"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                )
            };
        }
        private IEnumerable<SettingDefinition> GetConstSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
            {
                new SettingDefinition(
                    AppSettingNames.Const.UserDefaultPassword, 
                    "123456", 
                    new FixedLocalizableString("用户缺省密码"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.Const.RoleUserDefaultPassword, 
                    "RoleUserDefaultPassword", 
                    new FixedLocalizableString("角色用户缺省密码"),
                    scopes: SettingScopes.Tenant
                )
            };
        }
    }
}
