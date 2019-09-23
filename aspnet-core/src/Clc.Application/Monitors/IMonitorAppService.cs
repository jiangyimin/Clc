using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.BoxRecords.Dto;
using Clc.Monitors.Dto;

namespace Clc.Monitors
{
    public interface IMonitorAppService : IApplicationService
    {
        Task<List<DoorDto>> GetDoorsAsync();

        Task<PagedResultDto<DoorRecordDto>> GetRecordsAsync(int workplaceId, PagedAndSortedResultRequestDto input);

        Task Insert(int doorId, int affairId);
    }
}
