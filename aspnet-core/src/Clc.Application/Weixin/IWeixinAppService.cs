using Abp.Application.Services;
using Clc.Weixin.Dto;

namespace Clc.Weixin
{
    public interface IWeixinAppService : IApplicationService
    {
        WxIdentifyDto Login(string workerCn, string password, string deviceId);
 
        // List<RouteTaskDto> GetTaskList(string workerId);

        void SetIdentifyTime(int taskId);

    }
}
