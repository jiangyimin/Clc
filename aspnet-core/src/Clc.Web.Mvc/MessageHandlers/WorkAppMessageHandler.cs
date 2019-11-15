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
            var result = _workManager.ClickEventMessageHandler(requestMessage.FromUserName, requestMessage.EventKey);

            if (result.Item1)
            {
                responseMessage.Content = "系统已受理 " + requestMessage.EventKey;
                if (result.Item2 != null) {
                    _context.Clients.All.SendAsync("getMessage", result.Item2 );
                }
            }
            else 
            {
                responseMessage.Content = result.Item2;
            }
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();

            var worker = _workManager.GetWorkerByCn(requestMessage.FromUserName);
            if (worker == null) {
                responseMessage.Content = "未登记在工作人员表中";
            }
            else 
            {   
                int depotId = _workManager.GetWorkerDepotId(worker.Id);
                if (_workManager.IsInDepotRadius(depotId, (float)requestMessage.Location_X, (float)requestMessage.Location_Y))
                    responseMessage.Content = _workManager.DoSignin(1, depotId, worker.Id);
                else 
                    responseMessage.Content = "你未在有效范围内";
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
