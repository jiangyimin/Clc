using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;
using Clc.Authorization.Users;

namespace Clc.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            List<SettingDefinition> lst = new List<SettingDefinition>();
            lst.AddRange(GetVISettingDefinitions(context));
            lst.AddRange(GetConstSettingDefinitions(context));
            lst.AddRange(GetRuleSettingDefinitions(context));
            lst.AddRange(GetTimeRuleSettingDefinitions(context));
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
                    AppSettingNames.Const.IdentifyEmergencyPassword, 
                    User.UserDefaultPassword, 
                    new FixedLocalizableString("身份确认紧急密码"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.Const.WorkerRfidLength, 
                    "5", 
                    new FixedLocalizableString("员工RFID卡编码长度"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.Const.ArticleRfidLength, 
                    "3", 
                    new FixedLocalizableString("物品RFID卡编码长度"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.Const.BoxRfidLength, 
                    "8", 
                    new FixedLocalizableString("尾箱RFID卡编码长度"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                ),
            };
        }
        private IEnumerable<SettingDefinition> GetRuleSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
            {
                new SettingDefinition(
                    AppSettingNames.Rule.VerifyLogin, 
                    "false", 
                    new FixedLocalizableString("校验码登录"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.Rule.Radius, 
                    "900", 
                    new FixedLocalizableString("半径(米)"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.Rule.DoubleArticleRoles, 
                    "车长|持枪员", 
                    new FixedLocalizableString("双人领物角色"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.Rule.MinWorkersOnDuty, 
                    "2", 
                    new FixedLocalizableString("最少当班人数"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.Rule.EnableDynEmergPassword, 
                    "false", 
                    new FixedLocalizableString("动态应急开门密码"),
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients: true
                ),
                new SettingDefinition(
                    AppSettingNames.Rule.AltCheckinDepots, 
                    "调度", 
                    new FixedLocalizableString("代验入大队列表"),
                    scopes: SettingScopes.Tenant
                ),
            };
        }
        private IEnumerable<SettingDefinition> GetTimeRuleSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>
            {
                new SettingDefinition(
                    AppSettingNames.TimeRule.DaysChangeReadonly, 
                    "2", 
                    new FixedLocalizableString("再次申请开门间隔(秒)"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.TimeRule.RecheckInterval, 
                    "0",            // 0: 不需要重新验证
                    new FixedLocalizableString("重新验证间隔(分钟)"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.TimeRule.MinSigninInterval, 
                    "60", 
                    new FixedLocalizableString("最小签到间隔(分钟)"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.TimeRule.AskOpenInterval, 
                    "120", 
                    new FixedLocalizableString("再次申请开门间隔(秒)"),
                    scopes: SettingScopes.Tenant
                ),
                new SettingDefinition(
                    AppSettingNames.TimeRule.AskOpenPeriod, 
                    "60", 
                    new FixedLocalizableString("申请开门周期(秒)"),
                    scopes: SettingScopes.Tenant
                ),
            };
        }
    }
}
