using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.PreRoutes.Dto;
using Clc.Authorization;
using Clc.Works;

namespace Clc.PreRoutes
{
    [AbpAuthorize(PermissionNames.Pages_Arrange)]
    public class PreRouteAppService : ClcAppServiceBase, IPreRouteAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<PreRoute> _preRouteRepository;
        private readonly IRepository<PreRouteWorker> _workerRepository;
        private readonly IRepository<PreRouteTask> _taskRepository;

        public PreRouteAppService(IRepository<PreRoute> preRouteRepository, 
            IRepository<PreRouteWorker> workerRepository,
            IRepository<PreRouteTask> taskRepository)
        {
            _preRouteRepository = preRouteRepository;
            _workerRepository = workerRepository;
            _taskRepository = taskRepository;
        }

        public async Task<List<PreRouteDto>> GetPreRoutesAsync(string sorting)
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var query = _preRouteRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle).Where(x => x.DepotId == depotId).OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<PreRouteDto>>(entities);
        }

        public async Task<PreRouteDto> Insert(PreRouteDto input)
        {
            var entity = ObjectMapper.Map<PreRoute>(input);
            entity.DepotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            await _preRouteRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteDto>(entity);
        }

        public async Task<PreRouteDto> Update(PreRouteDto input)
        {
            var entity = _preRouteRepository.Get(input.Id);
            ObjectMapper.Map<PreRouteDto, PreRoute>(input, entity);
            await _preRouteRepository.UpdateAsync(entity);
            return ObjectMapper.Map<PreRouteDto>(entity);
        }

        public async Task Delete(int id)
        {
            await _preRouteRepository.DeleteAsync(id);
        }

        
        #region Son Tables
        public async Task<List<PreRouteWorkerDto>> GetPreRouteWorkers(int id, string sorting)
        {
            var query =_workerRepository.GetAllIncluding(x => x.Worker, x => x.WorkRole).Where(e => e.PreRouteId == id);
            query = query.OrderBy(x => x.WorkRole.Cn);            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var list = ObjectMapper.Map<List<PreRouteWorkerDto>>(entities);
            return list;
        }

        public async Task<PreRouteWorkerDto> UpdateWorker(PreRouteWorkerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<PreRouteWorkerDto, PreRouteWorker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteWorkerDto>(entity);
        }

        public async Task<PreRouteWorkerDto> InsertWorker(PreRouteWorkerDto input)
        {
            var entity = ObjectMapper.Map<PreRouteWorker>(input);

            await _workerRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteWorkerDto>(entity);
        }

        public async Task DeleteWorker(int id)
        {
            await _workerRepository.DeleteAsync(id);
        }

        public async Task<List<PreRouteTaskDto>> GetPreRouteTasks(int id, string sorting)
        {
            var query =_taskRepository.GetAllIncluding(x => x.Outlet, x => x.TaskType).Where(e => e.PreRouteId == id);
            query = query.OrderBy(sorting);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<PreRouteTaskDto>>(entities);
        }

        public async Task<PreRouteTaskDto> UpdateTask(PreRouteTaskDto input)
        {
            var entity = await _taskRepository.GetAsync(input.Id);
            ObjectMapper.Map<PreRouteTaskDto, PreRouteTask>(input, entity);

            await _taskRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteTaskDto>(entity);
        }

        public async Task<PreRouteTaskDto> InsertTask(PreRouteTaskDto input)
        {
            var entity = ObjectMapper.Map<PreRouteTask>(input);

            await _taskRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<PreRouteTaskDto>(entity);
        }

        public async Task DeleteTask(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        #endregion
    }
}

