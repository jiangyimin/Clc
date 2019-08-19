using Abp.Dependency;
using Clc.Configuration;
using Microsoft.AspNetCore.Hosting;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;

namespace Clc.Web.MessageHandlers
{
    public class WeixinUtils
    {
        public static void SendMessage(string app, string toUser, string message)
        {
            var env = IocManager.Instance.Resolve<IHostingEnvironment>();
            var appConfiguration = env.GetAppConfiguration();
            string corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            string secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", app)];
            string agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", app)];
            var accessToken = AccessTokenContainer.GetToken(corpId, secret);
            MassApi.SendTextAsync(accessToken, agentId, message, toUser);   

        } 

        public static void SendTextCard(string app, string toUser, string title, string desc)
        {
            var env = IocManager.Instance.Resolve<IHostingEnvironment>();
            var appConfiguration = env.GetAppConfiguration();
            string corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
            string secret = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Secret", app)];
            string agentId = appConfiguration[string.Format("SenparcWeixinSetting:{0}:AgentId", app)];
            var accessToken = AccessTokenContainer.GetToken(corpId, secret);

            
            MassApi.SendTextCardAsync(accessToken, agentId, title, desc, "work.weixin.qq.com", null, toUser);   
        } 

    }
}