using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;
using Clc.Fields;
using Clc.Routes;
using Clc.Runtime;
using Clc.Runtime.Cache;
using Clc.Works.Dto;

namespace Clc.Works
{

    [AbpAuthorize]
    public class WorkAppService : ClcAppServiceBase, IWorkAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<Signin> _signinRepository;
        private readonly IRepository<Route> _routeRepository;
        private readonly IRouteCache _routeCache;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IRouteTypeCache _routeTypeCache;

        private readonly IArticleCache _articleCache;
        private readonly IArticleTypeCache _articleTypeCache;
        private readonly IBoxCache _boxCache;

        public WorkAppService(IRepository<Signin> signinRepository,
            IRepository<Route> routeRepository,
            IRouteCache routeCache,
            IWorkRoleCache workRoleCache,
            IRouteTypeCache routeTypeCache,
            IArticleCache articleCache,
            IArticleTypeCache articleTypeCache,
            IBoxCache boxCache)
        {
            _signinRepository = signinRepository;
            _routeRepository = routeRepository;
            _routeCache = routeCache;
            _workRoleCache = workRoleCache;
            _routeTypeCache = routeTypeCache;
            _articleCache = articleCache;
            _articleTypeCache = articleTypeCache;
            _boxCache = boxCache;
        }

        public bool VerifyUnlockPassword(string password)
        {
            var depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var unlockPassword = WorkManager.GetUnlockPassword(depotId); // Get DepotId and WorkCn
            return unlockPassword == password;
        }

        public string GetTodayString()
        {
            return DateTime.Now.Date.ToString("yyyy-MM-dd");
        }
        public DateTime getNow()
        {
            return DateTime.Now;
        }

        public MyWorkDto GetMyWork()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result; 
            int depotId = WorkManager.GetWorkerDepotId(workerId);         
            var dto = new MyWorkDto();
            dto.WorkerCn = WorkManager.GetWorkerCn(workerId);

            var aw = WorkManager.GetValidAffairWorker(depotId, workerId);
            if (aw != null)
            {
                dto.AffairId = aw.Affair.Id;
                dto.Status = aw.Affair.Status;
                dto.StartTime = ClcUtils.GetDateTime(aw.Affair.StartTime);
                dto.EndTime = ClcUtils.GetDateTime(aw.Affair.EndTime, aw.Affair.IsTomorrow);
                dto.Workers = WorkManager.GetWorkersFromAffairId(aw.Affair.Id);
            }
            return dto;
        }

        #region signin

        public async Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate)
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var query = _signinRepository.GetAllIncluding(x => x.Worker);
            query = query.Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            entities.Sort((a, b) => a.Worker.Cn.CompareTo(b.Worker.Cn));
            // var list = entities.Distinct(new LambdaEqualityComparer<Signin>((a, b) => a.Worker.Cn == b.Worker.Cn)).ToList();
            return ObjectMapper.Map<List<SigninDto>>(entities);
        }

        public string SigninByRfid(string rfid) 
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            Worker worker = WorkManager.GetWorkerByRfid(rfid);

            if (worker == null) return "找不到此人";
            if ( (!worker.LoanDepotId.HasValue && worker.DepotId != depotId) 
                || (worker.LoanDepotId.HasValue) && worker.LoanDepotId != depotId ) return "此人不应在此运作中心签到";
            
            return WorkManager.DoSignin(depotId, worker.Id);
        }
        #endregion

        #region Route for Article and Box

        public async Task<List<RouteCDto>> GetRoutesForLendAsync(DateTime carryoutDate, int affairId)
        { 
            var lst = await GetRoutesForArticle(carryoutDate, affairId);
            lst.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return lst;
        }

        public async Task<List<RouteCDto>> GetRoutesForReturnAsync(DateTime carryoutDate, int affairId)
        { 
            var lst = await GetRoutesForArticle(carryoutDate, affairId);
            lst.Sort((a, b) => a.EndTime.CompareTo(b.EndTime));
            return lst;
        }

        public RouteWorkerMatchResult MatchWorkerForLend(DateTime carryoutDate, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var ret =  GetEqualRfidWorker(_routeCache.Get(carryoutDate, affairId), rfid);
            // RULE JUDGE
            var rt = _routeTypeCache[ret.Item1.RouteTypeId];
            if (!ClcUtils.NowInTimeZone(ret.Item1.StartTime, rt.LendArticleLead, rt.LendArticleDeadline)) {
               result.Message = "不在领物时间段";
               return result;
            }
            if (string.IsNullOrEmpty(ret.Item2.ArticleTypeList)) {
               result.Message = "此人不需要领物";
               return result;
            }

            result.RouteMatched = new RouteMatchedDto(ret.Item1);
            result.WorkerMatched = ret.Item2;
            return result;
        }

        public (string, RouteArticleCDto) MatchArticleForLend(string workerCn, string vehicleCn, string routeName, string articleTypeList, string rfid)
        {
            var article = _articleCache.GetList().FirstOrDefault(x => x.Rfid == rfid);
            if (article == null) return ("此Rfid没有对应的物品", null);

            var type = _articleTypeCache[article.ArticleTypeId];
            if (!articleTypeList.Contains(type.Cn)) return ("不允许领用此物品类型", null);

            if (!string.IsNullOrEmpty(article.BindInfo))
            {
                switch (type.BindStyle) {
                case "人":
                    if (!article.BindInfo.Contains(workerCn)) return ("此物品未绑定到此人", null);
                    break;
                case "车":
                    if (!article.BindInfo.Contains(vehicleCn)) return ("此物品未绑定到此车", null);
                    break;
                case "线":
                    if (!article.BindInfo.Contains(routeName)) return ("此物品未绑定到此线路", null);
                    break;
                default:
                    throw new InvalidOperationException("无此绑定类型");
                }
            }
            return ("", new RouteArticleCDto(article));
        }

        // 
        private (RouteCDto, WorkerMatchedDto) GetEqualRfidWorker(List<RouteCDto> routes, string rfid)
        {           
            foreach (var route in routes)
            {
                foreach (var w in route.Workers) 
                {
                    var worker = WorkManager.GetWorker(w.WorkerId);
                    if (worker.Rfid == rfid) {
                        WorkerMatchedDto dto = new WorkerMatchedDto(w.Id, worker, _workRoleCache[w.WorkRoleId].ArticleTypeList);
                        return (route, dto);
                    }
                }
            }
            return (null, null);
        }

        private async Task<List<RouteCDto>> GetRoutesForArticle(DateTime carryoutDate, int affairId)
        {
            try 
            {
                var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.Workers, x => x.Articles);
                query = ApplyWhere(query, carryoutDate, affairId);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                var lst = entities.Select(MapToDto).ToList();
                _routeCache.Set(carryoutDate, affairId, lst);           // Cached
                return lst;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private RouteCDto MapToDto(Route route)
        {
            var dto = ObjectMapper.Map<RouteCDto>(route);
            dto.Workers = new List<RouteWorkerCDto>();
            dto.Articles = new List<RouteArticleCDto>();
            foreach (var w in route.Workers)
            {
                dto.Workers.Add(new RouteWorkerCDto() { Id = w.Id, WorkerId = w.WorkerId, WorkRoleId = w.WorkRoleId});
                // dto.WorkerList += string.Format("{0}({1}) ", WorkManager.GetWorker(w.WorkerId).Name, _workRoleCache[w.WorkRoleId].Name);
            }
            foreach (var a in route.Articles)
            {
                // dto.Articles.Add(new RouteArticleCDto());
                // dto.ArticleList += "1 ";
            }
            return dto;
        }
        private IQueryable<Route> ApplyWhere(IQueryable<Route> query, DateTime carryoutDate, int affairId)
        {
            List<int> depots = WorkManager.GetShareDepods(affairId);
            var q = query.Where(x => x.CarryoutDate == carryoutDate &&　depots.Contains(x.DepotId)　&& x.Status == "激活");
            return q;
        }

        #endregion
    }
}