using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.ObjectMapping;
using Clc.Routes;

namespace Clc.Runtime.Cache
{
    public class RouteCache : IRouteCache, ITransientDependency
    {
        public IObjectMapper ObjectMapper { get; set; }

        private readonly string CacheName = "CachedRouteWorker";
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Route> _routeRepository;

        public RouteCache(
            ICacheManager cacheManager,
            IRepository<Route> routeRepository)
        {
            _cacheManager = cacheManager;
            _routeRepository = routeRepository;
            _cacheManager.GetCache(CacheName).DefaultSlidingExpireTime = TimeSpan.FromHours(2);
        }

        public void Set(DateTime carryoutDate, int depotId, object value)
        {
            string cacheKey = carryoutDate.ToString() + depotId.ToString();
            _cacheManager.GetCache(CacheName).Set(cacheKey, value);
        }

        public List<RouteCacheItem> Get(DateTime carryoutDate, int depotId)
        {
            string cacheKey = carryoutDate.ToString() + depotId.ToString();
            return _cacheManager.GetCache(CacheName).Get(cacheKey, () => {
                var query = _routeRepository.GetAllIncluding(x => x.RouteType, x => x.Workers).Where(x => x.CarryoutDate == carryoutDate && x.DepotId == depotId && x.Status != "安排");
                return ObjectMapper.Map<List<RouteCacheItem>>(query.ToList());
            });
        }

        public (RouteCacheItem, RouteWorkerCacheItem) GetRouteWorker(DateTime carryoutDate, int depotId, int workerId)
        {
            var list = Get(carryoutDate, depotId);
            if (list.Count == 0) return (null, null);
            
            RouteCacheItem a = null;
            foreach (var route in (List<RouteCacheItem>)list)
            {
                a = route;
                foreach (var worker in route.Workers)
                {
                    if (worker.WorkerId == workerId)
                        return (route, worker);
                }
            }
            return (a, null);
        }

    }
}