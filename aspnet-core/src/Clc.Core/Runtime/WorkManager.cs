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
        private readonly IDepotCache _depotCache;
        private readonly IArticleCache _articleCache;
        private readonly IPostCache _postCache;
        private readonly IBoxCache _boxCache;

        private readonly ISigninCache _signinCache;
        private readonly IRepository<Signin> _signinRepository;
        private readonly IRepository<Affair> _affairRepository;
        private readonly IRepository<AffairWorker> _affairWorkerRepository;

        public WorkManager(IWorkerCache workerCache,
            IWorkplaceCache workplaceCache,
            IDepotCache depotCache,
            IArticleCache articleCache,
            IPostCache postCache,
            IBoxCache boxCache,
            ISigninCache signinCache,
            IRepository<Signin> signinRepository,
            IRepository<Affair> affairRepository,
            IRepository<AffairWorker> affairWorkerRepository)
        {
            _workerCache = workerCache;
            _workplaceCache = workplaceCache;
            _depotCache = depotCache;
            _articleCache = articleCache;
            _postCache = postCache;
            _boxCache = boxCache;
            _signinCache = signinCache;

            _signinRepository = signinRepository;
            _affairRepository = affairRepository;
            _affairWorkerRepository = affairWorkerRepository;
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

        public bool IsCaptain(int workerId)
        {
            var worker = _workerCache[workerId];
            return worker.WorkerRoleName == ClcConsts.CaptainRoleName;
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
                return string.IsNullOrEmpty(worker.WorkerRoleName) ? false : true;
            }
            return false;
        }

        public string DoSignin(int depotId, int workerId)
        {
            // Get Signin 
            var signin = _signinRepository.FirstOrDefault(s => s.DepotId == depotId && s.CarryoutDate == DateTime.Today && s.WorkerId == workerId);
            if (signin == null)
            {
                Signin s = new Signin() { DepotId = depotId, CarryoutDate = DateTime.Today, WorkerId = workerId, SigninTime = DateTime.Now }; 
                _signinRepository.Insert(s);
                return "签到成功";
            }
            else
            {
                return "你已签过到";
            }
        }

        public string DoSignin(int tenantId, int depotId, int workerId)
        {
            string ret = null;
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                ret = DoSignin(depotId, workerId);
                CurrentUnitOfWork.SaveChanges();
            }
            return ret;
        }

        #endregion

        #region Other

        public string GetDepotAgentCn(int depotId)
        {
            return _depotCache[depotId].AgentCn;
        }
        
        public Workplace GetWorkplace(int id)
        {
            return _workplaceCache[id];
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

        public string GetSigninInfo(int depotId, int workerId)
        {
            var s = _signinCache.Get(depotId, workerId);
            if (s == null) return "未签到";
            return s.SigninTime.ToString("HH:mm") + " 签到";
        }

        public string GetReportToManagers(int depotId)
        {
            //return "90005";
            return _depotCache[depotId].ReportTo;
        }
        #endregion

        #region Affair, Article, Box, 

        public List<int> GetShareDepods(int affairId)
        {
            var affair = _affairRepository.Get(affairId);
            var wp = _workplaceCache[affair.WorkplaceId];
            var depots = new List<int>() { affair.DepotId };
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
        public AffairWorker GetValidAffairWorker(int depotId, int workerId)
        {
            var lst = _affairWorkerRepository.GetAllIncluding(x => x.Affair)
                .Where(x => x.Affair.DepotId == depotId && x.Affair.CarryoutDate == DateTime.Now.Date && x.WorkerId == workerId)
                .ToList();            
            if (lst == null) return null;

            foreach (var affair in lst)
            {
                DateTime start = ClcUtils.GetDateTime(affair.Affair.StartTime);
                DateTime end = ClcUtils.GetDateTime(affair.Affair.EndTime, affair.Affair.IsTomorrow);
                if (ClcUtils.NowInTimeZone(start, end)) return affair;
            }
            return null;
        }

        public List<int> GetWorkersFromAffairId(int affairId) 
        {
            var lst = _affairWorkerRepository.GetAllIncluding(x => x.Affair).Where(x => x.AffairId == affairId).ToList();

            List<int> workers = new List<int>();
            foreach (var w in lst)
            {
                workers.Add(w.WorkerId);
            }
            return workers;
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

    }
}