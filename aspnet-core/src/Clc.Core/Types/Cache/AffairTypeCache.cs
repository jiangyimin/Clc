using System;
using System.Collections.Generic;
using System.Linq;
using Abp;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public class AffairTypeCache : IAffairTypeCache, IEventHandler<EntityChangedEventData<AffairType>>
    {
        private readonly string CacheName = "CachedAffairType";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<AffairType> _affairTypeRepository;

        public AffairTypeCache(
            ICacheManager cacheManager,
            IRepository<AffairType> affairTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _affairTypeRepository = affairTypeRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<AffairType> GetList()
        {
            var cacheKey = "AffairTypes@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache(CacheName)
                .Get(cacheKey, () => _affairTypeRepository.GetAll().ToList());
        }

        public AffairType GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public AffairType GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<AffairType> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "AffairTypes@" + (_abpSession.TenantId ?? 0); }
        }
    }
}