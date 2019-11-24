using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.ObjectMapping;
using Clc.Affairs;

namespace Clc.Runtime.Cache
{
    public class AffairCache : IAffairCache, ITransientDependency
    {
        public IObjectMapper ObjectMapper { get; set; }

        private readonly string CacheName = "CachedAffair";
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Affair> _affairRepository;

        public AffairCache(
            ICacheManager cacheManager,
            IRepository<Affair> affairRepository)
        {
            _cacheManager = cacheManager;
            _affairRepository = affairRepository;
            _cacheManager.GetCache(CacheName).DefaultSlidingExpireTime = TimeSpan.FromHours(2);
        }

        public void Set(DateTime carryoutDate, int depotId, object value)
        {
            string cacheKey = carryoutDate.ToString() + depotId.ToString();
            _cacheManager.GetCache(CacheName).Set(cacheKey, value);
        }

        public List<AffairCacheItem> Get(DateTime carryoutDate, int depotId)
        {
            string cacheKey = carryoutDate.ToString() + depotId.ToString();
            return _cacheManager.GetCache(CacheName).Get(cacheKey, () => {
                var query = _affairRepository.GetAllIncluding(x => x.Workplace, x => x.Workers, x => x.Tasks).Where(x => x.CarryoutDate == carryoutDate && x.DepotId == depotId && x.Status != "安排");
                return ObjectMapper.Map<List<AffairCacheItem>>(query.ToList());
            });
        }

        public AffairCacheItem GetAffair(DateTime carryoutDate, int depotId, int id)
        {
            return Get(carryoutDate, depotId).FirstOrDefault(x => x.Id == id);
        }

        public (AffairCacheItem, AffairWorkerCacheItem) GetAffairWorker(DateTime carryoutDate, int depotId, int workerId)
        {
            var list = Get(carryoutDate, depotId);
            if (list.Count == 0) return (null, null);
            
            AffairCacheItem a = null;
            foreach (var affair in (List<AffairCacheItem>)list)
            {
                a = affair;
                foreach (var worker in affair.Workers)
                {
                    if (worker.WorkerId == workerId)
                        return (affair, worker);
                }
            }
            return (a, null);
        }

        public (AffairCacheItem, AffairWorkerCacheItem) GetAffairWorker(DateTime carryoutDate, int depotId, int affairId, int workerId)
        {
            var list = Get(carryoutDate, depotId);
            if (list.Count == 0) return (null, null);
            
            AffairCacheItem a = null;
            foreach (var affair in (List<AffairCacheItem>)list)
            {
                if (affair.Id != affairId) continue;

                a = affair;
                foreach (var worker in affair.Workers)
                {
                    if (worker.WorkerId == workerId)
                        return (affair, worker);
                }
            }
            return (a, null);
        }

    }
}