using System;
using System.Linq;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Clc.Affairs;
using Clc.Configuration;
using Clc.Fields;
using Clc.Runtime;
using Clc.Runtime.Cache;

namespace Clc.Works
{
    public class WorkManager : IDomainService
    {
        public ISettingManager SettingManager;

        private readonly IWorkerCache _workerCache;
        private readonly IWorkplaceCache _workplaceCache;
        private readonly IDepotCache _depotCache;
        // private readonly IRepository<Affair> _affairRepository;
        private readonly IRepository<AffairWorker> _affairWorkerRepository;

        public WorkManager(IWorkerCache workerCache,
            IWorkplaceCache workplaceCache,
            IDepotCache depotCache,
            IRepository<AffairWorker> affairWorkerRepository)
        {
            _workerCache = workerCache;
            _workplaceCache = workplaceCache;
            _depotCache = depotCache;
            _affairWorkerRepository = affairWorkerRepository;
        }

        #region Worker
        public int GetWorkerDepotId(int workerId)
        {
            return _workerCache[workerId].DepotId;
        }

        public string GetWorkerCn(int workerId)
        {
            return _workerCache[workerId].Cn;
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

        #endregion

        #region Depot Util

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
        #endregion

        #region Affair, Article, Box, 
        public AffairWorker GetValidAffairWorker(int depotId, int workerId)
        {
            var lst = _affairWorkerRepository.GetAllIncluding(x => x.Affair)
                .Where(x => x.Affair.DepotId == depotId && x.Affair.CarryoutDate == DateTime.Now.Date && x.WorkerId == workerId)
                .ToList();            
            if (lst == null) return null;

            foreach (var affair in lst)
            {
                DateTime start = ClcUtils.GetNowDateTime(affair.Affair.StartTime);
                DateTime end = ClcUtils.GetNowDateTime(affair.Affair.EndTime, affair.Affair.IsTomorrow);
                if (ClcUtils.NowInTimeZone(start, end)) return affair;
            }
            return null;
        }

        public string GetWorkersFromAffairId(int affairId) 
        {
            var lst = _affairWorkerRepository.GetAllIncluding(x => x.Affair).Where(x => x.AffairId == affairId).ToList();
            string workers = "";
            foreach (var w in lst)
            {
                workers += _workerCache[w.WorkerId].Name + " ";
            }
            return workers;
        }

        #endregion

        #region Route
        #endregion
    }
}