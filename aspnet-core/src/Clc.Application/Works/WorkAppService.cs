using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.UI;
using Clc.Configuration;
using Clc.Fields;
using Clc.Routes;
using Clc.Runtime;
using Clc.Runtime.Cache;
using Clc.Types;
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
        private readonly IRepository<RouteArticle> _routeArticleRepository;
        private readonly IRepository<RouteTask> _routeTaskRepository;
        private readonly IRepository<Depot> _depotRepository;
        private readonly IRouteCache _routeCache;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IRouteTypeCache _routeTypeCache;

        private readonly IArticleCache _articleCache;
        private readonly IArticleTypeCache _articleTypeCache;
        private readonly IBoxCache _boxCache;
        private readonly IOutletCache _outletCache;

        public WorkAppService(IRepository<Signin> signinRepository,
            IRepository<Route> routeRepository,
            IRepository<RouteArticle> routeArticleRepository,
            IRepository<Depot> depotRepository,
            IRouteCache routeCache,
            IWorkRoleCache workRoleCache,
            IRouteTypeCache routeTypeCache,
            IArticleCache articleCache,
            IArticleTypeCache articleTypeCache,
            IBoxCache boxCache,
            IOutletCache outletCache)
        {
            _signinRepository = signinRepository;
            _routeRepository = routeRepository;
            _routeArticleRepository = routeArticleRepository;
            _depotRepository = depotRepository;
            _routeCache = routeCache;
            _workRoleCache = workRoleCache;
            _routeTypeCache = routeTypeCache;
            _articleCache = articleCache;
            _articleTypeCache = articleTypeCache;
            _boxCache = boxCache;
            _outletCache = outletCache;
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

            // For Captain (Maybe be Agent)
            if (WorkManager.IsCaptain(workerId))
            {
                dto.WorkerId = WorkManager.GetCaptainOrAgentId(workerId);
                dto.WorkerCn = WorkManager.GetWorkerCn(dto.WorkerId);
                return dto;
            }

            // Else for non-Captain
            dto.WorkerCn = WorkManager.GetWorkerCn(workerId);
            var aw = WorkManager.GetValidAffairWorker(depotId, workerId);
            if (aw != null)
            {
                dto.AffairId = aw.Affair.Id;
                dto.Status = aw.Affair.Status;
                dto.StartTime = ClcUtils.GetDateTime(aw.Affair.StartTime);
                dto.EndTime = ClcUtils.GetDateTime(aw.Affair.EndTime, aw.Affair.IsTomorrow);
                var lst = WorkManager.GetWorkersFromAffairId(aw.Affair.Id);
                dto.Workers = new List<MyWorkerDto>();
                foreach (int id in lst)
                {
                    var w = WorkManager.GetWorker(id);
                    dto.Workers.Add(new MyWorkerDto() 
                        {Cn = w.Cn, Name = w.Name, Rfid = w.Rfid, Photo = w.Photo != null ? Convert.ToBase64String(w.Photo) : null}
                    );
                }
            }
            return dto;
        }

        public string GetReportToManagers()
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            return WorkManager.GetReportToManagers(depotId);
        }

        #region Agent
        public string GetAgentString()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result; 
            int depotId = WorkManager.GetWorkerDepotId(workerId);
            string agentCn = WorkManager.GetDepotAgentCn(depotId);
            var worker = WorkManager.GetWorkerByCn(agentCn);
            if (worker == null) return null;
            return string.Format("{0} {1}", worker.Cn, worker.Name);
        }
        public async Task SetAgent(int workerId)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var depot = _depotRepository.Get(depotId);
            depot.AgentCn = WorkManager.GetWorker(workerId).Cn;
        }

        public async Task ResetAgent()
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var depot = _depotRepository.Get(depotId);
            depot.AgentCn = null;
        }
        #endregion

        
        #region signin

        public async Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
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
            if (worker.DepotId != depotId) return "此人不应在此运作中心签到";
            
            return WorkManager.DoSignin(depotId, worker.Id);
        }
        #endregion

        #region Article

        public async Task<List<RouteCDto>> GetRoutesForArticleAsync(DateTime carryoutDate, int affairId)
        { 
            var lst = await GetRoutesForArticle(carryoutDate, affairId);
            lst.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return lst;
        }

        public RouteWorkerMatchResult MatchWorkerForLend(DateTime carryoutDate, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var ret =  GetEqualRfidWorker(_routeCache.Get(carryoutDate, affairId, "Article"), rfid);
            if (ret.Item1 == null) {
                result.Message = "未安排任务";
                return result;
            }
                
            // RULE JUDGE
            var rt = _routeTypeCache[ret.Item1.RouteTypeId];
            if (!ClcUtils.NowInTimeZone(ret.Item1.StartTime, rt.LendArticleLead, rt.LendArticleDeadline)) {
               result.Message = "不在领物时间段";
               return result;
            }
            if (string.IsNullOrEmpty(ret.Item2.ArticleTypeList)) {
               result.Message = "此人不需要领还物";
               return result;
            }

            result.RouteMatched = new RouteMatchedDto(ret.Item1);
            result.WorkerMatched = ret.Item2;
            result.WorkerMatched2 = ret.Item3;
            result.Articles = GetArticles(ret.Item2.RouteWorkerId);
            result.Articles2 = GetArticles(ret.Item3.RouteWorkerId);
            return result;
        }
        
        public RouteWorkerMatchResult MatchWorkerForReturn(DateTime carryoutDate, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var ret =  GetEqualRfidWorker(_routeCache.Get(carryoutDate, affairId, "Article"), rfid);
            if (ret.Item1 == null) {
                result.Message = "未安排任务";
                return result;
            }
            // RULE JUDGE
            if (string.IsNullOrEmpty(ret.Item2.ArticleTypeList)) {
               result.Message = "此人不需要领还物";
               return result;
            }

            result.RouteMatched = new RouteMatchedDto(ret.Item1);
            result.WorkerMatched = ret.Item2;
            result.WorkerMatched2 = ret.Item3;
            result.Articles = GetArticles(ret.Item2.RouteWorkerId);
            result.Articles2 = GetArticles(ret.Item3.RouteWorkerId);
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

        public (string, RouteArticleCDto) MatchArticleForReturn(string rfid)
        {
            var article = _articleCache.GetList().FirstOrDefault(x => x.Rfid == rfid);
            if (article == null) return ("此Rfid没有对应的物品", null);
            if (!article.ArticleRecordId.HasValue) return ("此物品需要先领用", null);
            
            return ("", new RouteArticleCDto(article));
        }

        #endregion

        #region Box

        public async Task<List<RouteCDto>> GetRoutesForBoxAsync(DateTime carryoutDate, int affairId)
        { 
            var lst = await GetRoutesForBox(carryoutDate, affairId);
            lst.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            return lst;
        }

        public RouteWorkerMatchResult MatchWorkerForInBox(DateTime carryoutDate, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var ret =  GetEqualRfidWorker(_routeCache.Get(carryoutDate, affairId, "Box"), rfid);
            if (ret.Item1 == null) {
                result.Message = "未安排任务";
                return result;
            }
                
            // RULE JUDGE
            if (string.IsNullOrEmpty(ret.Item2.Duties) || !ret.Item2.Duties.Contains("尾箱")) {
               result.Message = "此人非尾箱交接人员";
               return result;
            }

            result.RouteMatched = new RouteMatchedDto(ret.Item1);
            result.WorkerMatched = ret.Item2;
            result.Boxes = new List<RouteBoxCDto>();
            return result;
        }

        public RouteWorkerMatchResult MatchWorkerForOutBox(DateTime carryoutDate, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var ret =  GetEqualRfidWorker(_routeCache.Get(carryoutDate, affairId, "Box"), rfid);
            if (ret.Item1 == null) {
                result.Message = "未安排任务";
                return result;
            }
                
            // RULE JUDGE
            if (string.IsNullOrEmpty(ret.Item2.Duties) || !ret.Item2.Duties.Contains("尾箱")) {
               result.Message = "此人非尾箱交接人员";
               return result;
            }

            result.RouteMatched = new RouteMatchedDto(ret.Item1);
            result.WorkerMatched = ret.Item2;
            result.Boxes = new List<RouteBoxCDto>();
            return result;
        }

        public (string, RouteBoxCDto) MatchInBox(DateTime carryoutDate, int affairId, int routeId, string rfid)
        {
            var box = _boxCache.GetList().FirstOrDefault(x => x.Cn == rfid);
            if (box == null) return ("没有对应的尾箱", null);

            var outlet = _outletCache[box.OutletId];
            foreach (var route in _routeCache.Get(carryoutDate, affairId, "Box"))
                foreach (var t in route.Tasks)
                    if (t.OutletId == box.OutletId && t.TaskTypeId == 2) 
                    {
                        var dto = new RouteBoxCDto() { 
                            TaskId = t.Id,
                            OutletId = outlet.Id,
                            Outlet = $"{outlet.Cn} {outlet.Name}",
                            BoxId = box.Id, 
                            DisplayText = box.Name,
                        };
                        return ("", dto);
                    }
            return ("此尾箱所属不在任务列表中", null);
        }

        public (string, RouteBoxCDto) MatchOutBox(DateTime carryoutDate, int affairId, int routeId, string rfid)
        {
            var box = _boxCache.GetList().FirstOrDefault(x => x.Cn == rfid);
            if (box == null) return ("没有对应的尾箱", null);
            if (!box.BoxRecordId.HasValue) return ("此尾箱需要先入库", null);

            var outlet = _outletCache[box.OutletId];
            foreach (var route in _routeCache.Get(carryoutDate, affairId, "Box"))
                foreach (var t in route.Tasks)
                    if (t.OutletId == box.OutletId && t.TaskTypeId == 1) 
                    {
                        var dto = new RouteBoxCDto() { 
                            TaskId = t.Id,
                            OutletId = outlet.Id,
                            Outlet = $"{outlet.Cn} {outlet.Name}",
                            BoxId = box.Id, 
                            DisplayText = box.Name,
                            RecordId = box.BoxRecordId.Value
                        };
                        return ("", dto);
                    }
            return ("此尾箱所属不在任务列表中", null);
        }

        #endregion

        #region private

        private (RouteCDto, WorkerMatchedDto, WorkerMatchedDto) GetEqualRfidWorker(List<RouteCDto> routes, string rfid)
        {           
            foreach (var route in routes)
            {
                foreach (var w in route.Workers) 
                {
                    var worker = WorkManager.GetWorker(w.WorkerId);
                    if (worker.Rfid == rfid) {
                        WorkerMatchedDto dto = new WorkerMatchedDto(w.Id, worker, _workRoleCache[w.WorkRoleId]);
                        WorkerMatchedDto dto2 = GetAnotherWorkerMathcedDto(route.Workers, _workRoleCache[w.WorkRoleId]);
                        return (route, dto, dto2);
                    }
                }
            }
            return (null, null, null);
        }

        private WorkerMatchedDto GetAnotherWorkerMathcedDto(List<RouteWorkerCDto> workers, WorkRole workRole)
        {
            string anotherRole = GetAnotherRoleName(workRole);
            if ( anotherRole!= null )
            {
                foreach (var w in workers)
                {
                    var role = _workRoleCache[w.WorkRoleId];
                    if (role.Name == anotherRole)
                        return new WorkerMatchedDto(w.Id, WorkManager.GetWorker(w.WorkerId), role);
                }
            }
            return null;
        }

        private string GetAnotherRoleName(WorkRole workRole)
        {
            string doubleRoles = SettingManager.GetSettingValueAsync(AppSettingNames.Rule.DoubleArticleRoles).Result;
            string[] array = doubleRoles.Split('|');
            if (array[0] == workRole.Name) return array[1];
            if (array[1] == workRole.Name) return array[0];
            return null;
        }

        private List<RouteArticleCDto> GetArticles(int workerId)
        {
            List<RouteArticleCDto> ret = new List<RouteArticleCDto>();
            var articles = _routeArticleRepository.GetAllIncluding(x => x.ArticleRecord).Where(x => x.RouteWorkerId == workerId).ToList();
            foreach (var a in articles)
            {
                var article = WorkManager.GetArticle(a.ArticleRecord.ArticleId);
                ret.Add(new RouteArticleCDto(article, a.ArticleRecordId, a.ArticleRecord.ReturnTime.HasValue));
            }
            return ret;
        }

        private async Task<List<RouteCDto>> GetRoutesForArticle(DateTime carryoutDate, int affairId)
        {
            try 
            {
                var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.Workers);
                query = ApplyWhere(query, carryoutDate, affairId);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                var lst = entities.Select(MapToDtoArticle).ToList();
                
                _routeCache.Set(carryoutDate, affairId, "Article", lst);           // Cached
                return lst;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private async Task<List<RouteCDto>> GetRoutesForBox(DateTime carryoutDate, int affairId)
        {
            try 
            {
                var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Vehicle, x => x.Workers, x => x.Tasks);
                query = ApplyWhere(query, carryoutDate, affairId);
                var entities = await AsyncQueryableExecuter.ToListAsync(query);
                var lst = entities.Select(MapToDtoBox).ToList();
                
                _routeCache.Set(carryoutDate, affairId, "Box", lst);           // Cached
                return lst;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private RouteCDto MapToDtoArticle(Route route)
        {
            var dto = ObjectMapper.Map<RouteCDto>(route);
            dto.Workers = new List<RouteWorkerCDto>();
            foreach (var w in route.Workers)
            {
                dto.Workers.Add(new RouteWorkerCDto() { Id = w.Id, WorkerId = w.WorkerId, WorkRoleId = w.WorkRoleId});
            }
            return dto;
        }

        private RouteCDto MapToDtoBox(Route route)
        {
            var dto = ObjectMapper.Map<RouteCDto>(route);
            dto.Workers = new List<RouteWorkerCDto>();
            foreach (var w in route.Workers)
                dto.Workers.Add(new RouteWorkerCDto() { Id = w.Id, WorkerId = w.WorkerId, WorkRoleId = w.WorkRoleId});

            dto.Tasks = new List<RouteTaskCDto>();
            foreach (var t in route.Tasks)
                dto.Tasks.Add(new RouteTaskCDto(t));

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