using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.BoxRecords.Dto;
using Clc.Works.Dto;

namespace Clc.BoxRecords
{
    public interface IBoxRecordAppService : IApplicationService
    {
        Task<PagedResultDto<BoxRecordDto>> GetBoxesAsync(PagedAndSortedResultRequestDto requestDto);
        string InBox(int taskId, string rfid, string workers);
        string OutBox(int taskId, string rfid, string workers);       


        string GetBoxStatus(int articleId);

        Task<List<BoxRecordSearchDto>> SearchByDay(DateTime theDay);
        Task<List<BoxRecordSearchDto>> SearchByBoxId(int boxId, DateTime begin, DateTime end);

        Task<List<BoxReportDto>> GetReportData();
    }
}
