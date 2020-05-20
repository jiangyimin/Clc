using System.Collections.Generic;
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
        void SetIdentifyEvent(int routeId, string outlet, string issuer);

        void InsertRouteArriveEvent(int taskId, string addresss);
        #endregion

        #region App04
        List<WeixinTaskDto> GetTodayTasks(int outletId);

        WxIdentifyDto GetLookupInfo(int taskId, int routeId);

        void UpdateTaskRate(int taskId, int rated, string info);

        #endregion
    }
}
