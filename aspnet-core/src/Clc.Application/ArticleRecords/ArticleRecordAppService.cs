using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc;
using Clc.Authorization;
using Clc.Fields;
using Clc.Routes;
using Clc.Runtime;
using Clc.Works.Dto;

namespace Clc.ArticleRecords
{
    [AbpAuthorize(PermissionNames.Pages_Article)]
    public class ArticleRecordAppService : ClcAppServiceBase, IArticleRecordAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<ArticleRecord> _recordRepository;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<RouteArticle> _routeArticleRepository;
        // private readonly IRepository<Article> _articleRepository;

        public ArticleRecordAppService(IRepository<ArticleRecord> recordRepository, 
            IRepository<Article> articleRepository,
            IRepository<RouteArticle> routeArticleRepository)    
        {
            _recordRepository = recordRepository;
            _articleRepository = articleRepository;
            _routeArticleRepository = routeArticleRepository;
        }

        // public async Task<List<ArticleListDto>> GetArticleList(int depotId, string sorting)
        // {
        //     var query = _articleRepository.GetAll().Where(a => a.DepotId == depotId).OrderBy(sorting);
        //     var entities = await AsyncQueryableExecuter.ToListAsync(query);

        //     var dtos = new List<ArticleListDto>(entities.Select(ObjectMapper.Map<ArticleListDto>).ToList());
        //     foreach (var dto in dtos)
        //     {
        //         dto.ArticleTypeName = DomainManager.GetArticleType(dto.ArticleTypeId).Name;
        //         dto.ArticleRecordInfo = GetArticleRecordInfo(dto.ArticleRecordId);
        //     }
        //     return dtos;
        // }

        public int Lend(int routeId, int routeWorkerId, int affairId, List<int> ids)
        {
            foreach (int id in ids)
            {               
                ArticleRecord record = new ArticleRecord() {
                    RouteWorkerId = routeWorkerId,
                    ArticleId = id,
                    LendTime = DateTime.Now,
                    AffairId = affairId
                };

                int recordId = _recordRepository.InsertAndGetId(record);
                Article article = _articleRepository.Get(recordId);
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
        // public void ReturnArticles(int routeId, int routeWorkerId, string remark)
        // {
        //     Route r = _routeRepository.Get(routeId);
        //     r.Status = "还物";
        //     RouteWorker rw = _routeWorkerRepository.Get(routeWorkerId);
        //     rw.ReturnTime = DateTime.Now;
        //     foreach (string id in rw.RecordList.Split())
        //     {
        //         ArticleRecord record = _recordRepository.Get(int.Parse(id));
        //         record.ReturnTime = DateTime.Now;
        //         record.Remark += $"【归还管理:{remark}】";
        //     }
        // }

        // public async Task<List<ArticleRecordSearchDto>> SearchByDay(int depotId, DateTime theDay)
        // {
        //     var query = _recordRepository.GetAll().Where(a => a.DepotId == depotId && a.LendTime.Date == theDay);
        //     var entities = await AsyncQueryableExecuter.ToListAsync(query);

        //     return MapToDto(entities);
 
        // }
        // public async Task<List<ArticleRecordSearchDto>> SearchByArticleId(int depotId, int articleId, DateTime begin, DateTime end)
        // {
        //     var query = _recordRepository.GetAll().Where(a => a.DepotId == depotId && a.ArticleId == articleId &&
        //         a.LendTime.Date >= begin && a.LendTime.Date <= end);
        //     var entities = await AsyncQueryableExecuter.ToListAsync(query);

        //     return MapToDto(entities);
        // }

        #region util

        // private string GetArticleRecordInfo(int? recordId)
        // {
        //     if (!recordId.HasValue) return null;

        //     var r = _recordRepository.Get(recordId.Value);

        //     string l = $"领用人：{r.WorkerName} 领用时间：{r.LendTime.ToString("yyyy-MM-dd HH:mm")} ";
        //     string h = r.ReturnTime.HasValue ? $"【归还时间：{r.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm")}】" : "【未还】";
        //     return l+h;
        // }

        // private List<ArticleRecordSearchDto> MapToDto(List<ArticleRecord> entities)
        // {
        //     List<ArticleRecordSearchDto> dtos = new List<ArticleRecordSearchDto>();
        //     foreach (var r in entities)
        //     {
        //         var a = DomainManager.GetArticle(r.DepotId, r.ArticleId);
        //         dtos.Add(new ArticleRecordSearchDto() {
        //             Worker = r.WorkerCn + " " + r.WorkerName,
        //             Article = a != null ? a.Cn + " " + a.Name : null,
        //             LendTime = r.LendTime.ToString("yyyy-MM-dd HH:mm:ss"),
        //             ReturnTime =  r.ReturnTime.HasValue ? r.ReturnTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
        //             Remark = r.Remark
        //         });
        //     }
        //     return dtos;
        // }
        #endregion
    }
}