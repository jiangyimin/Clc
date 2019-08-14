using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Routes.Dto;
using Clc.Authorization;
using Clc.PreRoutes;
using Clc.Runtime.Cache;
using Clc.Works;

namespace Clc.Routes
{
    [AbpAuthorize(PermissionNames.Pages_Arrange)]
    public class RouteAppService : ClcAppServiceBase, IRouteAppService
    {
        public WorkManager WorkManager { get; set; }

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
        private readonly ITaskTypeCache _taskTypeCache;

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
            ITaskTypeCache taskTypeCache)
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
            _taskTypeCache = taskTypeCache;
        }

        public async Task<List<RouteDto>> GetRoutesAsync(DateTime carryoutDate, string sorting)
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.CreateWorker);
            query = query.Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<RouteDto>>(entities);
        }

        public async Task<RouteDto> Insert(RouteDto input)
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result;
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
        public async Task<int> Activate(List<int> ids)
        {
            int count = 0;
            foreach (int id in ids)
            {
                var route = _routeRepository.Get(id);
                if (route.Status != "安排") continue;          // Skip
                route.Status = "激活";
                await _routeRepository.UpdateAsync(route);
                count++;            
            }
            return count;
        }

        public Task Back(int id)
        {
            var entity = _routeRepository.Get(id);
            entity.Status = "安排";
            return _routeRepository.UpdateAsync(entity);
        }

        public async Task<int> CreateFrom(DateTime carryoutDate, DateTime fromDate)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
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
            int depotId = WorkManager.GetWorkerDepotId(workerId);
            var routes = _preRouteRepository.GetAllList(e => e.DepotId == depotId);
            try 
            {
                foreach (PreRoute r in routes)
                {
                    int insertRouteId = await CopyInsertRouteAndGetId(r, depotId, carryoutDate, workerId);

                    var workers =  _preRouteWorkerRepository.GetAllList(e => e.PreRouteId == r.Id);
                    foreach (PreRouteWorker w in workers)
                        await CopyInsertWorker(w, insertRouteId);

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
            var query =_workerRepository.GetAllIncluding(x => x.Worker, x => x.WorkRole).Where(e => e.RouteId == id);
            query = query.OrderBy(x => x.WorkRole.Cn);            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var list = ObjectMapper.Map<List<RouteWorkerDto>>(entities);
            return list;
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
            var query =_taskRepository.GetAllIncluding(x => x.Outlet, x => x.TaskType).Where(e => e.RouteId == id);
            query = query.OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<RouteTaskDto>>(entities);
        }

        public async Task<RouteTaskDto> UpdateTask(RouteTaskDto input)
        {
            var entity = await _taskRepository.GetAsync(input.Id);
            ObjectMapper.Map<RouteTaskDto, RouteTask>(input, entity);

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

        public async Task<List<RouteEventDto>> GetRouteEvents(int id, string sorting)
        {
            var query = _eventRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new List<RouteEventDto>(entities.Select(ObjectMapper.Map<RouteEventDto>).ToList());
        }

        public async Task<List<RouteArticleDto>> GetRouteArticles(int id, string sorting)
        {
            var query = _articleRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new List<RouteArticleDto>(entities.Select(ObjectMapper.Map<RouteArticleDto>).ToList());
        }

        public async Task<List<RouteInBoxDto>> GetRouteInBoxes(int id, string sorting)
        {
            var query = _inBoxRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new List<RouteInBoxDto>(entities.Select(ObjectMapper.Map<RouteInBoxDto>).ToList());
        }

        public async Task<List<RouteOutBoxDto>> GetRouteOutBoxes(int id, string sorting)
        {
            var query = _outBoxRepository.GetAll().Where(e => e.RouteId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new List<RouteOutBoxDto>(entities.Select(ObjectMapper.Map<RouteOutBoxDto>).ToList());
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
        #endregion
    }
}

