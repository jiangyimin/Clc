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
using Clc.Works;

namespace Clc.Affairs
{
    [AbpAuthorize(PermissionNames.Pages_Arrange)]
    public class AffairAppService : ClcAppServiceBase, IAffairAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<Affair> _affairRepository;
        private readonly IRepository<AffairWorker> _workerRepository;
        private readonly IRepository<AffairTask> _taskRepository;
        private readonly IRepository<AffairEvent> _eventRepository;

        public AffairAppService(IRepository<Affair> affairRepository, 
            IRepository<AffairWorker> workerRepository,
            IRepository<AffairTask> taskRepository,
            IRepository<AffairEvent> eventRepository)
        {
            _affairRepository = affairRepository;
            _workerRepository = workerRepository;
            _taskRepository = taskRepository;
            _eventRepository = eventRepository;
        }

        public async Task<List<AffairDto>> GetAffairsAsync(DateTime carryoutDate, string sorting)
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            string dateString = carryoutDate.ToString("yyyy-MM-dd");
            // string filter = $"DepotId={depotId} AND CarryoutDate=\"{dateString}\"";
            var query = _affairRepository.GetAllIncluding(x => x.Workplace, x => x.CreateWorker).Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<AffairDto>>(entities);
        }

        public async Task<AffairDto> Insert(AffairDto input)
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result;
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
            int depotId = WorkManager.GetWorkerDepotId(workerId);
            var list = _affairRepository.GetAllList(e=>e.DepotId == depotId && e.CarryoutDate == fromDate);
            foreach (Affair a in list)
            {
                Affair affair = new Affair();
                affair.DepotId = depotId;
                affair.CarryoutDate = carryoutDate;
                affair.Status = "安排";
                affair.WorkplaceId = a.WorkplaceId;
                affair.StartTime = a.StartTime;
                affair.EndTime = a.EndTime;
                affair.IsTomorrow = a.IsTomorrow;
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


                var tasks =  _taskRepository.GetAllList(e => e.AffairId == a.Id);
                foreach (AffairTask t in tasks)
                {
                    AffairTask task = new AffairTask();
                    task.AffairId = affairId;
                    task.WorkplaceId = t.WorkplaceId;
                    task.StartTime = t.StartTime;
                    task.EndTime = t.EndTime;
                    task.IsTomorrow = t.IsTomorrow;
                    task.CreateWorkerId = workerId;
                    task.CreateTime = DateTime.Now;
                    await _taskRepository.InsertAsync(task);
                }
           }
            return list.Count;
        }

        public async Task<int> Activate(List<int> ids)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            
            int count = 0;
            foreach (int id in ids)
            {
                var affair = _affairRepository.Get(id);
                if (affair.Status != "安排") continue;          // Skip
                affair.Status = "激活";
                await _affairRepository.UpdateAsync(affair);
                // for affairEvent
                string issuer = string.Format("{0} {1}", WorkManager.GetWorkerCn(workerId), WorkManager.GetWorkerName(workerId));
                var ae = new AffairEvent() { AffairId = affair.Id, EventTime = DateTime.Now, Name = "激活任务",Issurer = issuer};
                await _eventRepository.InsertAsync(ae);
                
                count++;            
            }
            return count;
        }

        public Task Back(int id)
        {
            var entity = _affairRepository.Get(id);
            entity.Status = "安排";
            return _affairRepository.UpdateAsync(entity);
        }
        
        #region Son Tables
        public async Task<List<AffairWorkerDto>> GetAffairWorkers(int id)
        {
            var query =_workerRepository.GetAllIncluding(x => x.Worker, x => x.WorkRole).Where(e => e.AffairId == id).OrderBy(x => x.WorkRole.Cn);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var list = ObjectMapper.Map<List<AffairWorkerDto>>(entities);
            return list;
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

        public async Task<List<AffairTaskDto>> GetAffairTasks(int id, string sorting)
        {
            var query =_workerRepository.GetAll().Where(e => e.AffairId == id).OrderBy(sorting);
            
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

        public async Task<AffairTaskDto> InsertTask(AffairTaskDto input)
        {
            var entity = ObjectMapper.Map<AffairTask>(input);

            await _taskRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<AffairTaskDto>(entity);
        }

        public async Task DeleteTask(int id)
        {
            await _workerRepository.DeleteAsync(id);
        }

        public async Task<List<AffairEventDto>> GetAffairEvents(int id, string sorting)
        {
            var query =_eventRepository.GetAll().Where(e => e.AffairId == id).OrderBy(sorting);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new List<AffairEventDto>(entities.Select(ObjectMapper.Map<AffairEventDto>).ToList());
        }

        #endregion
    }
}

