﻿using System;
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

        Task<List<AskDoorDto>> GetAskDoorsAsync(DateTime day);
        Task<List<EmergDoorDto>> GetEmergDoorsAsync(DateTime day);


        Task ProcessIssueEmergDoor(int issueId, int doorId, string content, int leadId);

        Task<EmergDoorRecordDto> GetLastUnApproveEmergDoor(int workerId);        
    }
}
