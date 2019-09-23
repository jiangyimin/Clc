using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Affairs;
using Clc.BoxRecords.Dto;
using Clc.Fields;
using Clc.Monitors.Dto;
using Clc.Runtime;
using Clc.Works;

namespace Clc.Monitors
{

    [AbpAuthorize]
    public class MonitorAppService : ClcAppServiceBase, IMonitorAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<Workplace> _workplaceRepository;
        private readonly IRepository<DoorRecord> _recordRepository;

        public MonitorAppService(IRepository<Workplace> workplaceRepository,
            IRepository<DoorRecord> recordRepository)
        {
            _workplaceRepository = workplaceRepository;
            _recordRepository = recordRepository;
        }

 
        public async Task<List<DoorDto>> GetDoorsAsync()
        {
            var query = _workplaceRepository.GetAllIncluding(x => x.Depot);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<DoorDto>>(entities);
        }

        public async Task<PagedResultDto<DoorRecordDto>> GetRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input)
        {
            var query = _recordRepository.GetAllIncluding(x => x.WorkplaceId, x => x.OpenAffair, x => x.ApplyAffair)
                .Where(x => x.WorkplaceId == workplaceId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(x => x.CreateTime);                     // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<DoorRecordDto>(
                totalCount,
                entities.Select(MapToDoorRecordDto).ToList()
            );
        }

        public async Task Insert(int doorId, int affairId)
        {
            var entity = new DoorRecord() { WorkplaceId = doorId, OpenAffairId = affairId };
            entity.CreateTime = DateTime.Now;
            await _recordRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
        }

        #region util

        private DoorRecordDto MapToDoorRecordDto(DoorRecord entity)
        {
            var dto = ObjectMapper.Map<DoorRecordDto>(entity);

            foreach (AffairWorker w in entity.OpenAffair.Workers)
            {
                var worker = WorkManager.GetWorker(w.WorkerId);
                dto.OpenWorkers = string.Format("{0} {1}, ", worker.Id, worker.Name);
            }
            foreach (AffairWorker w in entity.ApplyAffair.Workers)
            {
                var worker = WorkManager.GetWorker(w.WorkerId);
                dto.OpenWorkers = string.Format("{0} {1}, ", worker.Id, worker.Name);
            }
            return dto;
        }

        #endregion
    }
}