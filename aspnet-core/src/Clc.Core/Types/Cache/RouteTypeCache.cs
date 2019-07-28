using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public class RouteTypeCache : IRouteTypeCache, IEventHandler<EntityChangedEventData<RouteType>>
    {
        private readonly string CacheName = "CachedRouteType";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<RouteType> _routeTypeRepository;

        public RouteTypeCache(
            ICacheManager cacheManager,
            IRepository<RouteType> routeTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _routeTypeRepository = routeTypeRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<RouteType> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _routeTypeRepository.GetAll().ToList());
        }

        public RouteType GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
                
        public void HandleEvent(EntityChangedEventData<RouteType> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "RouteTypes@" + (_abpSession.TenantId ?? 0); }
        }
    }
}