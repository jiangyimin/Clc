namespace Clc.Configuration
{
    public static class AppSettingNames
    {
        public const string UiTheme = "App.UiTheme";

        public static class VI
        {
            public const string CompanyName = "VI.CompanyName";
            public const string CompanyImageName = "VI.CompanyImageName";
            public const string DepotTitleName = "VI.DepotTitleName";            
        }

        public static class Const
        {
           public const string IdentifyEmergencyPassword = "Const.IdentifyEmergencyPassword";
           public const string CameraPassword = "Const.CameraPassword";
            public const string WorkerRfidLength = "Const.WorkerRfidLength";
            public const string ArticleRfidLength = "Const.ArticleRfidLength";
            public const string BoxRfidLength = "Const.BoxRfidLength";
        }
        public static class Rule
        {
            public const string LoginIpList = "Rule.LoginIpList";
            public const string VerifyLogin = "Rule.VerifyLogin";
            public const string Radius = "Rule.Radius";
            public const string DoubleArticleRoles = "Rule.DoubleArticleRoles";

            public const string MinWorkersOnDuty = "Rule.MinWorkersOnDuty";

            public const string EnableDynEmergPassword = "Rule.EnableDynEmergPassword";
            public const string AltCheckinDepots = "Rule.AltCheckinDepots";
        }

        public static class TimeRule
        {
            public const string DaysChangeReadonly = "TimeRule.DaysChangeReadonly";
            public const string RecheckInterval = "TimeRule.RecheckInterval";
            public const string ReturnDeadline = "TimeRule.ReturnDeadline";
            public const string AskOpenInterval = "TimeRule.AskOpenInterval";
            // public const string AskOpenPeriod = "TimeRule.AskOpenPeriod";  
        }

    }
}
