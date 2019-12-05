using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Affairs.Dto;
using Clc.Authorization;
using Clc.Runtime;
using Clc.Runtime.Cache;
using Clc.Types;
using Clc.Works;
using Clc.Works.Dto;

namespace Clc.Affairs
{
    [AbpAuthorize]
    public class AffairAppService : ClcAppServiceBase, IAffairAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<Affair> _affairRepository;
        private readonly IRepository<AffairWorker> _workerRepository;
        private readonly IRepository<AffairTask> _taskRepository;
        private readonly IRepository<AffairEvent> _eventRepository;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IAffairCache _affairCache;

        public AffairAppService(IRepository<Affair> affairRepository, 
            IRepository<AffairWorker> workerRepository,
            IRepository<AffairTask> taskRepository,
            IRepository<AffairEvent> eventRepository,
            IWorkRoleCache workRoleCache,
            IAffairCache affairCache)
        {
            _affairRepository = affairRepository;
            _workerRepository = workerRepository;
            _taskRepository = taskRepository;
            _eventRepository = eventRepository;
            _workRoleCache = workRoleCache;
            _affairCache = affairCache;
        }

        public AffairDto GetAffair(int id)
        {
            var entity = _affairRepository.GetAllIncluding(x => x.Workplace).First(x => x.Id == id);
            return ObjectMapper.Map<AffairDto>(entity);
         }

        public async Task<List<AffairDto>> GetAffairsAsync(DateTime carryoutDate, string sorting)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var query = _affairRepository.GetAllIncluding(x => x.Workplace, x => x.CreateWorker).Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<AffairDto>>(entities);
        }

        public async Task<List<AffairDto>> GetQueryAffairsAsync(DateTime carryoutDate, int depotId, string sorting)
        {
            var query = _affairRepository.GetAllIncluding(x => x.Workplace, x => x.CreateWorker).Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<AffairDto>>(entities);
        }

        public async Task<AffairDto> Insert(AffairDto input)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            var entity = ObjectMapper.Map<Affair>(input);
            entity.DepotId = WorkManager.GetWorkerDepotId(workerId);
            entity.CreateWorkerId = workerId;
            entity.CreateTime = DateTime.Now;
            await _affairRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairDto>(entity);
        }

        public async Task<AffairDto> Update(AffairDto input)
        {
            var entity = _affairRepository.Get(input.Id);
            ObjectMapper.Map<AffairDto, Affair>(input, entity);
            await _affairRepository.UpdateAsync(entity);
            return ObjectMapper.Map<AffairDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _affairRepository.DeleteAsync(id);
        }

        public async Task<int> CreateFrom(DateTime carryoutDate, DateTime fromDate)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            int depotId = WorkManager.GetWorkerDepotId(workerId);
            var list = _affairRepository.GetAllList(e=>e.DepotId == depotId && e.CarryoutDate == fromDate);
            foreach (Affair a in list)
            {
                Affair affair = new Affair();
                affair.DepotId = depotId;
                affair.CarryoutDate = carryoutDate;
                affair.Status = "安排";
                affair.Content = a.Content;
                affair.WorkplaceId = a.WorkplaceId;
                affair.StartTime = a.StartTime;
                affair.EndTime = a.EndTime;
                affair.Remark = a.Remark;
                affair.CreateWorkerId = workerId;
                affair.CreateTime = DateTime.Now;
                int affairId = await _affairRepository.InsertAndGetIdAsync(affair);

                var workers =  _workerRepository.GetAllList(e => e.AffairId == a.Id);
                foreach (AffairWorker w in workers)
                {
                    AffairWorker worker = new AffairWorker();
                    worker.AffairId = affairId;
                    worker.WorkerId = w.WorkerId;
                    worker.WorkRoleId = w.WorkRoleId;
                    await _workerRepository.InsertAsync(worker);
                }


                // var tasks =  _taskRepository.GetAllList(e => e.AffairId == a.Id);
                // foreach (AffairTask t in tasks)
                // {
                //     AffairTask task = new AffairTask();
                //     task.AffairId = affairId;
                //     task.WorkplaceId = t.WorkplaceId;
                //     task.Content = t.Content;
                //     task.StartTime = t.StartTime;
                //     task.EndTime = t.EndTime;
                //     task.CreateWorkerId = workerId;
                //     task.CreateTime = DateTime.Now;
                //     await _taskRepository.InsertAsync(task);
                // }
           }
            return list.Count;
        }

        public async Task<(string, int)> Activate(List<int> ids, bool finger)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent
             
            int count = 0;
            foreach (int id in ids)
            {
                Affair affair = _affairRepository.Get(id);
                if (affair.Status != "安排") continue;          // Skip
                var reason = CanActivateAffair(affair);
                if (reason != null)
                {
                    var wp = WorkManager.GetWorkplace(affair.WorkplaceId);
                    return ($"{wp.Name}因({reason})不能激活", count);
                }
                affair.Status = "激活";
                await _affairRepository.UpdateAsync(affair);
                
                // for affairEvent
                var worker = WorkManager.GetWorker(workerId);
                string issuer = string.Format("{0} {1}", worker.Cn, worker.Name);
                var ae = new AffairEvent() { AffairId = affair.Id, EventTime = DateTime.Now, Name = "激活任务", Description = finger?"指纹":"密码", Issurer = issuer};
                await _eventRepository.InsertAsync(ae);
                
                count++;            
            }
            return (null, count);
        }
        public async Task SetActiveAffairCache(DateTime carryoutDate)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            // 为激活任务设置缓存
            var query = _affairRepository.GetAllIncluding(x => x.Workplace, x => x.Workers).Where(x => x.CarryoutDate == carryoutDate && x.DepotId == depotId && x.Status != "安排");
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var lst = ObjectMapper.Map<List<AffairCacheItem>>(entities);
            if (lst.Count > 0)
                _affairCache.Set(carryoutDate, depotId, lst);
        }

        public async Task Back(int id, bool finger)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            var affair = _affairRepository.Get(id);
            affair.Status = "安排";
            await _affairRepository.UpdateAsync(affair);

            // for affairEvent
            var worker = WorkManager.GetWorker(workerId);
            string issuer = string.Format("{0} {1}", worker.Cn, worker.Name);
            var ae = new AffairEvent() { AffairId = affair.Id, EventTime = DateTime.Now, Name = "回退任务", Description = finger?"指纹":"密码", Issurer = issuer};
            await _eventRepository.InsertAsync(ae);

        }
        
        #region Son Tables
        public async Task<List<AffairWorkerDto>> GetAffairWorkersAsync(int id)
        {
            var query =_workerRepository.GetAllIncluding(x => x.Worker, x => x.WorkRole).Where(e => e.AffairId == id).OrderBy(x => x.WorkRole.Cn);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var list = ObjectMapper.Map<List<AffairWorkerDto>>(entities);
            return list;
        }

        public List<AffairWorkerDto> GetAffairWorkersSync(int id)
        {
            var query =_workerRepository.GetAllIncluding(x => x.Worker, x => x.WorkRole)
                .Where(e => e.AffairId == id).OrderBy(x => x.WorkRole.Cn);
            
            var entities = query.ToList();
            return  entities.Select(MapToAffairWorkerDto).ToList();
        }

        public async Task<AffairWorkerDto> UpdateWorker(AffairWorkerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<AffairWorkerDto, AffairWorker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairWorkerDto>(entity);
        }

        public async Task<AffairWorkerDto> InsertWorker(AffairWorkerDto input)
        {
            var entity = ObjectMapper.Map<AffairWorker>(input);

            await _workerRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairWorkerDto>(entity);
        }

        public async Task DeleteWorker(int id)
        {
            await _workerRepository.DeleteAsync(id);
            CurrentUnitOfWork.SaveChanges();
         }

        public async Task<List<AffairTaskDto>> GetAffairTasksAsync(int id, string sorting)
        {
            var query =_taskRepository.GetAllIncluding(x => x.Workplace, x => x.CreateWorker).Where(e => e.AffairId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<AffairTaskDto>>(entities);
        }

        public async Task<AffairTaskDto> UpdateTask(AffairTaskDto input)
        {
            var entity = await _taskRepository.GetAsync(input.Id);
            ObjectMapper.Map<AffairTaskDto, AffairTask>(input, entity);

            await _taskRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairTaskDto>(entity);
        }

        public void SetTaskTime(int id, bool isStart)
        {
            var entity = _taskRepository.Get(id);
            if (isStart)
            {
                if (!entity.StartTimeActual.HasValue) entity.StartTimeActual = DateTime.Now;
            }
            else
            {
                entity.EndTimeActual = DateTime.Now;
            }
            
            _taskRepository.Update(entity);
        }

        public async Task<AffairTaskDto> InsertTask(AffairTaskDto input)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent
            
            var entity = ObjectMapper.Map<AffairTask>(input);
            entity.CreateWorkerId = workerId;
            entity.CreateTime = DateTime.Now;
            await _taskRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairTaskDto>(entity);
        }

        public async Task DeleteTask(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        public async Task<List<AffairEventDto>> GetAffairEventsAsync(int id)
        {
            var query =_eventRepository.GetAll().Where(e => e.AffairId == id);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new List<AffairEventDto>(entities.Select(ObjectMapper.Map<AffairEventDto>).ToList());
        }

        public async Task<AffairEventDto> InsertEvent(int affairId, string name, string desc, string issurer)
        {
            var entity = new AffairEvent();
            entity.AffairId = affairId;
            entity.EventTime = DateTime.Now;
            entity.Name = "监控";
            entity.Description = desc;
            entity.Issurer = issurer;

            await _eventRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairEventDto>(entity);
        }

        public async Task<AffairEventDto> InsertTempArticle(string style, int affairId, List<RouteArticleCDto> articles, string workers)
        {
            string desc = null;
            foreach (var a in articles) {
                desc += a.DisplayText + ',';
            }

            var entity = new AffairEvent();
            entity.AffairId = affairId;
            entity.EventTime = DateTime.Now;
            entity.Name = style;
            entity.Description = desc;
            entity.Issurer = workers;

            await _eventRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairEventDto>(entity);
        }
        #endregion

        #region priavte

        private AffairWorkerDto MapToAffairWorkerDto(AffairWorker aw)
        {
            var dto = ObjectMapper.Map<AffairWorkerDto>(aw);

            if (aw.Worker.Photo != null) 
                dto.PhotoString = Convert.ToBase64String(aw.Worker.Photo);
            return dto;
        }

        private string CanActivateAffair(Affair affair)
        {
            // check workplace
            var wp = WorkManager.GetWorkplace(affair.WorkplaceId);

            // check time
            var start  = ClcUtils.GetDateTime(affair.StartTime).Subtract(new TimeSpan(12, 0, 0));
            var end = ClcUtils.GetDateTime(affair.EndTime);
            if (!ClcUtils.NowInTimeZone(start, end))  return $"未到激活提前量(半天)或已过结束时间";

            // check same workerId in same route
            var workers = _workerRepository.GetAllList(w => w.AffairId == affair.Id);
            var r = workers.GroupBy(x => x.WorkerId).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if (r.Length > 0) return "同一人被多次安排";

            // check workRoles
            List<string> strRoles = new List<string>(wp.WorkRoles.Split('|', ','));
            var roles = _workRoleCache.GetList().FindAll(x => strRoles.Contains(x.Name));
            foreach (WorkRole role in roles) 
            {
                if (role.mustHave && workers.FirstOrDefault(w => w.WorkRoleId == role.Id) == null) {
                    return  $"任务中必须安排{role.Name}角色";
                }
            }

            return null;
        }

        #endregion 
    }
}

