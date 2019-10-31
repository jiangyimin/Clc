using System.IO;
using Clc.RealTime;
using Clc.Works;
using Microsoft.AspNetCore.SignalR;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;

namespace Clc.Web.MessageHandlers
{
    public class WorkAppMessageHandler : WorkMessageHandler<WorkAppMessageContext>
    {
        private IHubContext<MyChatHub> _context;
        public WorkManager _workManager;
        public WorkAppMessageHandler(WorkManager workManager, IHubContext<MyChatHub> context,
            Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            _workManager = workManager;
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
                if (_workManager.IsWorkerRoleUser(requestMessage.FromUserName))
                {
                    _context.Clients.All.SendAsync("getMessage", requestMessage.FromUserName + " unlockScreen");
                    responseMessage.Content = "你的解屏命令已发出";
                }
                else 
                {
                    responseMessage.Content = "你不需要解屏";
                }
            }
            else if (requestMessage.EventKey == "锁屏") 
            {
                if (_workManager.IsWorkerRoleUser(requestMessage.FromUserName))
                {
                    _context.Clients.All.SendAsync("getMessage", requestMessage.FromUserName + " lockScreen");
                    responseMessage.Content = "你的锁屏命令已发出";
                }
                else 
                {
                    responseMessage.Content = "你不需要锁屏";
                }
            }
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            var worker = _workManager.GetWorkerByCn(requestMessage.FromUserName);
            if (worker == null) {
                responseMessage.Content = "非工作人员不需要签到";
            }
            else 
            {   
                int depotId = _workManager.GetWorkerDepotId(worker.Id);
                if (_workManager.IsInDepotRadius(depotId, (float)requestMessage.Location_X, (float)requestMessage.Location_Y))
                    responseMessage.Content = _workManager.DoSignin(1, depotId, worker.Id);
                else 
                    responseMessage.Content = "你未在中心范围内";
            }
            return responseMessage;
        }

        public override IWorkResponseMessageBase DefaultResponseMessage(IWorkRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";   //"这是一条没有找到合适回复信息的默认消息。";
            return responseMessage;
         }
    }
}
