using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Routes.Dto;
using Clc.PreRoutes;
using Clc.Runtime.Cache;
using Clc.Works;
using Clc.Runtime;
using Clc.Types;
using Clc.Extensions;
using Clc.Weixin.Dto;

namespace Clc.Routes
{
    [AbpAuthorize]
    public class RouteAppService : ClcAppServiceBase, IRouteAppService
    {
        public WorkManager WorkManager { get; set; }
        private readonly List<string> _activeStatus = new List<string>() { "激活", "领物", "还物" };

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<RouteWorker> _workerRepository;
        private readonly IRepository<RouteTask> _taskRepository;
        private readonly IRepository<RouteEvent> _eventRepository;
        private readonly IRepository<RouteArticle> _articleRepository;
        private readonly IRepository<RouteInBox> _inBoxRepository;
        private readonly IRepository<RouteOutBox> _outBoxRepository;
        private readonly IRepository<PreRoute> _preRouteRepository;
        private readonly IRepository<PreRouteWorker> _preRouteWorkerRepository;
        private readonly IRepository<PreRouteTask> _preRouteTaskRepository;
        private readonly IRepository<PreVehicleWorker> _preVehicleWorkerRepository;
        private readonly IRepository<ArticleRecord> _articleRecordRepository;
        private readonly IRepository<BoxRecord> _boxRecordRepository;
        private readonly ITaskTypeCache _taskTypeCache;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IRouteTypeCache _routeTypeCache;
        private readonly IRouteCache _routeCache;

        private readonly IOutletCache _outletCache;

        public RouteAppService(IRepository<Route> routeRepository, 
            IRepository<RouteWorker> workerRepository,
            IRepository<RouteTask> taskRepository,
            IRepository<RouteEvent> eventRepository,
            IRepository<RouteArticle> articleRepository,
            IRepository<RouteInBox> inBoxRepository,
            IRepository<RouteOutBox> outBoxRepository,
            IRepository<PreRoute> preRouteRepository,
            IRepository<PreRouteWorker> preRouteWorkerRepository,
            IRepository<PreRouteTask> preRouteTaskRepository, 
            IRepository<PreVehicleWorker> preVehicleWorkerRepository,
            IRepository<ArticleRecord> articleRecordRepository,           
            IRepository<BoxRecord> boxRecordRepository,   
            IRouteTypeCache routeTypeCache,        
            ITaskTypeCache taskTypeCache,
            IWorkRoleCache workRoleCache,
            IRouteCache routeCache,
            IOutletCache outletCache)
        {
            _routeRepository = routeRepository;
            _workerRepository = workerRepository;
            _taskRepository = taskRepository;
            _eventRepository = eventRepository;
            _articleRepository = articleRepository;
            _inBoxRepository = inBoxRepository;
            _outBoxRepository = outBoxRepository;
            _preRouteRepository = preRouteRepository;
            _preRouteWorkerRepository = preRouteWorkerRepository;
            _preRouteTaskRepository = preRouteTaskRepository;
            _preVehicleWorkerRepository = preVehicleWorkerRepository;
            _articleRecordRepository = articleRecordRepository;
            _boxRecordRepository = boxRecordRepository;
            _routeTypeCache = routeTypeCache;
            _taskTypeCache = taskTypeCache;
            _workRoleCache = workRoleCache;
            _routeCache = routeCache;
            _outletCache = outletCache;
        }

        public async Task<List<RouteDto>> GetRoutesAsync(DateTime carryoutDate, string sorting)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.AltVehicle, x => x.CreateWorker);
            query = query.Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<RouteDto>>(entities);
        }

        public async Task<List<RouteDto>> GetAuxRoutesAsync(DateTime carryoutDate, int workplaceId, string sorting) 
        {
            var depots = WorkManager.GetShareDepots(workplaceId);
            var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.AltVehicle, x => x.CreateWorker);
            query = query.Where(x => x.CarryoutDate == carryoutDate && depots.Contains(x.DepotId) && _activeStatus.Contains(x.Status)).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<RouteDto>>(entities);
        }

        public async Task<List<RouteDto>> GetQueryRoutesAsync(DateTime carryoutDate, int depotId, string sorting)
        {
            var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.AltVehicle, x => x.CreateWorker);
            query = query.Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<RouteDto>>(entities);
        }

        public async Task<RouteDto> Insert(RouteDto input)
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result;
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            var entity = ObjectMapper.Map<Route>(input);
            entity.DepotId = WorkManager.GetWorkerDepotId(workerId);
            entity.CreateWorkerId = workerId;
            entity.CreateTime = DateTime.Now;
            await _routeRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteDto>(entity);
        }

        public async Task<RouteDto> Update(RouteDto input)
        {
            var entity = _routeRepository.Get(input.Id);
            ObjectMapper.Map<RouteDto, Route>(input, entity);
            await _routeRepository.UpdateAsync(entity);
            return ObjectMapper.Map<RouteDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _routeRepository.DeleteAsync(id);
        }

        public void SetStatus(DateTime carryoutDate, int depotId, int routeId, string status) {
            // Repoistory
            var e = _routeRepository.Get(routeId);
            if (e.Status != status)
                e.Status = status;

            // Set Cache
            var route = _routeCache.GetRoute(carryoutDate, depotId, routeId);
            route.Status = status;
        }

        public async Task<(string, int)> Activate(List<int> ids, bool finger)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            int count = 0;
            foreach (int id in ids)
            {
                var route = _routeRepository.Get(id);
                if (route.Status != "安排") continue;          // Skip
                var reason = CanActivateRoute(route);
                if (reason != null)
                {
                    return ($"{route.RouteName}因({reason})不能激活", count);
                }
                route.Status = "激活";
                await _routeRepository.UpdateAsync(route);

                // RouteEvent
                var worker = WorkManager.GetWorker(workerId);
                string issuer = string.Format("{0} {1}", worker.Cn, worker.Name);
                var re = new RouteEvent() { RouteId = route.Id, EventTime = DateTime.Now, Name = "激活线路", Description = finger?"指纹":"密码", Issurer = issuer};
                await _eventRepository.InsertAsync(re);
                
                count++;            
            }
            return (null, count);
        }

        public void SetActiveRouteCache(DateTime carryoutDate)
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            // 为激活任务设置缓存
            var query = _routeRepository.GetAllIncluding(x => x.Vehicle, x => x.AltVehicle, x => x.Workers)
                .Where(x => x.CarryoutDate == carryoutDate && x.DepotId == depotId && _activeStatus.Contains(x.Status));
           var lst = ObjectMapper.Map<List<RouteCacheItem>>(query.ToList());
            if (lst.Count > 0)
                _routeCache.Set(carryoutDate, depotId, lst);
        }

        public async Task<int> Close(List<int> ids)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            int count = 0;
            foreach (int id in ids)
            {
                var route = _routeRepository.Get(id);
                route.Status = "关闭";
                await _routeRepository.UpdateAsync(route);

                // RouteEvent
                var worker = WorkManager.GetWorker(workerId);
                string issuer = string.Format("{0} {1}", worker.Cn, worker.Name);
                var re = new RouteEvent() { RouteId = route.Id, EventTime = DateTime.Now, Name = "关闭线路", Issurer = issuer};
                await _eventRepository.InsertAsync(re);

                count++;            
            }
            return count;
        }

        public async Task Back(int id, bool finger)
        {
            var route = _routeRepository.Get(id);
            route.Status = "安排";
            await _routeRepository.UpdateAsync(route);

            // for RouteEvent
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent
            var worker = WorkManager.GetWorker(workerId);
            string issuer = string.Format("{0} {1}", worker.Cn, worker.Name);
            var ae = new RouteEvent() { RouteId = route.Id, EventTime = DateTime.Now, Name = "回退线路", Description = finger?"指纹":"密码", Issurer = issuer};
            await _eventRepository.InsertAsync(ae);
        }

        public async Task<int> CreateFrom(DateTime carryoutDate, DateTime fromDate)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            int depotId = WorkManager.GetWorkerDepotId(workerId);
            var routes = _routeRepository.GetAllList(e=>e.DepotId == depotId && e.CarryoutDate == fromDate);
            foreach (Route r in routes)
            {
                int insertRouteId = await CopyInsertRouteAndGetId(r, depotId, carryoutDate, workerId);

                var workers =  _workerRepository.GetAllList(e => e.RouteId == r.Id);
                foreach (RouteWorker w in workers) 
                {
                     await CopyInsertWorker(w, insertRouteId);
                }

                var tasks =  _taskRepository.GetAllList(e => e.RouteId == r.Id);
                foreach (RouteTask t in tasks) 
                {
                    if (_taskTypeCache[t.TaskTypeId].isTemporary)
                        continue;               // skip
                    await CopyInsertTask(t, insertRouteId, workerId);
                }
            }
            return routes.Count;
        }

        public async Task<int> CreateFromPre(DateTime carryoutDate)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            int depotId = WorkManager.GetWorkerDepotId(workerId);
            var routes = _preRouteRepository.GetAllList(e => e.DepotId == depotId);
            try 
            {
                foreach (PreRoute r in routes)
                {
                    int insertRouteId = await CopyInsertRouteAndGetId(r, depotId, carryoutDate, workerId);

                    var vws = _preVehicleWorkerRepository.GetAllList(e => e.VehicleId == r.VehicleId);
                    if (vws.Count > 0) {
                        foreach (PreVehicleWorker w in vws)
                            await CopyInsertVehicleWorker(w, insertRouteId);
                    }
                    else {
                        var workers = _preRouteWorkerRepository.GetAllList(e => e.PreRouteId == r.Id);
                        foreach (PreRouteWorker w in workers)
                            await CopyInsertWorker(w, insertRouteId);
                    }
                    
                    var tasks =  _preRouteTaskRepository.GetAllList(e => e.PreRouteId == r.Id);
                    foreach (PreRouteTask t in tasks)
                        await CopyInsertTask(t, insertRouteId, workerId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return routes.Count;
        }
        
        #region Son Tables
        public async Task<List<RouteWorkerDto>> GetRouteWorkers(int id, string sorting)
        {
            var query =_workerRepository.GetAllIncluding(x => x.Worker, x => x.AltWorker, x => x.WorkRole, x => x.Articles).Where(e => e.RouteId == id);
            query = query.OrderBy(x => x.WorkRole.Cn);            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var lst = entities.Select(MapToWorker).ToList();
            return lst;
        }

        public async Task<RouteWorkerDto> UpdateWorker(RouteWorkerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<RouteWorkerDto, RouteWorker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteWorkerDto>(entity);
        }

        public async Task<RouteWorkerDto> InsertWorker(RouteWorkerDto input)
        {
            var entity = ObjectMapper.Map<RouteWorker>(input);

            await _workerRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteWorkerDto>(entity);
        }

        public async Task DeleteWorker(int id)
        {
            await _workerRepository.DeleteAsync(id);
        }

        public async Task<List<RouteTaskDto>> GetRouteTasks(int id, string sorting)
        {
            var query =_taskRepository.GetAllIncluding(x => x.Outlet, x => x.TaskType, x => x.CreateWorker, x => x.InBoxes, x => x.OutBoxes)
                .Where(e => e.RouteId == id);
            query = query.OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var lst = entities.Select(MapToTask).ToList();
            return lst;
        }
        public async Task<List<TaskInBoxDto>> GetTaskInBoxes(int id, string sorting)
        {
            var query = _inBoxRepository.GetAllIncluding(x => x.BoxRecord, x=> x.BoxRecord.Box, x => x.BoxRecord.Box.Outlet)
                .Where(e => e.RouteTaskId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<TaskInBoxDto>>(entities);
        }

        public async Task<List<TaskOutBoxDto>> GetTaskOutBoxes(int id, string sorting)
        {
            var query = _outBoxRepository.GetAllIncluding(x => x.BoxRecord, x=> x.BoxRecord.Box, x => x.BoxRecord.Box.Outlet)
                .Where(e => e.RouteTaskId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<TaskOutBoxDto>>(entities);
        }


        public async Task<RouteTaskDto> UpdateTask(RouteTaskDto input)
        {
            var entity = await _taskRepository.GetAsync(input.Id);
            ObjectMapper.Map<RouteTaskDto, RouteTask>(input, entity);

            await _taskRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteTaskDto>(entity);
        }

        public async Task<RouteTaskDto> UpdateTaskRemark(int id, string remark)
        {
            var entity = await _taskRepository.GetAsync(id);
            entity.Remark = remark;
            await _taskRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteTaskDto>(entity);
        }

        public async Task<RouteTaskDto> UpdateTaskPrice(int id, int price)
        {
            var entity = await _taskRepository.GetAsync(id);
            entity.Price = (float)price;
            await _taskRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteTaskDto>(entity);
        }

        public async Task<RouteTaskDto> InsertTask(RouteTaskDto input)
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result;
            var entity = ObjectMapper.Map<RouteTask>(input);
            entity.CreateWorkerId = workerId;
            entity.CreateTime = DateTime.Now;
            await _taskRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<RouteTaskDto>(entity);
        }

        public async Task DeleteTask(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        public async Task<List<RouteEventDto>> GetRouteEvents(int id)
        {
            var query = _eventRepository.GetAll().Where(e => e.RouteId == id);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities.Select(ObjectMapper.Map<RouteEventDto>).ToList();
        }

        public async Task<List<RouteArticleDto>> GetRouteArticles(int id, string sorting)
        {
            var query = _articleRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities.Select(ObjectMapper.Map<RouteArticleDto>).ToList();
        }

        public async Task<List<RouteInBoxDto>> GetRouteInBoxes(int id, string sorting)
        {
            var query = _inBoxRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities.Select(ObjectMapper.Map<RouteInBoxDto>).ToList();
        }

        public async Task<List<RouteOutBoxDto>> GetRouteOutBoxes(int id, string sorting)
        {
            var query = _outBoxRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return entities.Select(ObjectMapper.Map<RouteOutBoxDto>).ToList();
        }

        #endregion

        #region other public 

        [AbpAllowAnonymous]
        public (Route, int) FindRouteForIdentify(int depotId, int workerId)
        {
            var query = _routeRepository.GetAllIncluding(x => x.Workers)
                    .Where(x => x.DepotId == depotId && x.CarryoutDate == DateTime.Today && (x.Status == "激活" || x.Status == "领物"))
                    .OrderByDescending(x => x.StartTime);
            var routes = query.ToList();

            foreach (Route route in routes)
            {
                if (route.Status != "领物" && DateTime.Now < ClcUtils.GetDateTime(route.StartTime)) continue;
                bool found = false;
                int subId = 0;
                foreach (RouteWorker rw in route.Workers)
                {
                    var workRole = _workRoleCache[rw.WorkRoleId];
                    if (string.IsNullOrEmpty(workRole.Duties)) continue;

                    var names =  workRole.Duties.Split('|', ' ', ',');
                    if (rw.GetFactWorkerId() == workerId &&  names.Contains("交接")) found = true;
                    if (names.Contains("辅助交接")) subId = rw.GetFactWorkerId();                    
                }

                if (found && subId != 0) {
                    route.Tasks = _taskRepository.GetAllList(t => t.RouteId == route.Id);
                    return (route, subId);
                }
            }
            return (null, 0);
        }

        [AbpAllowAnonymous]
        public void SetIdentifyTime(int taskId)
        {
            var task = _taskRepository.Get(taskId);
            task.IdentifyTime = DateTime.Now;
            _taskRepository.Update(task);
        }

        [AbpAllowAnonymous]
        public void SetIdentifyEvent(int routeId, string outlet, string issuer)
        {
            var entity = new RouteEvent();
            entity.RouteId = routeId;
            entity.TenantId = 1;
            entity.EventTime = DateTime.Now;
            entity.Name = "网点身份确认";
            entity.Description = outlet;
            entity.Issurer = issuer;
            _eventRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
        }

        [AbpAllowAnonymous]
        public void InsertRouteArriveEvent(int taskId, string address)
        {
            var task = _taskRepository.Get(taskId);
            string issuer = string.Format("{0} {1}", _outletCache[task.OutletId].Name, _taskTypeCache[task.TaskTypeId].Name);

            var entity = new RouteEvent();
            entity.RouteId = task.RouteId;
            entity.TenantId = 1;
            entity.EventTime = DateTime.Now;
            entity.Name = "网点无人身份确认";
            entity.Description = address;
            entity.Issurer = issuer;
            _eventRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
        }

        [AbpAllowAnonymous]
        public List<RouteTask> GetTodayTasks(int outletId)
        {
            var query = _taskRepository.GetAllIncluding(x => x.Route, x => x.TaskType, x => x.Outlet).Where(e => e.Route.CarryoutDate == DateTime.Now.Date && e.Route.Status != "生成" && e.OutletId == outletId).OrderBy(e => e.ArriveTime);
            
            return query.ToList();
        }

        [AbpAllowAnonymous]
        public (int, int, int) GetRouteForLookup(int routeId)
        {
            var query = _routeRepository.GetAllIncluding(x => x.Workers).Where(x => x.Id == routeId);
            var route = query.First();
            int main = 0;
            int sub = 0;
            foreach (RouteWorker rw in route.Workers)
            {
                var workRole = _workRoleCache[rw.WorkRoleId];
                if (string.IsNullOrEmpty(workRole.Duties)) continue;

                var names =  workRole.Duties.Split('|', ' ', ',');
                if (names.Contains("交接")) main = rw.GetFactWorkerId();
                if (names.Contains("辅助交接")) sub = rw.GetFactWorkerId();                    
            }

            return (main, sub, route.GetFactVehicleId());
        }

        [AbpAllowAnonymous]
        public void UpdateTaskRate(int taskId, int rated, string info)
        {
            var task = _taskRepository.Get(taskId);
            task.Rated = rated.ToString();
            task.OutletIdentifyInfo = info;
            _taskRepository.Update(task);
         }
         
        #endregion
        
        #region private

        private async Task<int> CopyInsertRouteAndGetId(Route src, int depotId, DateTime carroutDate,int workerId)
        {
            Route route = new Route();
            route.DepotId = depotId;
            route.CarryoutDate = carroutDate;
            route.RouteName = src.RouteName;
            route.Status = "安排";
            route.RouteTypeId = src.RouteTypeId;
            route.VehicleId = src.VehicleId;
            route.StartTime = src.StartTime;
            route.EndTime = src.EndTime;
            route.Mileage = src.Mileage;
            route.Remark = src.Remark;
            route.CreateWorkerId = workerId;
            route.CreateTime = DateTime.Now;
            return (await _routeRepository.InsertAndGetIdAsync(route));
        }
        private async Task CopyInsertWorker(RouteWorker src, int routeId)
        {
            RouteWorker worker = new RouteWorker();
            worker.RouteId = routeId;
            worker.WorkerId = src.WorkerId;
            worker.WorkRoleId = src.WorkRoleId;
            await _workerRepository.InsertAsync(worker);
        }

        private async Task CopyInsertTask(RouteTask src, int routeId, int workerId)
        {
            RouteTask task = new RouteTask();
            task.RouteId = routeId;
            task.ArriveTime = src.ArriveTime;
            task.OutletId = src.OutletId;
            task.TaskTypeId = src.TaskTypeId;
            task.Remark = src.Remark;
            task.CreateWorkerId = workerId;
            task.CreateTime = DateTime.Now;
            await _taskRepository.InsertAsync(task);
        }
        private async Task<int> CopyInsertRouteAndGetId(PreRoute src, int depotId, DateTime carroutDate,int workerId)
        {
            Route route = new Route();
            route.DepotId = depotId;
            route.CarryoutDate = carroutDate;
            route.RouteName = src.RouteName;
            route.Status = "安排";
            route.RouteTypeId = src.RouteTypeId;
            route.VehicleId = src.VehicleId;
            route.StartTime = src.StartTime;
            route.EndTime = src.EndTime;
            route.Mileage = src.Mileage;
            route.Remark = src.Remark;
            route.CreateWorkerId = workerId;
            route.CreateTime = DateTime.Now;
            return (await _routeRepository.InsertAndGetIdAsync(route));
        }
        private async Task CopyInsertWorker(PreRouteWorker src, int routeId)
        {
            RouteWorker worker = new RouteWorker();
            worker.RouteId = routeId;
            worker.WorkerId = src.WorkerId;
            worker.WorkRoleId = src.WorkRoleId;
            await _workerRepository.InsertAsync(worker);
        }

        private async Task CopyInsertTask(PreRouteTask src, int routeId, int workerId)
        {
            RouteTask task = new RouteTask();
            task.RouteId = routeId;
            task.ArriveTime = src.ArriveTime;
            task.OutletId = src.OutletId;
            task.TaskTypeId = src.TaskTypeId;
            task.Remark = src.Remark;
            task.CreateWorkerId = workerId;
            task.CreateTime = DateTime.Now;
            await _taskRepository.InsertAsync(task);
        }

        private async Task CopyInsertVehicleWorker(PreVehicleWorker src, int routeId)
        {
            RouteWorker worker = new RouteWorker();
            worker.RouteId = routeId;
            worker.WorkerId = src.WorkerId;
            worker.WorkRoleId = src.WorkRoleId;
            await _workerRepository.InsertAsync(worker);
        }

        private RouteWorkerDto MapToWorker(RouteWorker rw)
        {
            var dto = ObjectMapper.Map<RouteWorkerDto>(rw);

            var workerId = rw.GetFactWorkerId();
            var worker = WorkManager.GetWorker(workerId);

            dto.Signin = WorkManager.GetSigninInfo(worker.DepotId, workerId, DateTime.Now);

            if (rw.Articles == null) return dto;

            foreach (var ra in rw.Articles) 
            {
                var record = _articleRecordRepository.Get(ra.ArticleRecordId);
                var a = WorkManager.GetArticle(record.ArticleId);
                var s = record.ReturnTime.HasValue ? string.Format("{0}已还", record.ReturnTime.Value.ToString("HH:mm")) : "未还";
                dto.ArticleList += string.Format("{0}({1}) ", a.Name, s);
            }

            return dto;
        }

        private RouteTaskDto MapToTask(RouteTask rt)
        {
            var dto = ObjectMapper.Map<RouteTaskDto>(rt);

            if (rt.InBoxes != null) {
                dto.InBoxNum = rt.InBoxes.Count;
                foreach (var ib in rt.InBoxes) 
                {
                    var record = _boxRecordRepository.Get(ib.BoxRecordId);
                    var b = WorkManager.GetBox(record.BoxId);

                    dto.InBoxList += string.Format("{0} {1}({2}) ", b.Cn, b.Name, record.InTime.Value.ToString("HH:mm"));
                }
            }

            if (rt.OutBoxes != null) {
                dto.OutBoxNum = rt.OutBoxes.Count;
                foreach (var ob in rt.OutBoxes) 
                {
                    var record = _boxRecordRepository.Get(ob.BoxRecordId);
                    var b = WorkManager.GetBox(record.BoxId);
                    if (record.OutTime.HasValue)
                        dto.OutBoxList += string.Format("{0} {1}({2}) ", b.Cn, b.Name, record.OutTime.Value.ToString("HH:mm"));
                }
            }
            return dto;
        }

        private string CanActivateRoute(Route route)
        {
            var depot = WorkManager.GetDepot(route.DepotId);
            if (depot.LastReportDate.HasValue && depot.LastReportDate.Value >= DateTime.Now)    // route.CarryoutDate)
                return "未到可激活时点";

            var routeType = _routeTypeCache[route.RouteTypeId];

            // check Active Ahead
            var start  = ClcUtils.GetDateTime(route.StartTime).Subtract(new TimeSpan(0, routeType.ActivateLead, 0));
            var end = ClcUtils.GetDateTime(route.EndTime);
            if (!ClcUtils.NowInTimeZone(start, end))  return $"未到激活提前量({routeType.ActivateLead}分钟)或已过结束时间";

            // check same workerId in same route
            var workers = _workerRepository.GetAllList(w => w.RouteId == route.Id);
            var r = workers.GroupBy(x => x.WorkerId).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if (r.Length > 0) return "同一人被多次安排";

            // check workRoles
            List<string> strRoles = new List<string>(routeType.WorkRoles.Split('|', ','));
            var roles = _workRoleCache.GetList().FindAll(x => strRoles.Contains(x.Name));
            foreach (WorkRole role in roles) 
            {
                if (role.mustHave && workers.FirstOrDefault(w => w.WorkRoleId == role.Id) == null) {
                    return  $"任务中必须安排{role.Name}角色";
                }
            }

            // check signin
            bool mustAllSignin = WorkManager.GetDepot(route.DepotId).ActiveRouteNeedCheckin;
            string unSigninNames = string.Empty;
            foreach (RouteWorker rw in workers)
            {
                var w = WorkManager.GetWorker(rw.WorkerId);
                unSigninNames += WorkManager.GetSigninInfo(w.DepotId, w.Id, ClcUtils.GetDateTime(route.StartTime)) == "未签到"  ? w.Name + " " : string.Empty;
                if (mustAllSignin && unSigninNames != string.Empty) 
                {
                    return  $"未签到的人员有{unSigninNames}";
                }
            }

            return null;
        }

        #endregion
    }
}

