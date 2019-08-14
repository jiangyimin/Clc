using Abp.Application.Services;

namespace Clc.Weixin
{
    public interface IWeixinAppService : IApplicationService
    {
        // WxIdentifyDto Login(int tenantId, string workerCn, string password, string deviceId);
 
        // // List<RouteTaskDto> GetTaskList(string workerId);

        // string Signin(int tenantId, string workerCn, double lon, double lat, double accuracy);

        // void SetIdentifyTime(int taskId, int routeId, int outletId);

        // void ResetDeviceId(int id);
        // void ResetAllDeviceId();

        string UnlockScreen(string fromUser);
        string LockScreen(string fromUser);

    }
}
