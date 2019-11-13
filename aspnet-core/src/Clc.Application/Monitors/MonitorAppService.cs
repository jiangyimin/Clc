using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
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
        private readonly IRepository<AskDoorRecord> _askDoorRepository;
        private readonly IRepository<EmergDoorRecord> _emergDoorRepository;

        public MonitorAppService(IRepository<Workplace> workplaceRepository,
            IRepository<AskDoorRecord> askDoorRepository, IRepository<EmergDoorRecord> emergDoorRepository)
        {
            _workplaceRepository = workplaceRepository;
            _askDoorRepository = askDoorRepository;
            _emergDoorRepository = emergDoorRepository;
        }

 
        public async Task<List<DoorDto>> GetDoorsAsync()
        {
            var query = _workplaceRepository.GetAllIncluding(x => x.Depot);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<DoorDto>>(entities);
        }

        public async Task<PagedResultDto<AskDoorRecordDto>> GetRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input)
        {
            var query = _askDoorRepository.GetAllIncluding(x => x.Workplace, x => x.MonitorAffair, x => x.MonitorAffair.Workers)
                .Where(x => x.WorkplaceId == workplaceId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(x => x.AskTime);                     // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<AskDoorRecordDto>(
                totalCount,
                entities.Select(MapToDoorRecordDto).ToList()
            );
        }

        public async Task Insert(int doorId, int affairId, string askWorkers)
        {
            var entity = new AskDoorRecord() { WorkplaceId = doorId, MonitorAffairId = affairId, AskWorkers = askWorkers};
            entity.AskTime = DateTime.Now;
            await _askDoorRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
        }

        private AskDoorRecordDto MapToDoorRecordDto(AskDoorRecord record)
        {
            var dto = ObjectMapper.Map<AskDoorRecordDto>(record);

            if (!record.MonitorAffairId.HasValue) return dto;
            
            foreach (var w in record.MonitorAffair.Workers)
            {
                dto.MonitorWorkers += string.Format("{0} {1}, ", w.Worker.Cn, w.Worker.Name);
            }
            return dto;
        }
    }
}