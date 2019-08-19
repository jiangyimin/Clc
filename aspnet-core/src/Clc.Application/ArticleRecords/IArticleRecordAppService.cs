using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.ArticleRecords.Dto;

namespace Clc.ArticleRecords
{
    public interface IArticleRecordAppService : IApplicationService
    {
        Task<PagedResultDto<ArticleRecordDto>> GetArticlesAsync(PagedAndSortedResultRequestDto requestDto);
        int Lend(int routeId, int routeWorkerId, List<int> ids, string workers);
        int Return(List<int> recordIds, string workers);       


        string GetArticleStatus(int articleId);

        Task<List<ArticleRecordSearchDto>> SearchByDay(DateTime theDay);
        Task<List<ArticleRecordSearchDto>> SearchByArticleId(int articleId, DateTime begin, DateTime end);

        Task<List<ArticleReportDto>> GetReportData();
    }
}
