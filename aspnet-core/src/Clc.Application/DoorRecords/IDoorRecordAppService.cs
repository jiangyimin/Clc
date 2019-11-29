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
        Task<List<DoorDto>> GetDoorsAsync();

        Task<PagedResultDto<AskDoorRecordDto>> GetAskDoorRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input);
        Task<PagedResultDto<EmergDoorRecordDto>> GetEmergDoorRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input);

        Task<List<AskDoorDto>> GetAskDoorsAsync(DateTime dt);
        Task<List<EmergDoorDto>> GetEmergDoorsAsync(DateTime dt);

        
        Task CarryoutAskOpen(int id, int monitorAffairId);
        Task CarryoutEmergOpen(int id, int monitorAffairId);


        Task ProcessIssueEmergDoor(int issueId, int doorId, string content, int leadId);

        Task ApproveEmergDoor(int id, int approverId);

        Task<EmergDoorRecordDto> GetLastUnApproveEmergDoor(int workerId);     
         
    }
}
