using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.Linq.Extensions;
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

        public async Task<List<DoorDto>> GetDoorsAsync(int depotId)
        {
            var query = _workplaceRepository.GetAllIncluding(x => x.Depot)
                .WhereIf(depotId > 0, x => x.DepotId == depotId)
                .Where(x => !string.IsNullOrEmpty(x.AskOpenStyle));
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<DoorDto>>(entities);
        }

        public async Task<PagedResultDto<AskDoorRecordDto>> GetAskDoorRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input)
        {
            var query = _askDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot, x => x.Route, x => x.AskAffair, x => x.MonitorAffair, x => x.MonitorAffair.Workers, x => x.MonitorAffair.Workers)
                .Where(x => x.WorkplaceId == workplaceId && x.ProcessTime.HasValue);
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
                .Where(x => x.WorkplaceId == workplaceId && x.ProcessTime.HasValue);
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
            var query = _askDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot, x => x.Route, x => x.Route.Depot, x => x.AskAffair)
                .Where(x => x.AskTime.Date == dt && x.AskReason == x.Approver && !x.ProcessTime.HasValue);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return entities.Select(MapToAskDoorDto).ToList();
        }

        public async Task<List<EmergDoorDto>> GetEmergDoorsAsync(DateTime dt)
        {
            var query = _emergDoorRepository.GetAllIncluding(x => x.Workplace, x => x.Workplace.Depot, x => x.Issue, x => x.Approver)
                .Where(x => x.CreateTime.Date == dt && x.ApprovalTime.HasValue && !x.ProcessTime.HasValue);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<EmergDoorDto>>(entities);
        }

        public (string, string) GetNotifyInfo(int id) 
        {
            var record = _askDoorRepository.Get(id);

            var doorName = WorkManager.GetWorkplace(record.WorkplaceId).Name;
            var match = Regex.Matches(record.AskWorkers, @"\d+").Select(m => m.Value).ToList();

            return (string.Join('|', match), doorName);
        }


        public async Task CarryoutAskOpen(int id, int monitorAffairId)
        {
            if (id == 0) return;
            // get AskDoorRecord
            var entity = await _askDoorRepository.GetAsync(id);
            entity.ProcessTime = DateTime.Now;
            entity.MonitorAffairId = monitorAffairId;
            await _askDoorRepository.UpdateAsync(entity);
        }

        public async Task CarryoutEmergOpen(int id, int monitorAffairId)
        {
            if (id == 0) return;
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

        [AbpAllowAnonymous]
        public void ApproveEmergDoor(int id, int approverId)
        {
            // get EmergDoorRecord
            var entity = _emergDoorRepository.Get(id);
            entity.ApprovalTime = DateTime.Now;
            entity.ApproverId = approverId;
            _emergDoorRepository.Update(entity);
        }
        
        [AbpAllowAnonymous]
        public void ApproveTempDoor(int id, string approver)
        {
            // get AskDoorRecord
            var entity = _askDoorRepository.Get(id);
            entity.AskReason += '|' + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            entity.Approver = entity.AskReason;
            _askDoorRepository.Update(entity);
        }

        [AbpAllowAnonymous]
        public async Task<EmergDoorRecordDto> GetLastUnApproveEmergDoor(int workerId)
        {
            var query = _emergDoorRepository.GetAllIncluding(x => x.Issue, x => x.Workplace).Where(x => x.CreateTime.Date == DateTime.Now.Date && x.ApproverId == workerId && !x.ApprovalTime.HasValue);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return ObjectMapper.Map<EmergDoorRecordDto>(entity);
        }

        [AbpAllowAnonymous]
        public async Task<AskDoorDto> GetLastUnApproveTempDoor(string workerCn)
        {
            var query = _askDoorRepository.GetAllIncluding(x => x.Workplace)
                .Where(x => x.AskTime.Date == DateTime.Now.Date && x.AskReason == workerCn && x.Approver == null);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return ObjectMapper.Map<AskDoorDto>(entity);
        }

        #region private
        private AskDoorDto MapToAskDoorDto(AskDoorRecord record)
        {
            var dto = ObjectMapper.Map<AskDoorDto>(record);
            dto.AskStyle = record.RouteId.HasValue ? $"线路({record.Route.RouteName})": "验证";
            dto.RouteInfo = record.RouteId.HasValue ? $"{record.Route.RouteName}({record.Route.Depot.Name})": "";

            return dto;
        }

        private AskDoorRecordDto MapToAskDoorRecordDto(AskDoorRecord record)
        {
            var dto = ObjectMapper.Map<AskDoorRecordDto>(record);

            dto.AskStyle = record.RouteId.HasValue ? $"线路({record.Route.RouteName})" : "验证";
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