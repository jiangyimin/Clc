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
            IRepository<AffairTask> affairTaskRepository)
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
        }

        #region Worker

        public Worker GetWorker(int workerId)
        {
            return _workerCache[workerId];
        }

        public Worker GetWorkerByRfid(string rfid) 
        {
            var w = _workerCache.GetList().Find(x => x.Rfid == rfid);
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
        
        public Workplace GetWorkplace(int id)
        {
            return _workplaceCache[id];
        }

        public List<Workplace> GetDoors(int workerId)
        {
            var depotId = _workerCache[workerId].DepotId;
            return _workplaceCache.GetList().FindAll(x => x.DepotId == depotId && !string.IsNullOrWhiteSpace(x.DoorIp));
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

        public List<int> GetShareDepods(DateTime carryoutDate, int depotId, int affairId)
        {
            var affair = _affairCache.GetAffair(carryoutDate, depotId, affairId);
            var wp = _workplaceCache[affair.WorkplaceId];
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

        #region Process Weixin Receive Command

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
                case "申请开金库":
                    ret = WeixinAskOpenDoor(workerCn, true);
                    break;
                case "申请开库房":
                    ret = WeixinAskOpenDoor(workerCn, false);
                    break;
                default:
                    ret = (false, "系统无此命令");
                    break;
            }

            return ret;
        }

        private (bool, string) WeixinAskOpenDoor(string cn, bool isVault)
        {
            var worker = GetWorkerByCn(cn);
            var aw = _affairCache.GetAffairWorker(DateTime.Now.Date, worker.DepotId, worker.Id);
            // Item1= Affiar, Item2=AffairWorker
            if (aw.Item1 == null && aw.Item2 == null) return (false, "请管理员激活你的任务");
            if (aw.Item1 != null && aw.Item2 == null) return (false, "你还没安排在激活的任务中");

            // judge workplace is vault
            var wp = _workplaceCache[aw.Item1.WorkplaceId];
            if (isVault)
            {
                if (wp.Name.Contains("金库")) return AskOpenDoor(aw.Item1, aw.Item2);
                // 操作金库子任务
                var affairTasks = _affairTaskRepository.GetAll().Where(x => x.AffairId == aw.Item1.Id).ToList();
            }
            else
            {
                if (wp.Name.Contains("金库")) return (false, "你没安排库房任务");
                return AskOpenDoor(aw.Item1, aw.Item2);
            }
            return (false, null);
        }

        private (bool, string) AskOpenDoor(AffairCacheItem affair, AffairWorkerCacheItem worker)
        {
            int doorId = 0;
            var msg = CheckAskOpenValid(affair, worker, true, out doorId);
            if (msg == null) {
                // Set Time
                int interval = int.Parse(SettingManager.GetSettingValue(AppSettingNames.TimeRule.AskOpenInterval));
                var diff = DateTime.Now - worker.LastAskDoor.Value;
                if (worker.LastAskDoor.HasValue &&  diff.TotalSeconds < interval)
                    return (false, $"两次申请间隔需要大于{interval}秒");

                worker.LastAskDoor = DateTime.Now;
                using (CurrentUnitOfWork.SetTenantId(1)) {
                    var affairWorker = _affairWorkerRepository.Get(worker.Id);
                    affairWorker.LastAskDoor = DateTime.Now;
                    CurrentUnitOfWork.SaveChanges();
                }

                var checkOpen = CheckOpenDoor(affair, true, interval);
                if (checkOpen.Item1) 
                {
                    using (CurrentUnitOfWork.SetTenantId(1)) {
                        var ask = new AskDoorRecord();
                        ask.WorkplaceId = doorId;
                        ask.AskAffairId = affair.Id;
                        ask.AskWorkers = checkOpen.Item3;
                        CurrentUnitOfWork.SaveChanges();
                    }
                    return (true, checkOpen.Item2);
                }
                
                return (false, checkOpen.Item2);
            }
            else 
            {
                return (false, msg);
            }
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

        private (bool, string, string) CheckOpenDoor(AffairCacheItem affair, bool isVault, int len)
        {
            DateTime min = DateTime.Now; 
            DateTime max = DateTime.Now; 
            int count = 0;
            string askWorkers = null;
            foreach (AffairWorkerCacheItem aw in affair.Workers)
            {
                // if vaultDoor, skip if no duty
                var duties = _workRoleCache[aw.WorkRoleId].Duties;
                if (isVault && (string.IsNullOrEmpty(duties) || !duties.Contains("金库")))
                    continue;             
 
                if (!aw.LastAskDoor.HasValue) return (false, "已受理。等待其他同事申请", null);

                count++;
                var worker = _workerCache[aw.WorkerId];
                askWorkers += string.Format("{0} {1}, ", worker.Cn, worker.Name);
                if (DateTime.Compare(aw.LastAskDoor.Value, min) < 0)
                    min = aw.LastAskDoor.Value;
            }

            int num = int.Parse(SettingManager.GetSettingValue(AppSettingNames.Rule.MinWorkersOnDuty));
            var diff = max - min;
            if (count >= num && diff.TotalSeconds <= len)
            {
                var wp = _workplaceCache[affair.WorkplaceId];
                var msg = string.Format("openDoor {0}", _depotCache[wp.DepotId].Name + wp.Name);
                return (true, msg, askWorkers);
            }
            else 
            {
                if (diff.TotalSeconds <= len)
                    return (false, "已受理。但有其他让你的申请过时", null);
                else
                    return (false, "已受理。但未达最低在岗人数({num})", null);
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

        private string CheckAskOpenValid(AffairCacheItem affair, AffairWorkerCacheItem worker, bool isVault, out int doorId)
        {
            doorId = 0;
            string msg = null;
            // Check workRole Duty
            var duties = _workRoleCache[worker.WorkRoleId].Duties;
            if ( isVault && (string.IsNullOrEmpty(duties) || !duties.Contains("金库")) )
                return "申请人的角色中需要有金库职责";

            // Check Time
            Workplace wp = _workplaceCache[affair.WorkplaceId];
            DateTime start = ClcUtils.GetDateTime(affair.StartTime).AddMinutes(-wp.AskOpenLead);
            DateTime end = wp.AskOpenDeadline == 0 ? ClcUtils.GetDateTime(affair.EndTime)
                                                : ClcUtils.GetDateTime(affair.StartTime).AddMinutes(wp.AskOpenDeadline);

            // Check Main Affair's valid, msg (first is last)
            if (!ClcUtils.NowInTimeZone(start, end)) msg = "不在申请时段";
            if (wp.AskOpenStyle == "验证" && NeedCheckin(worker.CheckinTime)) msg = "需要先验入";
            if (wp.AskOpenStyle == "任务") msg = "任务开门方式，不能从微信端申请";
            if (isVault && !wp.Name.Contains("金库")) msg = "无金库开门任务";
           
            // Check sub vault tasks
            if (isVault && msg != null)
            {
                var tasks = _affairTaskRepository.GetAll().Where(x => x.AffairId == affair.Id).ToList();
                foreach (var task in tasks)
                {
                    wp = _workplaceCache[task.WorkplaceId];
                    start = ClcUtils.GetDateTime(task.StartTime).AddMinutes(-wp.AskOpenLead);
                    end = wp.AskOpenDeadline == 0 ? ClcUtils.GetDateTime(task.EndTime)
                                                : ClcUtils.GetDateTime(task.StartTime).AddMinutes(wp.AskOpenDeadline);

                    if (ClcUtils.NowInTimeZone(start, end))
                    {
                        msg = null; break;
                    }
                }
            }
            
            doorId = wp.Id;
            return msg;     
        }
        #endregion
    }
}