using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.ArticleRecords.Dto;
using Clc.Works.Dto;

namespace Clc.ArticleRecords
{
    public interface IArticleRecordAppService : IApplicationService
    {
        Task<PagedResultDto<ArticleRecordDto>> GetArticlesAsync(PagedAndSortedResultRequestDto requestDto);
        int Lend(int routeId, int routeWorkerId, List<RouteArticleCDto> articles, string workers);
        int Return(int routeId, List<RouteArticleCDto> articles, string workers);       


        // string GetArticleStatus(int articleId);

        Task<List<ArticleRecordSearchDto>> SearchByDay(DateTime theDay);
        Task<List<ArticleRecordSearchDto>> SearchByArticleId(int articleId, DateTime begin, DateTime end);

        Task<List<ArticleReportDto>> GetReportData();
    }
}
