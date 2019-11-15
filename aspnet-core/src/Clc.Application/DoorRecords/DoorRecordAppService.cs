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
using Clc.Runtime;
using Clc.Works;

namespace Clc.DoorRecords
{
    [AbpAuthorize(PermissionNames.Pages_Arrange)]
    public class DoorRecordAppService : ClcAppServiceBase, IDoorRecordAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<AskDoorRecord> _askDoorRepository;
        private readonly IRepository<EmergDoorRecord> _emergDoorRepository;

        public DoorRecordAppService(IRepository<AskDoorRecord> askDoorRepository, 
            IRepository<EmergDoorRecord> emergDoorRepository)
        {
           _askDoorRepository = askDoorRepository;
            _emergDoorRepository = emergDoorRepository;

        }

        public async  Task<PagedResultDto<AskDoorRecordDto>> GetAskDoorsAsync(PagedAndSortedResultRequestDto requestDto)

        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            return null;
            
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


        public async Task<EmergDoorRecordDto> GetLastUnApproveEmergDoor(int workerId)
        {
            var query = _emergDoorRepository.GetAllIncluding(x => x.Issue, x => x.Workplace).Where(x => x.ApproverId == workerId);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return ObjectMapper.Map<EmergDoorRecordDto>(entity);
        }

        public (bool, string) AskOpenDoor(DateTime carryoutDate, int depotId, int affairId)
        {
            return (false, "To do");
        }
        public (bool, string) AskOpenDoorTask(DateTime carryoutDate, int depotId, int affairTaskId)
        {
            return (false, "To do");

        }
        
    }
}