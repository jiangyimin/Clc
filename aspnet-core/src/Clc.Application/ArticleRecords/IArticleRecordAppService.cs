using System.Collections.Generic;
using Abp.Application.Services;
using Clc.Works.Dto;

namespace Clc.ArticleRecords
{
    public interface IArticleRecordAppService : IApplicationService
    {
        int Lend(int routeId, int routeWorkerId, int affairId, List<int> ids);
        // void ReturnArticles(int routeId, int routeWorkerId, string remark);       


        string GetArticleStatus(int articleId);

        //Task<List<ArticleArticleRecordSearchDto>> SearchByDay(int depotId, DateTime theDay);
        //Task<List<ArticleArticleRecordSearchDto>> SearchByArticleId(int depotId, int articleId, DateTime begin, DateTime end);
    }
}
