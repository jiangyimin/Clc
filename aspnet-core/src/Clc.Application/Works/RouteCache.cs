using System;
using System.Collections.Generic;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Clc.Works.Dto;

namespace Clc.Works
{
    public class RouteCache : IRouteCache, ITransientDependency
    {
        private readonly string CacheName = "CachedRoute";
        private readonly ICacheManager _cacheManager;

        public RouteCache(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public List<RouteCDto> Get(DateTime CarryoutDate, int affairId)
        {
            string cacheKey = CarryoutDate.Date.ToString() + affairId.ToString();
            return (List<RouteCDto>)_cacheManager.GetCache(CacheName)
                .Get(cacheKey, () => throw new Exception("Invaid"));
        }
        public void Set(DateTime CarryoutDate, int affairId, List<RouteCDto> routes)
        {
            string cacheKey = CarryoutDate.Date.ToString() + affairId.ToString();
            _cacheManager.GetCache(CacheName).Set(cacheKey, routes);
        }
    }
}