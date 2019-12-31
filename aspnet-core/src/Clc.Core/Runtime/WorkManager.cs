using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Clc.Affairs;
using Clc.Clients;
using Clc.Configuration;
using Clc.Fields;
using Clc.Runtime;
using Clc.Runtime.Cache;

namespace Clc.Works
{
    public class WorkManager : DomainService, IDomainService
    {
        private readonly IWorkerCache _workerCache;
        private readonly IVehicleCache _vehicleCache;
        private readonly IWorkplaceCache _workplaceCache;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IDepotCache _depotCache;
        private readonly IArticleCache _articleCache;
        private readonly IPostCache _postCache;
        private readonly IBoxCache _boxCache;

        private readonly ISigninCache _signinCache;
        private readonly IRepository<Signin> _signinRepository;

        private readonly IAffairCache _affairCache;
        private readonly IRepository<Affair> _affairRepository;
        private readonly IRepository<AffairWorker> _affairWorkerRepository;
        private readonly IRepository<AffairTask> _affairTaskRepository;
        private readonly IRepository<AffairEvent> _affairEventRepository;
        private readonly IRepository<AskDoorRecord> _askdoorRepository;

        public WorkManager(IWorkerCache workerCache,
            IVehicleCache vehicleCache,
            IWorkplaceCache workplaceCache,
            IWorkRoleCache workRoleCache,
            IDepotCache depotCache,
            IArticleCache articleCache,
            IPostCache postCache,
            IBoxCache boxCache,
            ISigninCache signinCache,
            IRepository<Signin> signinRepository,
            IAffairCache affairCache,
            IRepository<Affair> affairRepository,
            IRepository<AffairWorker> affairWorkerRepository,
            IRepository<AffairTask> affairTaskRepository,
            IRepository<AffairEvent> affairEventRepository,
            IRepository<AskDoorRecord> askdoorRepository)
        {
            _workerCache = workerCache;
            _vehicleCache = vehicleCache;
            _workplaceCache = workplaceCache;
            _workRoleCache = workRoleCache;
            _depotCache = depotCache;
            _articleCache = articleCache;
            _postCache = postCache;
            _boxCache = boxCache;

            _signinCache = signinCache;
            _signinRepository = signinRepository;

            _affairCache = affairCache;
            _affairRepository = affairRepository;
            _affairWorkerRepository = affairWorkerRepository;
            _affairTaskRepository = affairTaskRepository;
            _affairEventRepository = affairEventRepository;

            _askdoorRepository = askdoorRepository;
        }

        #region Worker

        public Worker GetWorker(int workerId)
        {
            return _workerCache[workerId];
        }

        public Worker GetWorkerByRfid(string rfid) 
        {
            var w = _workerCache.GetList().Find(x => !string.IsNullOrEmpty(x.Rfid) && x.Rfid == rfid);
            if (w != null) return _workerCache[w.Id];
            return null;
        }

        public Worker GetWorkerByFinger(int depotId, string finger, ref string index, ref int maxScore)
        {
            byte[] src = StringToByte(finger, 0);
            foreach (var w in _workerCache.GetList().Where(x => x.DepotId == depotId))
            {
                var ret = MatchFinger(src, w.Finger, w.Finger2);
                if (ret.Item1 > 50) 
                {
                    index = ret.Item2;
                    maxScore = ret.Item1;
                    return _workerCache[w.Id];
                }
            }

            return null;
        }

        public Worker GetWorkerByCn(string cn)
        {
            var w = _workerCache.GetList().Find(x => x.Cn == cn);
            if (w != null) return _workerCache[w.Id];
            return null;
        }

        public int GetWorkerDepotId(int workerId)
        {
            return _workerCache[workerId].DepotId;
        }

        public string GetWorkerCn(int workerId)
        {
            return _workerCache[workerId].Cn;
        }

        public string GetWorkerName(int workerId)
        {
            return _workerCache[workerId].Name;
        }

        public string GetWorkerPostAppName(Worker worker)
        {
            return _postCache[worker.PostId].AppName;
        }

        public List<WorkerCacheItem> GetWorkersByDefaultWorkRoleName(string name)
        {
            var lst = new List<WorkerCacheItem>();
            foreach (var w in _workerCache.GetList().FindAll(x => x.IsActive == true))
            {
                var roleName = _postCache[w.PostId].DefaultWorkRoleName;
                if (!string.IsNullOrEmpty(roleName) && roleName == name)
                    lst.Add(w);
            }
            return lst;
        } 
        
        public bool WorkerHasDefaultWorkRoleName(int workerId, string name)
        {
           var post = _postCache[_workerCache[workerId].PostId];
           return post.DefaultWorkRoleName == name;
        }

        public int GetCaptainOrAgentId(int workerId)
        {
            var worker = _workerCache[workerId];
            var depot = _depotCache[worker.DepotId];
            if (string.IsNullOrEmpty(depot.AgentCn))
                return workerId;
            else
                return GetWorkerByCn(depot.AgentCn).Id;
        }
        
        #endregion

        #region Depot, Workplace, Vehicle

        public Vehicle GetVehicle(int id)
        {
            return _vehicleCache[id];
        }
        
        public Depot GetDepot(int depotId)
        {
            return _depotCache[depotId];
        }
        public Depot GetDepotByName(string name)
        {
            return _depotCache.GetList().Find(x => x.Name == name);
        }
        public List<int> GetShareDepots(int wpId)
        {
            var wp = _workplaceCache[wpId];
            var depots = new List<int>() { wp.DepotId };
            if (!string.IsNullOrEmpty(wp.ShareDepotList))
            {
                var lst = _depotCache.GetList();
                foreach (var cn in wp.ShareDepotList.Split())
                {
                    depots.Add(lst.First(x => x.Cn == cn).Id);
                }
            }
            return depots;
        }
        
        public Workplace GetWorkplace(int id)
        {
            return _workplaceCache[id];
        }

        public List<Workplace> GetDoors(int workerId)
        {
            var depotId = _workerCache[workerId].DepotId;
            return _workplaceCache.GetList().FindAll(x => x.DepotId == depotId && !string.IsNullOrWhiteSpace(x.DoorIp)).ToList();
        }

        public string GetUnlockPassword(int depotId)
        {
            return _depotCache[depotId].UnlockScreenPassword;
        }

        public bool IsInDepotRadius(int depotId, float lat, float lon)
        {
            int radius = int.Parse(SettingManager.GetSettingValue(AppSettingNames.Rule.Radius));

            var depot = _depotCache[depotId];
            if (!depot.Latitude.HasValue || !depot.Latitude.HasValue) return false;

            int dist = (int)ClcUtils.GetDistance(lat, lon, depot.Latitude.Value, depot.Longitude.Value);

            if (depot.Radius.HasValue) radius = depot.Radius.Value;

            if (dist > radius) return false;

            return true;
        }

        public string GetVaultName(int depotId) 
        {
            var depotName = _depotCache[depotId].Name;
            var wp = _workplaceCache.GetList().FirstOrDefault(x => x.DepotId == depotId && x.Name.Contains("金库"));
            if (wp == null) return null;
            return depotName + wp.Name;
        }

        #endregion

        #region Signin/Checkin/Door
        public (bool, string) DoSignin(int depotId, int workerId, string style)
        {
            // Get Signin 
            var signin = _signinRepository.GetAll()
                .Where(s => s.CarryoutDate == DateTime.Today && s.DepotId == depotId && s.WorkerId == workerId)
                .LastOrDefault();

            if (signin == null)
            {
                signin = new Signin() { DepotId = depotId, CarryoutDate = DateTime.Today, WorkerId = workerId, SigninTime = DateTime.Now, SigninStyle = style }; 
                _signinRepository.Insert(signin);
                if (ClcUtils.IsMorning(DateTime.Now))
                    return (true, "上午时段签到成功");
                else
                    return (true, "下午时段签到成功");
            }
            else
            {
                var signedTz = ClcUtils.IsMorning(signin.SigninTime);

                if (!ClcUtils.IsMorning(DateTime.Now) && signedTz) 
                {
                    signin = new Signin() { DepotId = depotId, CarryoutDate = DateTime.Today, WorkerId = workerId, SigninTime = DateTime.Now, SigninStyle = style }; 
                    _signinRepository.Insert(signin);
                    return (true, "下午时段签到成功");
                }
                else
                {
                    return (false, string.Format("你已在{0}签到过", signin.SigninTime.ToString("HH:mm:ss")));
                }
            }
        }

        public string DoSignin(int tenantId, int depotId, int workerId)
        {
            string ret = null;
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                ret = DoSignin(depotId, workerId, "微信").Item2;
                CurrentUnitOfWork.SaveChanges();
            }
            return ret;
        }

        public string GetSigninInfo(int depotId, int workerId, DateTime dt)
        {
            var s = _signinCache.Get(depotId, workerId, ClcUtils.IsMorning(dt));
            if (s == null) return "未签到";
            return string.Format("{0} 签到", s.SigninTime.ToString("HH:mm:ss"));
        }

        public void DoCheckin(AffairCacheItem affair, AffairWorkerCacheItem aworker, int workerId)
        {
            // Get AffairWorker Entity
            var entity = _affairWorkerRepository.Get(aworker.Id);
            entity.CheckinTime = DateTime.Now;
            entity.CheckoutTime = null;
            _affairWorkerRepository.Update(entity);
            aworker.CheckinTime = DateTime.Now;
            aworker.CheckoutTime = null;
        }

        public void DoCheckout(AffairCacheItem affair, AffairWorkerCacheItem aworker, int workerId)
        {
            // Get AffairWorker Entity
            var entity = _affairWorkerRepository.Get(aworker.Id);
            entity.CheckoutTime = DateTime.Now;
            _affairWorkerRepository.Update(entity);
            aworker.CheckoutTime = DateTime.Now;
        }

        public (bool, string) MatchFinger(string finger, int workerId)
        {
            var src = StringToByte(finger, 0);
            var w = _workerCache[workerId];
            var ret = MatchFinger(src, w.Finger, w.Finger2);

            var threshold = int.Parse(SettingManager.GetSettingValue(AppSettingNames.Const.FingerThreshold));
            if (ret.Item1 > threshold) 
                return (true, $"{ret.Item2}得分({ret.Item1})");
            else
                return (false, $"指纹不匹配(得分{ret.Item1})");
        }
        
        public string GetReportToManagers(int depotId)
        {
            //return "90005";
            return _depotCache[depotId].ReportTo;
        }

        #endregion

        #region Affair，Article, Box,

        public AffairCacheItem FindCheckinAffairByWorkerId(int workerId)
        {
            int depotId = GetWorkerDepotId(workerId);

            AffairCacheItem found = null;
            foreach (var affair in _affairCache.Get(DateTime.Now.Date, depotId))
            {
                if (!ClcUtils.NowInTimeZone(affair.StartTime, affair.EndTime)) continue;
            
                var allcheckin = true;
                found = null;
                foreach (var worker in affair.Workers) {
                    if (worker.CheckoutTime.HasValue) continue;
                    if (worker.WorkerId == workerId) found = affair;

                    if (!worker.CheckinTime.HasValue) allcheckin = false;
                }

                if (found != null && allcheckin) return found;
            }
            return null;
        }

        public AffairCacheItem FindDutyAffairByWorkerId(int workerId)
        {
            int depotId = GetWorkerDepotId(workerId);

            foreach (var affair in _affairCache.Get(DateTime.Now.Date, depotId))
            {
                if (AllCheckout(affair.Workers)) continue;

                var wp = _workplaceCache[affair.WorkplaceId];
                if (!ClcUtils.NowInTimeZone(affair.StartTime, wp.DutyLead, affair.EndTime)) continue;

                // me in worker
                foreach (var worker in affair.Workers) {
                    if (worker.CheckoutTime.HasValue) continue;
                    if (worker.WorkerId == workerId) return affair;
                }
            }
            return null;
        }

        private bool AllCheckout(List<AffairWorkerCacheItem> workers)
        {
            bool allCheckout = true;
            foreach (var worker in workers) {
                if (!worker.CheckoutTime.HasValue) {
                    allCheckout = false;
                    break;
                }
            }
            return allCheckout;
        }

        public AffairCacheItem FindAltDutyAffairByDepotId(int depotId)
        {
            var wp = _workplaceCache.GetList().FirstOrDefault(x => x.DepotId == depotId && x.Name.Contains("金库"));
            if (wp == null) return null;
            var list = _affairCache.Get(DateTime.Now.Date, depotId);
            foreach (var affair in list)
            {
                if (AllCheckout(affair.Workers)) continue;

                if (!ClcUtils.NowInTimeZone(affair.StartTime, wp.DutyLead, affair.EndTime)) continue;
                // 金库
                if (affair.WorkplaceId == wp.Id) return affair;
            }
            return null;
        }

        public (AffairCacheItem, AffairWorkerCacheItem) GetCacheAffairWorker(DateTime carryoutDate, int depotId, int affairId, int workerId)
        {
            return _affairCache.GetAffairWorker(carryoutDate, depotId, affairId, workerId);
        }


        public (AffairCacheItem, AffairWorkerCacheItem) GetCacheAffairWorker(DateTime carryoutDate, int depotId, int workerId)
        {
            return _affairCache.GetAffairWorker(carryoutDate, depotId, workerId);
        }

        public bool HadStore(int affairId, Worker worker) 
        {
            var query = _affairEventRepository.GetAll()
                .Where(x => x.AffairId == affairId && x.Name == "临时存物" && x.Issurer.Contains($"{worker.Cn} {worker.Name}"));
            return query.ToList().Count > 0;
        }

        public bool HadTake(int affairId, Worker worker) 
        {
            var query = _affairEventRepository.GetAll()
                .Where(x => x.AffairId == affairId && x.Name == "临时取物" && x.Issurer.Contains($"{worker.Cn} {worker.Name}"));
            return query.ToList().Count > 0;
        }

        public (List<AffairEvent>, List<AffairEvent>) GetTempArticles(int affairId) 
        {
            var query = _affairEventRepository.GetAll().Where(x => x.AffairId == affairId && x.Name == "临时存物");

            var stores = query.ToList();
            query = _affairEventRepository.GetAll().Where(x => x.AffairId == affairId && x.Name == "临时取物");
            return (stores, query.ToList());           
        }
        
        public List<AffairEvent> GetStores(int affairId, Worker worker) 
        {
            var query = _affairEventRepository.GetAll()
                .Where(x => x.AffairId == affairId && x.Name == "临时存物" && x.Issurer.Contains($"{worker.Cn} {worker.Name}"));
            
            var store = query.FirstOrDefault();
            var routeInfo = store.Issurer.Split(',')[0];

            query = _affairEventRepository.GetAll()
                .Where(x => x.AffairId == affairId && x.Name == "临时存物" && x.Issurer.Contains(routeInfo));
            var stores = query.ToList();
            if (stores.Count < 2) return null;

            if (stores[0].Issurer.Contains($"{worker.Cn} {worker.Name}"))
                return stores;
            
            return new List<AffairEvent>() { stores[1], stores[0] };
        } 

        public Article GetArticle(int id)
        {
            return _articleCache[id];
        }

        public Article GetArticle(string cn)
        {
            return _articleCache.GetList().FirstOrDefault(x => x.Cn == cn);
        }
        
        public Box GetBoxByCn(string cn)
        {
            var b = _boxCache.GetList().Find(x => x.Cn == cn);
            return b;
        }
        
        public Box GetBox(int id)
        {
            return _boxCache[id];
        }

        public (int, int) GetAffairReportData(DateTime carryoutDate, int depotId)
        {
            var affairs = _affairCache.Get(DateTime.Now.Date, depotId);
            int count = 0;
            foreach (var a in affairs) {
                count += a.Workers.Count;
            }
            return (affairs.Count, count);
        }
        
        #endregion

        #region Weixin Receive Command and AskOpenDoor

        public (int, List<Workplace>) GetDoorsForAsk(DateTime carryoutDate, int depotId, int workerId) 
        {
            var lst = new List<Workplace>();
            var aw = _affairCache.GetAffairWorker(carryoutDate, depotId, workerId);
            var affair = aw.Item1;
            if (affair == null || string.IsNullOrEmpty(_workplaceCache[affair.WorkplaceId].AskOpenStyle)) return (0, lst);
            
            if (ClcUtils.NowInTimeZone(affair.StartTime, affair.EndTime))
            {
                lst.Add(_workplaceCache[affair.WorkplaceId]);   
            }

            if (AffairTaskCanAsk(affair)) {
                lst.Add(_workplaceCache[affair.Tasks[0].WorkplaceId]);
            }
            return (affair.Id, lst);
        }

        public (bool, string) ClickEventMessageHandler(string workerCn, string cmd)
        {
            (bool, string) ret = (false, null);
            switch (cmd) {
                case "解屏":
                    ret = (true, "unlockScreen " + workerCn);
                    break;
                case "锁屏":
                    ret = (true, "lockScreen " + workerCn);
                    break;
                case "触发开门":
                    ret = WeixinTriggerAsk(workerCn);
                    break;
                case "申请开门":
                    ret = WeixinAskOpenDoor(workerCn);
                    break;
                default:
                    ret = (false, "系统无此命令");
                    break;
            }

            return ret;
        }

        public (bool, string) WeixinAskOpenDoor(string workerCn) 
        {
            var worker = GetWorkerByCn(workerCn);
            if (worker == null) return (false, "未登记在工作人员表中");
            // return (true, "askOpenDoor " + string.Format("你有来自{0}({1})的任务开门申请", 1, 2));//, wp.Name, GetDepot(worker.DepotId).Name));

            // find affair
            var affair = FindDutyAffairByWorkerId(worker.Id);
            if (affair == null) return (false, "你无任务需要申请开门");

            var wp = GetWorkplace(affair.WorkplaceId);
            if (wp.Name.Contains("金库")) return (false, "金库不允许用手机方式申请开门");
            if (wp.AskOpenStyle != "直接") return (false, "需要设置为直接方式开门");
            
            int minNum = int.Parse(SettingManager.GetSettingValue(AppSettingNames.Rule.MinWorkersOnDuty));
            int askInterval = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.AskOpenInterval));
            // scan affair workers
            // int askCount = 0; 
            // DateTime min = DateTime.Now;
            List<int> workers = new List<int>();
            foreach (AffairWorkerCacheItem aw in affair.Workers)
            {
                if (aw.WorkerId == worker.Id) {
                    //if (aw.LastAskDoor.HasValue && DateTime.Now.Subtract(aw.LastAskDoor.Value) < TimeSpan.FromSeconds(askInterval))
                    //    return (false, $"两次申请需要间隔{askInterval}(秒）以上");
                    using (CurrentUnitOfWork.SetTenantId(1)) { SetLastAskDoorTime(affair, aw); };

                    workers.Add(aw.WorkerId);
                    if (workers.Count == minNum) {
                        // Right
                        //using (CurrentUnitOfWork.SetTenantId(1)) {
                            SetAskDoorRecord(wp.Id, affair.Id, workers.ToArray(), false);
                        //}
                        return (true, "askOpenDoor " + string.Format("你有来自{0}({1})的任务开门申请", wp.Name, GetDepot(worker.DepotId).Name));
                    }
                }
                else {
                    if (aw.LastAskDoor.HasValue && DateTime.Now.Subtract(aw.LastAskDoor.Value) < TimeSpan.FromSeconds(askInterval)) {
                        workers.Add(aw.WorkerId);
                        if (workers.Count == minNum) {
                            // Right
                            //using (CurrentUnitOfWork.SetTenantId(1)) {
                                SetAskDoorRecord(wp.Id, affair.Id, workers.ToArray(), false);
                            //}
                            return (true, "askOpenDoor " + string.Format("你有来自{0}({1})的任务开门申请", wp.Name, GetDepot(worker.DepotId).Name));
                        }
                    }
               }
            }

            return (false, "等待其他人申请开门");
        }

        public (bool, string) AskOpenDoor(int depotId, int affairId, int doorId, int[] workers, int taskId, bool waiting = false)
        {
            // judge workplace is vault
            var wp = _workplaceCache[doorId];
            if (string.IsNullOrEmpty(wp.AskOpenStyle)) return (false, "不需要申请开门");
            if (wp.AskOpenStyle == "线路") return (false, "已设置为线路任务方式开门");
            var isVault = wp.Name.Contains("金库");

            var affair = _affairCache.GetAffair(DateTime.Now.Date, depotId, affairId);

            // scan affair workers
            foreach (AffairWorkerCacheItem aw in affair.Workers)
            {
                if (aw.CheckoutTime.HasValue) continue;
                // judge this worker VaultDuty.
                var duties = _workRoleCache[aw.WorkRoleId].Duties;
                var VD = !string.IsNullOrEmpty(duties) && duties.Contains("金库");

                if (isVault && VD && !workers.Contains(aw.WorkerId))
                    return (false, "金库门需要全部相关人员确认");
                if (isVault && !VD && workers.Contains(aw.WorkerId))
                    return (false, "申请中有非金库职责人员");

                // if checkin
                if (workers.Contains(aw.WorkerId) && NeedCheckin(aw.CheckinTime))
                    return (false, "需要先验入");

                if (workers.Contains(aw.WorkerId))
                    SetLastAskDoorTime(affair, aw);
                
            }    

            if (isVault)
            {
                string remark = null;
                if (taskId != 0)
                {
                    AffairEvent e = new AffairEvent();
                    e.AffairId = affairId;
                    e.Name = "金库子任务开门";
                    e.EventTime = DateTime.Now;
                    e.Issurer = GetWorkerString(workers);
                    var task = _affairTaskRepository.Get(taskId);
                    e.Description = task.Content;
                    _affairEventRepository.Insert(e);
                    remark = $"金库子任务({task.Content})";
                }
                SetAskDoorRecord(doorId, affairId, workers, waiting, remark);
                if (!waiting) return (true, "你的金库开门申请已发往监控中心");
            }
            else {
                SetAskDoorRecord(doorId, affairId, workers, waiting);
                if (!waiting) return (true, "你的申请已发往监控中心");
            }

            return (true, "请去门口用手机触发申请");
        }

        public void RouteAskOpenDoor(string style, int routeId, int affairId, int workplaceId, string askWorkers)
        {
            SetRouteAskDoorRecord(style, routeId, workplaceId, affairId, askWorkers);
        }

        public WorkerCacheItem GetCaptain(int depotId)
        {
            var captain = _workerCache.GetList().FirstOrDefault(x => x.DepotId == depotId && x.PostName == "大队长");
            if (captain == null) throw new Exception();
            return captain;

        }
        public void InsertTempAskDoorRecord(string style, int depotId, string routeName, int affairId, int doorId, string askWorkers, string cn)
        {
            var askDoor = new AskDoorRecord();
            askDoor.AskTime = DateTime.Now;
            askDoor.WorkplaceId = doorId;
            askDoor.AskAffairId = affairId;
            askDoor.AskWorkers = askWorkers;
            askDoor.AskReason = cn;
            askDoor.Remark = style + $"({routeName})";
            _askdoorRepository.Insert(askDoor);

        }

        // used in weixin
        public (bool, string) WeixinTriggerAsk(string workerCn)
        {
            var worker = GetWorkerByCn(workerCn);
            if (worker == null) return (false, "未登记在工作人员表中");

            var query = _askdoorRepository.GetAllIncluding(x => x.Workplace)
                .Where(x => x.AskTime.Date == DateTime.Now.Date && x.Workplace.DepotId == worker.DepotId && x.Approver != x.AskReason && x.Remark.Contains("手机触发"));
            var record = query.FirstOrDefault(x => DateTime.Now.Subtract(x.AskTime) < TimeSpan.FromMinutes(5));
            if (record == null) return (false, "请先在电脑上申请开门");


            if (!record.AskReason.Contains(workerCn)) return (false, "你尚未申请开门");
            List<string> cns = new List<string>();
            if (!string.IsNullOrEmpty(record.Approver))
            {
                cns.AddRange(record.Approver.Split()); 
               if (!cns.Contains(workerCn)) cns.Add(workerCn);
            }
            else
                cns.Add(workerCn);
            var newApprover = GetSortedStringList(cns);
            record.Approver = newApprover;

            if (newApprover == record.AskReason) {
                var name = GetDepot(worker.DepotId).Name;
                return (true, "askOpenDoor " + string.Format("你有来自{0}({1})的任务开门申请", record.Workplace.Name, name));
            }
            else
                return (false, "请等待其他申请人员触发开门");
        }
        #endregion

        #region private
       // Finger Util1
        private byte[] StringToByte(string str, int index)
        {  
            byte[] outBytes = new byte[256];
            if (str.Length < (index + 1) * 512) return outBytes;
              
            for (int i = 0; i < outBytes.Length; i++)  
            {  
                outBytes[i] = Convert.ToByte(str.Substring(index*512 + i*2, 2), 16);  
            }  
            return outBytes;  
        } 
        
        // Finger Util2
        private int MatchDbFinger(byte[] src, string dbFinger, ref int score)
        {
            int score1 = 0, score2 = 0;
            var dst = StringToByte(dbFinger, 0);
            int ret = 0;
            try {
                ret = FingerDll.UserMatch(src, dst, 3, ref score1);
                dst = StringToByte(dbFinger, 1);
                ret = FingerDll.UserMatch(src, dst, 3, ref score2);
                score = Math.Max(score1, score2);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            return ret;
        }

        // Finger Util3
        private (int, string) MatchFinger(byte[] src, string finger, string finger2)
        {
            if (string.IsNullOrEmpty(finger) && string.IsNullOrEmpty(finger2))
                return (0, "无指纹存储数据");

            int score = 0, score2 = 0;      // 得分
            int ret;
            try {
                if (!string.IsNullOrEmpty(finger)) ret = MatchDbFinger(src, finger, ref score);
                if (!string.IsNullOrEmpty(finger2)) ret = MatchDbFinger(src, finger2, ref score2);
            }
            catch (Exception ex) {
                Logger.Debug(ex.Message);
            }

            int matchScore = Math.Max(score, score2);

            return (matchScore, score >= score2 ? "匹配指纹1" : "匹配指纹2");
        }


        // AaskDoor
        private string GetWorkerString(int[] ids)
        {
            string ret = null;
            int count = 0;
            foreach (int id in ids) {
                count++;
                var w = _workerCache[id];
                ret += count == ids.Length ? 
                    string.Format("{0} {1}", w.Cn, w.Name) : string.Format("{0} {1}, ", w.Cn, w.Name);
            }
            return ret;
        }


        private string SortWorkers(int[] workers)
        {
            List<string> cns = new List<string>();
            foreach(var id in workers)
                cns.Add(GetWorker(id).Cn);
            
            return GetSortedStringList(cns);

        }
        public string GetSortedStringList(List<string> cns)
        {
            var sa = cns.OrderBy(x => x);
            string ret = null;
            foreach (var s in sa) 
                if (s.Length > 0) ret += $"{s} ";
            return ret;
        }
 
        private bool AffairTaskCanAsk(AffairCacheItem affair)
        {
            foreach (var task in affair.Tasks)
            {
                if (ClcUtils.NowInTimeZone(task.StartTime, task.EndTime)) return true;
            }   
            return false;
        }

        private void SetLastAskDoorTime(AffairCacheItem affair, AffairWorkerCacheItem worker)
        {
            // Cache value
            worker.LastAskDoor = DateTime.Now;

            // Repository
            var affairWorker = _affairWorkerRepository.Get(worker.Id);
            affairWorker.LastAskDoor = DateTime.Now;
        }

        private void SetRouteAskDoorRecord(string style, int routeId, int doorId, int affairId, string askWorkers)
        {
            var askDoor = new AskDoorRecord();
            askDoor.AskTime = DateTime.Now;
            askDoor.WorkplaceId = doorId;
            askDoor.AskAffairId = affairId;
            askDoor.RouteId = routeId;
            askDoor.AskWorkers = askWorkers;
            askDoor.Remark = style;
            _askdoorRepository.Insert(askDoor);
            CurrentUnitOfWork.SaveChanges();
        }

        private void SetAskDoorRecord(int doorId, int affairId, int[] workers, bool waiting, string remark = null)
        {
            var askWorkers = GetWorkerString(workers);
            var askDoor = new AskDoorRecord();
            askDoor.AskTime = DateTime.Now;
            askDoor.WorkplaceId = doorId;
            askDoor.AskAffairId = affairId;
            askDoor.AskWorkers = askWorkers;
            askDoor.Remark = remark;

            askDoor.AskReason = SortWorkers(workers);
            if (!waiting) {
                askDoor.Approver = SortWorkers(workers);
            }
            else {
                askDoor.Remark += "(手机触发)";
            }
            askDoor.TenantId = 1;
            _askdoorRepository.Insert(askDoor);
            CurrentUnitOfWork.SaveChanges();
        }

        private bool NeedCheckin(DateTime? checkinTime) 
        {
            if (!checkinTime.HasValue) return true;
            
            int recheck = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.RecheckInterval));
            if (recheck == 0) return false;
            var diff = DateTime.Now - checkinTime.Value;
            return diff.TotalMinutes > recheck;
        }

        #endregion
    }
}