using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.DoorRecords.Dto;

namespace Clc.DoorRecords
{
    public interface IDoorRecordAppService : IApplicationService
    {
        Task<PagedResultDto<AskDoorRecordDto>> GetAskDoorsAsync(PagedAndSortedResultRequestDto requestDto);

        Task ProcessIssueEmergDoor(int issueId, int doorId, string content, int leadId);

        Task<EmergDoorRecordDto> GetLastUnApproveEmergDoor(int workerId);

        (bool, string) AskOpenDoor(DateTime carryoutDate, int depotId, int affairId);
        
        
        (bool, string) AskOpenDoorTask(DateTime carryoutDate, int depotId, int affairTaskId);
        
    }
}
