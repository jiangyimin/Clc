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
        private readonly IRepository<AskDoorRecord> _askdoorRepository;

        public WorkManager(IWorkerCache workerCache,
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
            IRepository<AskDoorRecord> askdoorRepository)
        {
            _workerCache = workerCache;
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
            foreach (var w in _workerCache.GetList())
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
        
        public bool IsWorkerRoleUser(string workerCn)
        {
            var w = _workerCache.GetList().Find(x => x.Cn == workerCn);
            if (w != null) 
            {
                var worker = _workerCache[w.Id];
                return string.IsNullOrEmpty(worker.LoginRoleNames) ? false : true;
            }
            return false;
        }

        #endregion

        #region Depot, Workplace

        public Depot GetDepot(int depotId)
        {
            return _depotCache[depotId];
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

        #endregion

        #region Signin/Checkin/Door
        public (bool, string) DoSignin(int depotId, int workerId, string style)
        {
            // Get Signin 
            var signin = _signinRepository.FirstOrDefault(s => s.CarryoutDate == DateTime.Today && s.DepotId == depotId && s.WorkerId == workerId);
            if (signin == null)
            {
                signin = new Signin() { DepotId = depotId, CarryoutDate = DateTime.Today, WorkerId = workerId, SigninTime = DateTime.Now, SigninStyle = style }; 
                _signinRepository.Insert(signin);
                return (true, "签到成功");
            }
            else
            {
                var min = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.MinSigninInterval));
                var diff = (DateTime.Now - signin.SigninTime).TotalMinutes;
                if (diff > min) 
                {
                    signin.SigninTime = DateTime.Now;
                    _signinRepository.Update(signin);
                    _signinCache.Get(depotId, workerId).SigninTime = DateTime.Now;
                    return (true, "再次签到成功");
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

        public string GetSigninInfo(int depotId, int workerId)
        {
            var s = _signinCache.Get(depotId, workerId);
            if (s == null) return "未签到";
            return string.Format("{0} 签到", s.SigninTime.ToString("HH:mm:ss"));
        }

        public void DoCheckin(AffairCacheItem affair, AffairWorkerCacheItem aworker, int workerId)
        {
            // Get AffairWorker Entity
            var entity = _affairWorkerRepository.Get(aworker.Id);
            entity.CheckinTime = DateTime.Now;
            _affairWorkerRepository.Update(entity);
            aworker.CheckinTime = DateTime.Now;
        }

        public (bool, string) MatchFinger(string finger, int workerId)
        {
            var src = StringToByte(finger, 0);
            var w = _workerCache[workerId];
            var ret = MatchFinger(src, w.Finger, w.Finger2);

            if (ret.Item1 > 50) 
                return (true, $"{ret.Item2}得分({ret.Item1})");
            else
                return (false, "指纹不匹配");
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
                if (!ClcUtils.NowInTimeZone(affair.StartTime, affair.EndTime)) continue;
                // me in worker
                foreach (var worker in affair.Workers)
                    if (worker.WorkerId == workerId) return affair;
            }
            return null;
        }

        public AffairCacheItem FindAltDutyAffairByDepotId(int depotId)
        {
            var wp = _workplaceCache.GetList().FirstOrDefault(x => x.DepotId == depotId && x.Name.Contains("金库"));
            if (wp == null) return null;
            var list = _affairCache.Get(DateTime.Now.Date, depotId);
            foreach (var affair in list)
            {
                if (!ClcUtils.NowInTimeZone(affair.StartTime, affair.EndTime)) continue;
                // 金库
                if (affair.WorkplaceId == wp.Id) return affair;
            }
            return null;
        }

        public (AffairCacheItem, AffairWorkerCacheItem) GetCacheAffairWorker(DateTime carryoutDate, int depotId, int workerId)
        {
            return _affairCache.GetAffairWorker(carryoutDate, depotId, workerId);
        }

        public Article GetArticle(int id)
        {
            return _articleCache[id];
        }
        public Box GetBox(int id)
        {
            return _boxCache[id];
        }
        #endregion

        #region Weixin Receive Command 

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
                    if (!IsWorkerRoleUser(workerCn)) 
                        ret = (false, "你不需要"+cmd);
                    else
                        ret = (true, "unlockScreen " + workerCn);
                    break;
                case "锁屏":
                    if (!IsWorkerRoleUser(workerCn)) 
                        ret = (false, "你不需要"+cmd);
                    else
                        ret = (true, "lockScreen " + workerCn);
                    break;
                default:
                    ret = (false, "系统无此命令");
                    break;
            }

            return ret;
        }

        // used in weixin
        public  (bool, string) AskOpenDoor(int workerId, int affairId, int workplaceId, int tenantId)
        {
            var worker = GetWorker(workerId);
            var aw = _affairCache.GetAffairWorker(DateTime.Now.Date, worker.DepotId, worker.Id);

            // valid again.
            if (aw.Item1 == null && aw.Item2 == null) return (false, "请管理员激活你的任务");
            if (aw.Item1 != null && aw.Item2 == null) return (false, "你还没安排在激活的任务中");
            if (aw.Item1.Id != affairId) return (false, "不允许同时安排在多个任务中");

            // judge workplace is vault
            var affairWp = _workplaceCache[aw.Item1.WorkplaceId];
            var isVault = affairWp.Name.Contains("金库") || aw.Item1.WorkplaceId != workplaceId;
            if (isVault && !_workRoleCache[aw.Item2.WorkRoleId].Duties.Contains("金库")) 
                return (false, "申请人的工作角色需要有金库职责");
            
            var wpOpen = _workplaceCache[workplaceId];
            if (string.IsNullOrEmpty(wpOpen.AskOpenStyle)) return (false, "不需要申请开门");
            if (wpOpen.AskOpenStyle == "验证" && NeedCheckin(aw.Item2.CheckinTime)) return (false, "需要先验入");
            if (wpOpen.AskOpenStyle == "任务") return (false, "押运任务开门方式");

            // check ask interval
            int interval = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.AskOpenInterval));
            if (aw.Item2.LastAskDoor.HasValue && (DateTime.Now - aw.Item2.LastAskDoor.Value).TotalSeconds < interval)
                return (false, $"两次申请间隔需要大于{interval}秒");

            SetLastAskDoorTime(aw.Item1, aw.Item2, tenantId);

            // check if allWorker is Asked
            DateTime min = DateTime.Now; 
            DateTime max = DateTime.Now; 
            int count = 0, vvCount = 0;
            string askWorkers = null;
            foreach (AffairWorkerCacheItem aworker in aw.Item1.Workers)
            {
                // if vaultDoor, skip if no duty
                var duties = _workRoleCache[aworker.WorkRoleId].Duties;
                if (isVault && (string.IsNullOrEmpty(duties) || !duties.Contains("金库")))
                    continue;             

                if (!aworker.LastAskDoor.HasValue) {
                    count++; continue;                    
                }

                vvCount++;
                worker = _workerCache[aworker.WorkerId];
                askWorkers += string.Format("{0} {1}, ", worker.Cn, worker.Name);
                if (DateTime.Compare(aworker.LastAskDoor.Value, min) < 0) min = aworker.LastAskDoor.Value;
                if (DateTime.Compare(aworker.LastAskDoor.Value, max) > 0) max = aworker.LastAskDoor.Value;
            }

            int minNum = int.Parse(SettingManager.GetSettingValue(AppSettingNames.Rule.MinWorkersOnDuty));
            int period = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.AskOpenPeriod));

            if (isVault && count == vvCount && (max - min).TotalSeconds <= period)
            {
                SetAskDoorRecord(wpOpen.Id, affairId, askWorkers, tenantId);
                return (true, "已到金库开门要求");
            }
            if (!isVault && vvCount >= minNum && (max - min).TotalSeconds <= period)
            {
                SetAskDoorRecord(wpOpen.Id, affairId, askWorkers, tenantId);
                return (true, "你的申请已通知监控中心，请等待开门");
            }

            return (false, "已申请开门");
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
            int ret = FingerDll.UserMatch(src, dst, 3, ref score1);
            dst = StringToByte(dbFinger, 1);
            ret = FingerDll.UserMatch(src, dst, 3, ref score2);
            score = Math.Max(score1, score2);
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

        private bool AffairTaskCanAsk(AffairCacheItem affair)
        {
            foreach (var task in affair.Tasks)
            {
                if (ClcUtils.NowInTimeZone(task.StartTime, task.EndTime)) return true;
            }   
            return false;
        }

        private void SetLastAskDoorTime(AffairCacheItem affair, AffairWorkerCacheItem worker, int tenantId)
        {
            worker.LastAskDoor = DateTime.Now;
            if (tenantId == 1) 
            {
                using (CurrentUnitOfWork.SetTenantId(tenantId)) {
                    var affairWorker = _affairWorkerRepository.Get(worker.Id);
                    affairWorker.LastAskDoor = DateTime.Now;
                    CurrentUnitOfWork.SaveChanges();
                }
            }
            else
            {
                    var affairWorker = _affairWorkerRepository.Get(worker.Id);
                    affairWorker.LastAskDoor = DateTime.Now;
                    CurrentUnitOfWork.SaveChanges();
            }
        }

        private void SetAskDoorRecord(int doorId, int affairId, string askWorkers, int tenantId)
        {
            if (tenantId == 1) 
            {
                using (CurrentUnitOfWork.SetTenantId(tenantId)) {
                    var askDoor = new AskDoorRecord();
                    askDoor.AskTime = DateTime.Now;
                    askDoor.WorkplaceId = doorId;
                    askDoor.AskAffairId = affairId;
                    askDoor.AskWorkers = askWorkers;
                    _askdoorRepository.Insert(askDoor);
                    CurrentUnitOfWork.SaveChanges();
                }
            }
            else
            {
                    var askDoor = new AskDoorRecord();
                    askDoor.AskTime = DateTime.Now;
                    askDoor.WorkplaceId = doorId;
                    askDoor.AskAffairId = affairId;
                    askDoor.AskWorkers = askWorkers;
                    _askdoorRepository.Insert(askDoor);
                    CurrentUnitOfWork.SaveChanges();
            }
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