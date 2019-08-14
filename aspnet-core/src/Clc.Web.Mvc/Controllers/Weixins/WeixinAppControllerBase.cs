using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Clc.Configuration;
using Clc.Controllers;
using Clc.RealTime;
using Clc.Web.MessageHandlers;
using Clc.Weixin;

using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin.Work;
using Senparc.Weixin.Work.Entities;

namespace Clc.Web.Controllers
{

    // Base
    [IgnoreAntiforgeryToken]
    public class WorkAppControllerBase : ClcControllerBase
    {
        public IWeixinAppService WeixinAppService {get; set; }
        public IHubContext<MyChatHub> HubContext { get; set; }
        private readonly string _token;
        private readonly string _encodingAESKey;

        private readonly string _corpId;

        public WorkAppControllerBase(IHostingEnvironment env, string app)
        {
            var appConfiguration = env.GetAppConfiguration();
            _token = appConfiguration[string.Format("SenparcWeixinSetting:{0}:Token", app)];
            _encodingAESKey = appConfiguration[string.Format("SenparcWeixinSetting:{0}:EncodingAESKey", app)];
            _corpId = appConfiguration["SenparcWeixinSetting:CorpId"];
        }

        /// <summary> 
        /// Get index （消息接受地址验证）
        /// </summary> 
        [HttpGet] 
        [ActionName("Index")] 
        public ActionResult Get(string msg_signature = "", string timestamp = "", string nonce = "", string echostr = "")
        {
            var verifyUrl = Signature.VerifyURL(_token, _encodingAESKey, _corpId, msg_signature, timestamp, nonce, echostr); 
            if (verifyUrl != null) 
                return Content(verifyUrl); 
            else 
                return Content("如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url。但请注意保持Token一致。"); 
        } 
 
        /// <summary> 
        ///  
        /// </summary> 
        /// <param name="postModel"></param> 
        /// <returns></returns> 
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        { 
            postModel.Token = _token; 
            postModel.EncodingAESKey = _encodingAESKey; 
            postModel.CorpId = _corpId;

            var maxRecordCount = 10;
            try
            {
                var messageHandler = new WorkAppMessageHandler(WeixinAppService, HubContext, Request.GetRequestMemoryStream(), postModel, maxRecordCount);
                messageHandler.Execute();
                return Content(messageHandler.FinalResponseDocument.ToString());
            }
            catch (Exception ex) 
            {
                Logger.Error($"微信MessageHandle错误 {ex.Message}");
                return new EmptyResult();
            }
        } 
    }
}