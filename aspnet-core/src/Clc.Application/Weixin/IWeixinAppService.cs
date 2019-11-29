using Abp.Application.Services;
using Clc.DoorRecords.Dto;
using Clc.Weixin.Dto;

namespace Clc.Weixin
{
    public interface IWeixinAppService : IApplicationService
    {
        #region App01
        void InsertDoorEmerg(int depotId, int workerId, int doorId, string content);

        AskDoorDto GetAskDoorForApproval(int doorId);
        #endregion

        #region App02
        WxIdentifyDto Login(string workerCn, string password, string deviceId);
 
        // List<RouteTaskDto> GetTaskList(string workerId);

        void SetIdentifyTime(int taskId);

        #endregion

    }
}
