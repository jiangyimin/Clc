using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.ArticleRecords.Dto;
using Clc.Authorization;
using Clc.Fields;
using Clc.Routes;
using Clc.Runtime;
using Clc.Works;

namespace Clc.ArticleRecords
{
    [AbpAuthorize(PermissionNames.Pages_Article, PermissionNames.Pages_Arrange)]
    public class ArticleRecordAppService : ClcAppServiceBase, IArticleRecordAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<ArticleRecord> _recordRepository;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<RouteArticle> _routeArticleRepository;

        public ArticleRecordAppService(IRepository<ArticleRecord> recordRepository, 
            IRepository<Article> articleRepository,
            IRepository<RouteArticle> routeArticleRepository)    
        {
            _recordRepository = recordRepository;
            _articleRepository = articleRepository;
            _routeArticleRepository = routeArticleRepository;
        }

        public async Task<PagedResultDto<ArticleRecordDto>> GetArticlesAsync(PagedAndSortedResultRequestDto input)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            
            var query = _articleRepository.GetAllIncluding(x => x.ArticleType, x => x.ArticleRecord, x=>x.ArticleRecord.RouteWorker)
                .Where(a => a.DepotId == depotId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<ArticleRecordDto>(
                totalCount,
                entities.Select(MapToArticleRecordDto).ToList()
            );
        }

        public int Lend(int routeId, int routeWorkerId, List<int> ids, string workers)
        {
            foreach (int id in ids)
            {               
                ArticleRecord record = new ArticleRecord() {
                    RouteWorkerId = routeWorkerId,
                    ArticleId = id,
                    LendTime = DateTime.Now,
                    LendWorkers = workers
                };

                int recordId = _recordRepository.InsertAndGetId(record);
                Article article = _articleRepository.Get(id);
                article.ArticleRecordId = recordId;

                RouteArticle ra = new RouteArticle() {
                    RouteId = routeId,
                    RouteWorkerId = routeWorkerId,
                    ArticleRecordId = recordId
                };
                _routeArticleRepository.Insert(ra);
            }
            return ids.Count();
        }

        public string GetArticleStatus(int articleId) 
        {
            var article = _articleRepository.Get(articleId);

            if (article.ArticleRecordId.HasValue)
            {
                var record = _recordRepository.Get(article.ArticleRecordId.Value);
                if (!record.ReturnTime.HasValue)
                {
                    return string.Format("此物已于{0}被领用", record.LendTime.ToShortDateString());
                }
            }
            return null;
        }

        public int Return(List<int> recordIds, string workers)
        {
            int i = 0;
            foreach (var id in recordIds)
            {  
                var record = _recordRepository.Get(id);   
                if (!record.ReturnTime.HasValue) {
                    record.ReturnTime = DateTime.Now;
                    record.ReturnWorkers = workers;
                    _recordRepository.Update(record);
                    i++;
                }    
            }  
            return i; 
        }

        public async Task<List<ArticleRecordSearchDto>> SearchByDay(DateTime theDay)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var query = _recordRepository.GetAllIncluding(x => x.Article, x => x.RouteWorker)
                .Where(x => x.LendTime.Date == theDay && x.Article.DepotId == depotId);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return  entities.Select(MapToSearchDto).ToList(); 
        }

        public async Task<List<ArticleRecordSearchDto>> SearchByArticleId(int articleId, DateTime begin, DateTime end)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var query = _recordRepository.GetAllIncluding(x => x.Article, x => x.RouteWorker)
                .Where(x => x.ArticleId == articleId && x.LendTime.Date >= begin && x.LendTime.Date <= end);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return  entities.Select(MapToSearchDto).ToList(); 
        }

        #region util

        private ArticleRecordDto MapToArticleRecordDto(Article entity)
        {
            var dto = ObjectMapper.Map<ArticleRecordDto>(entity);

            var record = entity.ArticleRecord;
            if (record == null) return dto;

            var worker = WorkManager.GetWorker(record.RouteWorker.WorkerId);
            
            string l = $"领用人：{worker.Name} 领用时间：{record.LendTime.ToString("yyyy-MM-dd HH:mm")} ";
            string r = record.ReturnTime.HasValue ? $"【归还时间：{record.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm")}】" : "【未还】";
            dto.ArticleRecordInfo = l + r;
            return dto;
        }

        private ArticleRecordSearchDto MapToSearchDto(ArticleRecord record)
        {
            var worker = WorkManager.GetWorker(record.RouteWorker.WorkerId);
            
            var dto = new ArticleRecordSearchDto();
            dto.Article = record.Article.Name;
            dto.Worker = worker.Cn + ' ' + worker.Name;
            dto.LendTime = record.LendTime.ToString("yyyy-MM-dd HH:mm:ss");
            dto.ReturnTime = record.ReturnTime.HasValue ? record.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
            dto.LendWorkers = record.LendWorkers;
            dto.ReturnWorkers = record.ReturnWorkers;
            return dto;
        }
        #endregion
    }
}