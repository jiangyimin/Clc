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
        int In(int routeId, List<RouteBoxCDto> boxes, string workers);
        int Out(int routeId, List<RouteBoxCDto> boxes, string workers);       


        string GetBoxStatus(int articleId);

        Task<List<BoxRecordSearchDto>> SearchByDay(DateTime theDay);
        Task<List<BoxRecordSearchDto>> SearchByBoxId(int boxId, DateTime begin, DateTime end);

        Task<List<BoxReportDto>> GetReportData();
    }
}
