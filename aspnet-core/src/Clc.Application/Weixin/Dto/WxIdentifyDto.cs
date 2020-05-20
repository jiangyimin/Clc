using System;
using System.Collections.Generic;
using Clc.Fields;
using Clc.Routes;

namespace Clc.Weixin.Dto
{
    public class WeixinWorkerDto 
    {
        public string Cn { get; set; }

        public string Name { get; set; }
        public string AdditiveInfo { get; set; }
        public string Photo { get; set; }

    }
    public class WeixinVehicleDto 
    {
        public string Cn { get; set; }
        public string License { get; set; }
        public string Photo { get; set; }

    }

    public class WeixinTaskDto 
    {
        public int TaskId { get; set; }
        public string ArriveTime { get; set; }
        public string TaskType { get; set; }

        public int OutletId { get; set; }
        public string OutletCn { get; set; }
        public string OutletName { get; set; }

        public string IdentifyTime { get; set; }

        public string Remark { get; set; }

        public string Rated { get; set; }

        public string IdentifyInfo { get; set; }
        public int RouteId { get; set; }

        public WeixinTaskDto(int taskId, string arriveTime, string taskType, int outletId, string outletCn, string outletName, string identifyTime, string remark)
        {
            TaskId = taskId;
            ArriveTime = arriveTime;
            TaskType = taskType;
            OutletId = outletId;
            OutletCn = outletCn;
            OutletName = outletName;
            IdentifyTime = identifyTime;
            Remark = remark;
        }

        public WeixinTaskDto(RouteTask task)
        {
            TaskId = task.Id;
            ArriveTime = task.ArriveTime;
            TaskType = task.TaskType.Name;
            IdentifyTime = task.IdentifyTime?.ToString();
            Remark = task.Remark;
            RouteId = task.RouteId;
            Rated = task.Rated;
            IdentifyInfo = task.OutletIdentifyInfo;
        }

    }

    public class WxIdentifyDto
    {
        public List<WeixinWorkerDto> Workers { get; set; }
        public WeixinVehicleDto Vehicle { get; set; }

        // Route and Tasks
        public int DepotId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public List<WeixinTaskDto> Tasks { get; set; }
        public int TaskId { get; set; }
        public string OutletCn { get; set; }
        public string OutletName { get; set; }
        public string OutletPassword { get; set; }
        public string OutletCipertext { get; set; }

        // Options
        public bool AllowDoIdentify { get; set; }
        public string ErrorMessage { get; set; }

        public WxIdentifyDto()
        {
            Workers = new List<WeixinWorkerDto>();
            Tasks = new List<WeixinTaskDto>();
       }

   }
}