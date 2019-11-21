using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Authorization;
using Clc.DoorRecords.Dto;
using Clc.Fields;
using Clc.Runtime;
using Clc.Works;

namespace Clc.DoorRecords
{
    [AbpAuthorize(PermissionNames.Pages_Arrange, PermissionNames.Pages_Monitor)]
    public class DoorRecordAppService : ClcAppServiceBase, IDoorRecordAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<AskDoorRecord> _askDoorRepository;
        private readonly IRepository<EmergDoorRecord> _emergDoorRepository;
        private readonly IRepository<Workplace> _workplaceRepository;


        public DoorRecordAppService(IRepository<AskDoorRecord> askDoorRepository, 
            IRepository<EmergDoorRecord> emergDoorRepository,
            IRepository<Workplace> workplaceRepository)
        {
            _askDoorRepository = askDoorRepository;
            _emergDoorRepository = emergDoorRepository;
            _workplaceRepository = workplaceRepository;
        }

        public async Task<List<DoorDto>> GetDoorsAsync()
        {
            var query = _workplaceRepository.GetAllIncluding(x => x.Depot)
                .Where(x => !string.IsNullOrEmpty(x.AskOpenStyle));
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<DoorDto>>(entities);
        }

        public async Task<PagedResultDto<AskDoorRecordDto>> GetAskDoorRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input)
        {
            var query = _askDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot, x => x.MonitorAffair, x => x.MonitorAffair.Workers, x => x.MonitorAffair.Workers)
                .Where(x => x.WorkplaceId == workplaceId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(x => x.AskTime);                     // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<AskDoorRecordDto>(
                totalCount,
                entities.Select(MapToAskDoorRecordDto).ToList()
            );
        }


        public async Task<PagedResultDto<EmergDoorRecordDto>> GetEmergDoorRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input)
        {
            var query = _emergDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot, x => x.MonitorAffair, x => x.MonitorAffair.Workers)
                .Where(x => x.WorkplaceId == workplaceId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = query.OrderByDescending(x => x.CreateTime);                     // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<EmergDoorRecordDto>(
                totalCount,
                entities.Select(MapToEmergDoorRecordDto).ToList()
            );
        }

        public async Task<List<AskDoorDto>> GetAskDoorsAsync(DateTime dt)
        {
            var query = _askDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot)
                .Where(x => x.AskTime.Date == dt && !x.ProcessTime.HasValue);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return entities.Select(MapToAskDoorDto).ToList();
        }

        public async Task<List<EmergDoorDto>> GetEmergDoorsAsync(DateTime dt)
        {
            var query = _emergDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot, x => x.Issue)
                .Where(x => x.CreateTime.Date == dt && x.ApprovalTime.HasValue && !x.ProcessTime.HasValue);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<EmergDoorDto>>(entities);
        }


        public async Task CarryoutAskOpen(int id, int monitorAffairId)
        {
            // get AskDoorRecord
            var entity = await _askDoorRepository.GetAsync(id);
            entity.ProcessTime = DateTime.Now;
            entity.MonitorAffairId = monitorAffairId;
            await _askDoorRepository.UpdateAsync(entity);
         }

        public async Task CarryoutEmergOpen(int id, int monitorAffairId)
        {
            // get EmergDoorRecord
            var entity = await _emergDoorRepository.GetAsync(id);
            entity.ProcessTime = DateTime.Now;
            entity.MonitorAffairId = monitorAffairId;
            await _emergDoorRepository.UpdateAsync(entity);
        }

        public async Task ProcessIssueEmergDoor(int issueId, int doorId, string content, int leadId)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();

            // insert EmergDoorRecord
            var entity = new EmergDoorRecord();
            entity.CreateTime = DateTime.Now;
            entity.WorkplaceId = doorId;
            entity.IssueId = issueId;
            entity.ApproverId = leadId;
            await _emergDoorRepository.InsertAsync(entity);
       }

        public async Task ApproveEmergDoor(int id, int approverId)
        {
            // get EmergDoorRecord
            var entity = await _emergDoorRepository.GetAsync(id);
            entity.ApprovalTime = DateTime.Now;
            entity.ApproverId = approverId;
            await _emergDoorRepository.UpdateAsync(entity);
        }


        public async Task<EmergDoorRecordDto> GetLastUnApproveEmergDoor(int workerId)
        {
            var query = _emergDoorRepository.GetAllIncluding(x => x.Issue, x => x.Workplace).Where(x => x.ApproverId == workerId);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return ObjectMapper.Map<EmergDoorRecordDto>(entity);
        }

        #region private
        private AskDoorDto MapToAskDoorDto(AskDoorRecord record)
        {
            var dto = ObjectMapper.Map<AskDoorDto>(record);
            dto.AskStyle = record.RouteId.HasValue ? "线路任务" : "常规";
            return dto;
        }

        private AskDoorRecordDto MapToAskDoorRecordDto(AskDoorRecord record)
        {
            var dto = ObjectMapper.Map<AskDoorRecordDto>(record);

            dto.AskStyle = record.RouteId.HasValue ? "线路任务" : "常规";
            if (!record.MonitorAffairId.HasValue) return dto;
            
            foreach (var w in record.MonitorAffair.Workers)
            {
                var worker = WorkManager.GetWorker(w.WorkerId);
                dto.MonitorWorkers += string.Format("{0} {1}, ", worker.Cn, worker.Name);
            }
            return dto;
        }

        private EmergDoorRecordDto MapToEmergDoorRecordDto(EmergDoorRecord record)
        {
            var dto = ObjectMapper.Map<EmergDoorRecordDto>(record);

            if (!record.MonitorAffairId.HasValue) return dto;
            
            foreach (var w in record.MonitorAffair.Workers)
            {
                var worker = WorkManager.GetWorker(w.WorkerId);
                dto.MonitorWorkers += string.Format("{0} {1}, ", worker.Cn, worker.Name);
            }
            return dto;
        }

        #endregion
    }
}