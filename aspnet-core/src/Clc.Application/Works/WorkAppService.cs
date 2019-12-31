using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.Linq.Extensions;
using Clc.Affairs;
using Clc.Configuration;
using Clc.Extensions;
using Clc.Fields;
using Clc.Fields.Dto;
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
        private readonly ICustomerTaskTypeCache _customerTaskTypeCache;

        public WorkAppService(IRepository<Signin> signinRepository,
            IRepository<Route> routeRepository,
            IRepository<RouteArticle> routeArticleRepository,
            IRepository<RouteTask> routeTaskRepository,
            IRepository<Depot> depotRepository,
            IRouteCache routeCache,
            IWorkRoleCache workRoleCache,
            IRouteTypeCache routeTypeCache,
            IArticleCache articleCache,
            IArticleTypeCache articleTypeCache,
            IBoxCache boxCache,
            IOutletCache outletCache,
            ICustomerTaskTypeCache customerTaskTypeCache)
        {
            _signinRepository = signinRepository;
            _routeRepository = routeRepository;
            _routeArticleRepository = routeArticleRepository;
            _routeTaskRepository = routeTaskRepository;
            _depotRepository = depotRepository;

            _routeCache = routeCache;
            _workRoleCache = workRoleCache;
            _routeTypeCache = routeTypeCache;
            _articleCache = articleCache;
            _articleTypeCache = articleTypeCache;
            _boxCache = boxCache;
            _outletCache = outletCache;
            _customerTaskTypeCache = customerTaskTypeCache;
        }

        public bool VerifyUnlockPassword(string password)
        {
            var depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var unlockPassword = WorkManager.GetUnlockPassword(depotId); // Get DepotId and WorkCn
            return unlockPassword == password;
        }

        public bool AllowCardWhenCheckin()
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            return WorkManager.GetDepot(depotId).AllowCardWhenCheckin;
        }

        public string GetMyPhoto(int id)
        {
            var worker = WorkManager.GetWorker(id);
            if (worker.Photo != null)
                return Convert.ToBase64String(worker.Photo);
            else 
                return null;

        }
        public string GetVehiclePhoto(int id)
        {
            var v = WorkManager.GetVehicle(id);
            if (v.Photo != null)
                return Convert.ToBase64String(v.Photo);
            else 
                return null;
        }
        
        public MeDto GetMe()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result; 
            if (workerId == 0) 
                return new MeDto("system", "", "", 0);
            
            if (WorkManager.WorkerHasDefaultWorkRoleName(workerId, "队长"))
                workerId = WorkManager.GetCaptainOrAgentId(workerId);

            var worker = WorkManager.GetWorker(workerId);
            return new MeDto(worker.LoginRoleNames, worker.Cn, worker.Name, worker.DepotId);
        }

        public string GetToday()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        
        public AffairWorkDto FindDutyAffair()
        {
            var dto = new AffairWorkDto();
            int workerId = GetCurrentUserWorkerIdAsync().Result;
            dto.DepotId = WorkManager.GetWorker(workerId).DepotId;
            var affair = WorkManager.FindDutyAffairByWorkerId(workerId);
            if (affair == null) return dto;

            var wp = WorkManager.GetWorkplace(affair.WorkplaceId);
            return dto.SetAffair(affair, wp.Name, false);
        }

        public AffairWorkDto FindAltDutyAffair()
        {
            var dto = new AffairWorkDto();
            dto.AltCheck = true;
            int workerId = GetCurrentUserWorkerIdAsync().Result;
            var depot = WorkManager.GetDepot(WorkManager.GetWorker(workerId).DepotId);

            var depots = SettingManager.GetSettingValue(AppSettingNames.Rule.AltCheckinDepots);
            if (!depots.Split('|', ',').Contains(depot.Name)) return dto;
            var affair = WorkManager.FindAltDutyAffairByDepotId(depot.Id);
            if (affair == null) return dto;

            var wp = WorkManager.GetWorkplace(affair.WorkplaceId);
            dto.DepotId = depot.Id;
            
            return dto.SetAffair(affair, wp.Name, true);
        }
        
        public AffairWorkDto GetMyCheckinAffair()
        {
            var dto = new AffairWorkDto();
            int workerId = GetCurrentUserWorkerIdAsync().Result; 
            dto.DepotId = WorkManager.GetWorkerDepotId(workerId); 

            var affair = WorkManager.FindCheckinAffairByWorkerId(workerId);
            if (affair == null) return dto;
            var wp = WorkManager.GetWorkplace(affair.WorkplaceId);
            dto.Workers = GetWorkersInfo(affair);
            return dto.SetAffair(affair, wp.Name, false);
        }


        public List<RouteCacheItem> GetCachedRoutes(int wpId, DateTime carryoutDate, int depotId, int affairId)
        {
            var depots = WorkManager.GetShareDepots(wpId);
            var ret = new List<RouteCacheItem>();
            foreach (var depot in depots)
            {
                var lst = _routeCache.Get(carryoutDate, depot);
                lst.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                ret.AddRange(lst);
            }
            return ret;
        }

        public List<TempArticleDto> GetTempArticles(int affairId) 
        {
            List<TempArticleDto> dtos = new List<TempArticleDto>();
            var es = WorkManager.GetTempArticles(affairId);

            foreach (var s in es.Item1)
            {
                var dto = ObjectMapper.Map<TempArticleDto>(s);

                var routeInfo = dto.Issurer.Split(',')[0];
                foreach (var t in es.Item2) {
                    if (t.Issurer.Contains(routeInfo)) 
                        dto.TakeTime = t.EventTime;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        public (string, string) GetReportToManagers()
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            return (WorkManager.GetDepot(depotId).Name, WorkManager.GetReportToManagers(depotId));
        }

        public TaskReportDto GetTaskReportData()
        {
            var data = new TaskReportDto();

            // Route
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var routes = _routeCache.Get(DateTime.Now.Date, depotId);
            int count = 0;
            foreach (var r in routes) {
                count += r.Workers.Count;
            }
            data.Route = new CountPair(routes.Count, count);

            // Affair
            var ret = WorkManager.GetAffairReportData(DateTime.Now.Date, depotId);
            data.Affair = new CountPair(ret.Item1, ret.Item2);

            // Task (fee)
            var query = _routeTaskRepository.GetAllIncluding(x => x.Route, x => x.Outlet, x => x.Outlet.Customer, x => x.TaskType)
                .Where(x => x.Route.CarryoutDate == DateTime.Now.Date && x.Route.DepotId == depotId && x.TaskType.isTemporary == true);
            data.Tasks = ObjectMapper.Map<List<TemporaryTaskDto>>(query.ToList());

            var fee = 0;
            foreach (var t in data.Tasks)
                fee += t.Price.HasValue ? (int)t.Price : t.TaskTypeBasicPrice;
            data.Task = new CountPair(data.Tasks.Count, fee);

            return data;
        }

        public void SetReportDate()
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var depot = _depotRepository.Get(depotId);
            
            var t = SettingManager.GetSettingValue(AppSettingNames.TimeRule.ActivateTime);
            var time = ClcUtils.GetDateTime(t, true);

            depot.LastReportDate = time;
            _depotRepository.Update(depot);
        }
        
        public void SetReportTime(string depotName)
        {
            var d = WorkManager.GetDepotByName(depotName);
            if (d == null) return;

            var depot = _depotRepository.Get(d.Id);

            depot.LastReportDate = DateTime.Now;
            _depotRepository.Update(depot);
        }
        
        public async Task<List<TemporaryTaskDto>> GetFeeTasks(DateTime dt, string sorting)
        {
            // Task (fee)
            var query = _routeTaskRepository.GetAllIncluding(x => x.Route, x => x.Route.Depot, x => x.Outlet, x => x.Outlet.Customer, x => x.TaskType)
                .Where(x => x.Route.CarryoutDate == dt &&  x.TaskType.isTemporary == true); 
            query = query.OrderBy(sorting);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<TemporaryTaskDto>>(entities);
        }

        public async Task<List<TemporaryTaskDto>> GetFeeTasks(FeeTaskSearchRequestDto input, string sorting)
        {
            var query = _routeTaskRepository.GetAllIncluding(x => x.Route, x => x.Route.Depot, x => x.Outlet, x => x.Outlet.Customer, x => x.TaskType)
                .Where(x => x.Route.CarryoutDate >= input.Start && x.Route.CarryoutDate <= input.End &&  x.TaskType.isTemporary == true)
                .WhereIf(input.CustomerId.HasValue, x => x.Outlet.Customer.Id == input.CustomerId.Value)
                .WhereIf(input.DepotId.HasValue, x => x.Route.DepotId == input.DepotId.Value);
            query = query.OrderBy(sorting);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<TemporaryTaskDto>>(entities);
        }

        public async Task CaculateTasksPrice(DateTime dt)
        {
            var query = _routeTaskRepository.GetAllIncluding(x => x.Route, x => x.Route.Depot, x => x.Outlet, x => x.Outlet.Customer, x => x.TaskType)
                .Where(x => x.Route.CarryoutDate == DateTime.Now.Date &&  x.TaskType.isTemporary == true); 
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            foreach (var e in entities) 
            {
                if (e.Price.HasValue) continue;

                e.Price = GetDefaultPrice(e);
                await _routeTaskRepository.UpdateAsync(e);
            }
        }

        private float GetDefaultPrice(RouteTask t)
        {
            var lst = _customerTaskTypeCache.GetList().FindAll(x => x.DepotId.HasValue);
            var lstVoid = _customerTaskTypeCache.GetList().FindAll(x => !x.DepotId.HasValue);
            var type = lst.FindLast(x => x.DepotId.Value == t.Route.DepotId && x.TaskTypeId == t.TaskTypeId);
            var typeVoid = lstVoid.FindLast(x => x.TaskTypeId == t.TaskTypeId);

            if (type != null)
                return type.Price;
            
            if (typeVoid != null)
                return typeVoid.Price;
            
            return t.TaskType.BasicPrice;
        }

        #region Agent
        public string GetAgentString()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result; 
            int depotId = WorkManager.GetWorkerDepotId(workerId);
            string agentCn = WorkManager.GetDepot(depotId).AgentCn;
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

        public List<ComboboxItemDto> GetLeaders() 
        {
            var leaders = WorkManager.GetWorkersByDefaultWorkRoleName("公司领导");
            var lst = new List<ComboboxItemDto>();
            if (leaders != null)
                foreach (var w in leaders)
                    lst.Add(new ComboboxItemDto { Value = w.Id.ToString(), DisplayText = $"{w.Cn} {w.Name}"});

            return lst;
        }
        public List<WorkplaceDto> GetDoors()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result;
            var lst = WorkManager.GetDoors(workerId);
            return ObjectMapper.Map<List<WorkplaceDto>>(lst);
        }

        public SimpleWorkerDto GetWorkerByRfid(string rfid)
        {
            var worker = WorkManager.GetWorkerByRfid(rfid);
            if (worker != null) 
                return new SimpleWorkerDto() { Name = string.Format("{0} {1}", worker.Cn, worker.Name) };
            return null;
        }

        #endregion

        #region signin

        public async Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var query = _signinRepository.GetAllIncluding(x => x.Worker);
            query = query.Where(x => x.CarryoutDate == carryoutDate && x.DepotId == depotId);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            entities.Sort((a, b) => a.Worker.Cn.CompareTo(b.Worker.Cn));
            // var list = entities.Distinct(new LambdaEqualityComparer<Signin>((a, b) => a.Worker.Cn == b.Worker.Cn)).ToList();
            return ObjectMapper.Map<List<SigninDto>>(entities);
        }

        public (bool, string) SigninByRfid(string rfid) 
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            Worker worker = WorkManager.GetWorkerByRfid(rfid);

            if (worker == null) return (false, "找不到此人");
            if (worker.DepotId != depotId) return (false, "此人不应在此运作中心签到");
            
            return WorkManager.DoSignin(depotId, worker.Id, "刷卡");
        }

        public (bool, string) SigninByFinger(string finger) 
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            int score = 0;
            string index = null;
            Worker worker = WorkManager.GetWorkerByFinger(depotId, finger, ref index, ref score);
            if (worker == null) return (false, "找不到此人");
            
            var ret = WorkManager.DoSignin(depotId, worker.Id, "指纹");
            if (ret.Item1)
                return (true, ret.Item2 + string.Format("({0},得分{1})", index, score));
            else
                return ret;
        }

        public (bool, string) VerifyFinger(string finger, string workerCn) 
        {
            Worker worker = WorkManager.GetWorkerByCn(workerCn);
            var mR = WorkManager.MatchFinger(finger, worker.Id);
            return mR;
        }

        public (bool, string) CheckinByRfid(string rfid, DateTime carryoutDate, int depotId, int affairId) 
        {
            Worker worker =  WorkManager.GetWorkerByRfid(rfid);
            if (worker == null) return (false, "找不到此人");
            var aw = WorkManager.GetCacheAffairWorker(carryoutDate, depotId, affairId, worker.Id);
            if (aw.Item2 == null) return (false, "此人没被安排在此任务中");
            if (!ClcUtils.NowInTimeZone(aw.Item1.StartTime, aw.Item1.EndTime)) return (false, "没在任务时段");
            WorkManager.DoCheckin(aw.Item1, aw.Item2, worker.Id);
            return (true, "刷卡验入成功");
        }

        public (bool, string) CheckinByFinger(string finger, int workerId, DateTime carryoutDate, int depotId, int affairId) 
        {
            var mR = WorkManager.MatchFinger(finger, workerId);
            if (!mR.Item1) return mR;

            var aw = WorkManager.GetCacheAffairWorker(carryoutDate, depotId, affairId, workerId);
            if (aw.Item2 == null) return (false, "此人没被安排在此任务中");
            if (!ClcUtils.NowInTimeZone(aw.Item1.StartTime, aw.Item1.EndTime)) return (false, "没在任务时段");
            WorkManager.DoCheckin(aw.Item1, aw.Item2, workerId);
            return (true, "验入成功!" + mR.Item2);
        }
 
        public void checkout(int workerId, DateTime carryoutDate, int depotId, int affairId) 
        {
            var aw = WorkManager.GetCacheAffairWorker(carryoutDate, depotId, affairId, workerId);
            if (aw.Item2 == null) return;
            WorkManager.DoCheckout(aw.Item1, aw.Item2, workerId);
        }
        
        public (bool, string) ConfirmByFinger(string finger, int workerId)
        {
            var ret = WorkManager.MatchFinger(finger, workerId);
            return ret;
        }

        #endregion

        #region Article

        public RouteWorkerMatchResult MatchWorkerForArticle(bool isLend, int wpId, DateTime carryoutDate, int depotId, string rfid, int routeId)
        {
            var result = new RouteWorkerMatchResult();
            (string, RouteCacheItem, RouteWorkerCacheItem, RouteWorkerCacheItem) found = (null, null, null, null);
            if (routeId > 0) 
                found = FindEqualRfidWorkerForArticle(isLend, _routeCache.Get(carryoutDate, depotId), rfid, routeId);
            else
                foreach (var id in WorkManager.GetShareDepots(wpId))
                {
                    var routes = _routeCache.Get(carryoutDate, id);
                    found = FindEqualRfidWorkerForArticle(isLend, routes, rfid);
                    if (found.Item1 == null) break;
                }

            // Error, Return
            if (found.Item1 != null) {
                result.Message = found.Item1;
                return result;
            }

            result.RouteMatched = new MatchedRouteDto(found.Item2);
            var w = found.Item3;
            result.WorkerMatched = new MatchedWorkerDto(w.Id, WorkManager.GetWorker(w.GetFactWorkerId()), _workRoleCache[w.WorkRoleId]);
            result.Articles = GetArticles(found.Item2.Id, w.Id);
            w = found.Item4;
            if (w != null)
            {
                result.WorkerMatched2 = new MatchedWorkerDto(w.Id, WorkManager.GetWorker(w.GetFactWorkerId()), _workRoleCache[w.WorkRoleId]);
                result.Articles2 = GetArticles(found.Item2.Id, w.Id);
            }
            return result;
        }
        
        public RouteWorkerMatchResult MatchWorkerForTakeTempArticle(DateTime carryoutDate, int depotId, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var worker = WorkManager.GetWorkerByRfid(rfid);
            if (worker == null) { 
                result.Message = "无此RFID"; return result;
            };

            if (WorkManager.HadTake(affairId, worker)) {
                result.Message = "你已取物"; return result;
            }

            var stores = WorkManager.GetStores(affairId, worker);
            if (stores == null || stores.Count < 2) {
                result.Message = "需要双人操作"; return result;
            }

            var routeInfo = stores[0].Issurer.Split(',')[0];
            var cn = stores[0].Issurer.Split(',')[1].Split()[0];
            result.RouteMatched = new MatchedRouteDto(routeInfo);
            result.WorkerMatched = new MatchedWorkerDto(WorkManager.GetWorkerByCn(cn));
            result.Articles = GetArticles(stores[0].Description);
 
            cn = stores[1].Issurer.Split(',')[1].Split()[0];
            result.WorkerMatched2 = new MatchedWorkerDto(WorkManager.GetWorkerByCn(cn));
            result.Articles2 = GetArticles(stores[1].Description);
            return result;
        }

        public RouteWorkerMatchResult MatchWorkerForStoreTempArticle(DateTime carryoutDate, int depotId, int affairId, string rfid)
        {
            var result = new RouteWorkerMatchResult();
            var worker = WorkManager.GetWorkerByRfid(rfid);
            if (worker == null) { 
                result.Message = "无此RFID"; return result;
            };
            if (WorkManager.HadStore(affairId, worker)) {
                result.Message = "你已存物"; return result;
            }

            // found
            (string, RouteCacheItem, RouteWorkerCacheItem, RouteWorkerCacheItem) found = ("找不到任务", null, null, null);
            foreach (var route in _routeCache.Get(carryoutDate, worker.DepotId))
            {
                foreach (var rw in route.Workers) 
                {
                    if (WorkManager.GetWorker(rw.GetFactWorkerId()).Rfid == rfid) {
                        if (string.IsNullOrEmpty(_workRoleCache[rw.WorkRoleId].ArticleTypeList)) {
                            found = ("此人不需要领还物", null, null, null); break;
                        }
                            
                        RouteWorkerCacheItem rw2 = FindAnotherRouteWorker(route.Workers, _workRoleCache[rw.WorkRoleId]);
                        if (rw2 == null)
                            found = ("临时存取需要双人操作", null, null, null);
                        else
                            found = (null, route, rw, rw2);
                        break;
                    }
                }
            }

            // Error, Return
            if (found.Item1 != null) {
                result.Message = found.Item1; return result;
            }

            result.RouteMatched = new MatchedRouteDto(found.Item2);
            var w = found.Item3;
            result.WorkerMatched = new MatchedWorkerDto(w.Id, WorkManager.GetWorker(w.GetFactWorkerId()), _workRoleCache[w.WorkRoleId]);
            result.Articles = GetArticles(found.Item2.Id, w.Id);
            w = found.Item4;
            result.WorkerMatched2 = new MatchedWorkerDto(w.Id, WorkManager.GetWorker(w.GetFactWorkerId()), _workRoleCache[w.WorkRoleId]);
            result.Articles2 = GetArticles(found.Item2.Id, w.Id);
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

        public (string, int) MatchArticleForReturn(string rfid)
        {
            var article = _articleCache.GetList().FirstOrDefault(x => x.Rfid == rfid);
            if (article == null) return ("此Rfid没有对应的物品", 0);            
            return ("", article.Id);
        }

        #endregion

        #region Box

        public RouteWorkerMatchResult MatchWorkerForBox(int wpId, DateTime carryoutDate, int depotId, int affairId, string rfid)
        {
            var depots = WorkManager.GetShareDepots(wpId);
            var result = new RouteWorkerMatchResult();
            (RouteCacheItem, RouteWorkerCacheItem, RouteWorkerCacheItem) found = (null, null, null);
            foreach (var dopotId in depots)
            {
                found = FindEqualRfidWorker(_routeCache.Get(carryoutDate, depotId), rfid);
                if (found.Item1 != null) break;
            }

            if (found.Item1 == null) 
            {
                result.Message = "还未安排在激活的任务中";
                return result;
            }
                
            // RULE JUDGE
            var wr = _workRoleCache[found.Item2.WorkRoleId];
            if (string.IsNullOrEmpty(wr.Duties) || !wr.Duties.Contains("尾箱")) {
               result.Message = "此人非尾箱交接人员";
               return result;
            }

            result.RouteMatched = new MatchedRouteDto(found.Item1);
            var w = found.Item2;
            result.WorkerMatched = new MatchedWorkerDto(w.Id, WorkManager.GetWorker(w.WorkerId), _workRoleCache[w.WorkRoleId]);
            result.Boxes = new List<RouteBoxCDto>();
            return result;
        }

        public string InBox(int taskId, string rfid, string workers)
        {
            var box = _boxCache.GetList().FirstOrDefault(x => x.Cn == rfid);
            if (box == null) return "没有对应的尾箱";
            
            return null;
        }

        public (string, RouteBoxCDto) MatchOutBox(DateTime carryoutDate, int affairId, int routeId, string rfid)
        {
            var box = _boxCache.GetList().FirstOrDefault(x => x.Cn == rfid);
            if (box == null) return ("没有对应的尾箱", null);
            if (!box.BoxRecordId.HasValue) return ("此尾箱需要先入库", null);

            return (null, null);

            // var outlet = _outletCache[box.OutletId];
            // foreach (var route in _routeCache.Get(carryoutDate, affairId, "Box"))
            //     foreach (var t in route.Tasks)
            //         if (t.OutletId == box.OutletId && t.TaskTypeId == 1) 
            //         {
            //             var dto = new RouteBoxCDto() { 
            //                 TaskId = t.Id,
            //                 OutletId = outlet.Id,
            //                 Outlet = $"{outlet.Cn} {outlet.Name}",
            //                 BoxId = box.Id, 
            //                 DisplayText = box.Name,
            //                 RecordId = box.BoxRecordId.Value
            //             };
            //             return ("", dto);
            //         }
            // return ("此尾箱所属不在任务列表中", null);
        }

        #endregion

        #region private
        private (string, RouteCacheItem, RouteWorkerCacheItem, RouteWorkerCacheItem) FindEqualRfidWorkerForArticle(bool isLend, List<RouteCacheItem> routes, string rfid, int routeId = 0)
        {           
            foreach (var route in routes)
            {
                if (routeId != 0 && route.Id != routeId) continue;
               
                if (routeId == 0) 
                {
                    // 时间段 JUDGE
                    var rt = _routeTypeCache[route.RouteTypeId];
                    if (isLend && !ClcUtils.NowInTimeZone(route.StartTime, rt.LendArticleLead, rt.LendArticleDeadline)) continue;
                    var span = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.ReturnDeadline));
                    if (!isLend && DateTime.Now > ClcUtils.GetDateTime(route.EndTime).AddMinutes(span)) continue;
                }
                
                foreach (var rw in route.Workers) 
                {
                    var worker = WorkManager.GetWorker(rw.GetFactWorkerId());
                    if (worker.Rfid == rfid) {
                        if (string.IsNullOrEmpty(_workRoleCache[rw.WorkRoleId].ArticleTypeList))
                            return ("此人不需要领还物", null, null, null);

                        RouteWorkerCacheItem rw2 = FindAnotherRouteWorker(route.Workers, _workRoleCache[rw.WorkRoleId]);
                        return (null, route, rw, rw2);
                    }
                }

            }
            return ("无任务或不在时间段", null, null, null);
        }

        private (RouteCacheItem, RouteWorkerCacheItem, RouteWorkerCacheItem) FindEqualRfidWorker(List<RouteCacheItem> routes, string rfid, int routeId = 0)
        {           
            foreach (var route in routes)
            {
                if (routeId != 0 && route.Id != routeId) continue;


                foreach (var w in route.Workers) 
                {
                    var worker = WorkManager.GetWorker(w.WorkerId);
                    if (worker.Rfid == rfid) {
                        RouteWorkerCacheItem w2 = FindAnotherRouteWorker(route.Workers, _workRoleCache[w.WorkRoleId]);
                        return (route, w, w2);
                    }
                }
            }
            return (null, null, null);
        }

        private RouteWorkerCacheItem FindAnotherRouteWorker(List<RouteWorkerCacheItem> workers, WorkRole workRole)
        {
            string anotherRole = GetAnotherRoleName(workRole);
            if ( anotherRole!= null )
            {
                foreach (var w in workers)
                {
                    var role = _workRoleCache[w.WorkRoleId];
                    if (role.Name == anotherRole)
                        return w;
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

        private List<RouteArticleCDto> GetArticles(int routeId, int workerId)
        {
            List<RouteArticleCDto> ret = new List<RouteArticleCDto>();
            var articles = _routeArticleRepository.GetAllIncluding(x => x.ArticleRecord).Where(x => x.RouteId == routeId && x.RouteWorkerId == workerId).ToList();
            foreach (var a in articles)
            {
                var article = WorkManager.GetArticle(a.ArticleRecord.ArticleId);
                ret.Add(new RouteArticleCDto(article, a.ArticleRecordId, a.ArticleRecord.ReturnTime.HasValue));
            }
            return ret;
        }
     
        private List<RouteArticleCDto> GetArticles(string strList)
        {
            List<RouteArticleCDto> ret = new List<RouteArticleCDto>();
            var articles = strList.Split(',');
            foreach (var a in articles)
            {
                if (a.Length == 0) continue;

                var article = WorkManager.GetArticle(a.Split(' ')[0]);
                if (article != null)
                    ret.Add(new RouteArticleCDto(article, 0, false));
            }
            return ret;
        }
     
     
        private string GetWorkersInfo(AffairCacheItem affair)
        {
            string info = null;
            foreach (var aw in affair.Workers)
            {
                info += string.Format("{0} ", WorkManager.GetWorker(aw.WorkerId).Name);
            }
            return info;
        }

        #endregion
    }
}