using System;
using Abp.Domain.Repositories;
using Clc.Routes;
using Clc.Works;
using Clc.Weixin.Dto;
using Clc.Fields;
using Clc.Runtime.Cache;
using Clc.Runtime;
using Clc.DoorRecords.Dto;
using Clc.Extensions;
using Abp.Configuration;
using Clc.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace Clc.Weixin
{
    public class WeixinAppService : ClcAppServiceBase, IWeixinAppService
    {
        public WorkManager WorkManager { get; set; }

        // App01 and App02
        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<AskDoorRecord> _askDoorRepository;
        private readonly IRepository<EmergDoorRecord> _emergDoorRepository;

        // App03
        private readonly IRouteAppService _routeAppService;
        private readonly IOutletCache _outletCache;
        private readonly ITaskTypeCache _taskTypeCache;
        private readonly IVehicleCache _vehicleCache;

        public WeixinAppService(IRepository<Issue> issueRepository, IRepository<AskDoorRecord> askDoorRepository, IRepository<EmergDoorRecord> emergDoorRepository,
            IRouteAppService routeAppService,IOutletCache outletCache, ITaskTypeCache taskTypeCache, IVehicleCache vehicleCache)
        {
            _issueRepository = issueRepository;
            _askDoorRepository = askDoorRepository;
            _emergDoorRepository = emergDoorRepository;

            _routeAppService = routeAppService;
            _outletCache = outletCache;
            _taskTypeCache = taskTypeCache;
            _vehicleCache = vehicleCache;
        }

        #region App01

        public AskDoorDto GetAskDoorForApproval(int doorId) 
        {
            

            return null;
        }

        public void InsertDoorEmerg(int depotId, int workerId, int doorId, string content)
        {
            using(CurrentUnitOfWork.SetTenantId(1))
            {
            }

        }
        #endregion

        #region App02
        public WxIdentifyDto Login(string workerCn, string password, string deviceId)
        {
            WxIdentifyDto dto = new WxIdentifyDto();

            var worker = WorkManager.GetWorkerByCn(workerCn);
            if (worker == null || password != worker.Password) {
                dto.ErrorMessage = "用户名或密码错误";
                return dto;
            }

            // judge right routeRole and subworker
            var ret = FindRoute(worker, dto);
            if (ret.Item1 == null) {
                dto.ErrorMessage = "找不到合适的线路";
                return dto;
            }
            if (ret.Item2 == null) 
                dto.ErrorMessage = "需要辅助交接员";
            
            dto.AllowDoIdentify = AllowDoIdentify(dto.DepotId);
            return dto;
        }

        public void SetIdentifyEvent(int routeId, string outlet, string issuer)
        {
            _routeAppService.SetIdentifyEvent(routeId, outlet, issuer);
        }

        public void SetIdentifyTime(int taskId)
        {
            _routeAppService.SetIdentifyTime(taskId);
        }

        public void InsertRouteArriveEvent(int taskId, string address)
        {
            _routeAppService.InsertRouteArriveEvent(taskId, address);
         }

        private bool AllowDoIdentify(int depotId) 
        {
            var depot = WorkManager.GetDepot(depotId);
            var depots = SettingManager.GetSettingValueForTenant(AppSettingNames.Rule.EditWorkerDepots, 1);
            if (!string.IsNullOrEmpty(depots) && depots.Split('|', ',').Contains(depot.Name)) 
                return true;
            return false;
        }
        
        private (Route, Worker) FindRoute(Worker main, WxIdentifyDto dto)
        {
            var ret = _routeAppService.FindRouteForIdentify(main.DepotId, main.Id);
            if (ret.Item1 == null) return (null, null);
            if (ret.Item2 == 0) return (ret.Item1, null);
            
            var sub = WorkManager.GetWorker(ret.Item2);
            dto.Workers.Add(new WeixinWorkerDto() 
                { 
                    Cn = main.Cn, Name = main.Name, AdditiveInfo = main.AdditiveInfo, Photo = main.Photo == null ? null : Convert.ToBase64String(main.Photo)   
                });
            dto.Workers.Add(new WeixinWorkerDto()
                { 
                    Cn = sub.Cn, Name = sub.Name, AdditiveInfo = sub.AdditiveInfo, Photo = sub.Photo == null ? null : Convert.ToBase64String(sub.Photo)   
                });

            var v = _vehicleCache[ret.Item1.GetFactVehicleId()];
            dto.Vehicle = new WeixinVehicleDto() {Cn = v.Cn, License = v.License, Photo = v.Photo == null ? null : Convert.ToBase64String(v.Photo) };

            dto.DepotId = ret.Item1.DepotId;
            dto.RouteId = ret.Item1.Id;
            dto.RouteName = ret.Item1.RouteName;

            foreach (RouteTask task in ret.Item1.Tasks) 
            {
                var outlet = _outletCache[task.OutletId];
                var type = _taskTypeCache[task.TaskTypeId];
                string tm = task.IdentifyTime.HasValue ? task.IdentifyTime.Value.ToString("HH:mm") : "";
                dto.Tasks.Add(new WeixinTaskDto(
                    task.Id, task.ArriveTime, type.Name, outlet.Id, outlet.Cn, outlet.Name, tm, task.Remark));
            }
            return (ret.Item1, sub);
        }

        #endregion

        #region App04
        public List<WeixinTaskDto> GetTodayTasks(int outletId)
        {
            var l = new List<WeixinTaskDto>();
            foreach (var e in _routeAppService.GetTodayTasks(outletId))
                l.Add(new WeixinTaskDto(e));

            return l;           
        }

        public WxIdentifyDto GetLookupInfo(int taskId, int routeId)
        {
            var dto = new WxIdentifyDto();

            var ret = _routeAppService.GetRouteForLookup(routeId);
            var main = WorkManager.GetWorker(ret.Item1);
            var sub = WorkManager.GetWorker(ret.Item2);
            dto.Workers.Add(new WeixinWorkerDto() 
                { 
                    Cn = main.Cn, Name = main.Name, AdditiveInfo = main.AdditiveInfo, Photo = main.Photo == null ? null : Convert.ToBase64String(main.Photo)   
                });
            dto.Workers.Add(new WeixinWorkerDto()
                { 
                    Cn = sub.Cn, Name = sub.Name, AdditiveInfo = sub.AdditiveInfo, Photo = sub.Photo == null ? null : Convert.ToBase64String(sub.Photo)   
                });

            var v = _vehicleCache[ret.Item3];
            dto.Vehicle = new WeixinVehicleDto() {Cn = v.Cn, License = v.License, Photo = v.Photo == null ? null : Convert.ToBase64String(v.Photo) };

            return dto;
        }

        public void UpdateTaskRate(int taskId, int rated, string info)
        {
            _routeAppService.UpdateTaskRate(taskId, rated, info);
        }

        #endregion
    }
}