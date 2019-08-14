using System.IO;
using System.Threading.Tasks;
using Abp.Dependency;
using Clc.RealTime;
using Clc.Weixin;
using Microsoft.AspNetCore.SignalR;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;

namespace Clc.Web.MessageHandlers
{
    public class WorkAppMessageHandler : WorkMessageHandler<WorkAppMessageContext>
    {
        private IHubContext<MyChatHub> _context;
        public IWeixinAppService _weixinAppService;
        public WorkAppMessageHandler(IWeixinAppService weixinAppService, IHubContext<MyChatHub> context,
            Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            _weixinAppService = weixinAppService;;
            _context = context;
        }

        public override IWorkResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了消息：" + requestMessage.Content;
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            if (requestMessage.EventKey == "解屏") 
            {
                _context.Clients.All.SendAsync("getMessage", requestMessage.FromUserName + " unlockScreen");
                 responseMessage.Content = "你的解屏命令已发出";
            }
            else if (requestMessage.EventKey == "锁屏") 
            {
                _context.Clients.All.SendAsync("getMessage", requestMessage.FromUserName + " lockScreen");
                responseMessage.Content = "你的锁屏命令已发出";
            }
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            responseMessage.Content = string.Format("位置坐标 {0} - {1}", requestMessage.Location_X, requestMessage.Location_Y);
            return responseMessage;

        }

        public override IWorkResponseMessageBase DefaultResponseMessage(IWorkRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这是一条没有找到合适回复信息的默认消息。";
            return responseMessage;
         }
    }
}
