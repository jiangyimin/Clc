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
    public class WorkerTypeCache : IWorkerTypeCache, IEventHandler<EntityChangedEventData<WorkerType>>
    {
        private readonly string CacheName = "CachedWorkerType";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WorkerType> _workerTypeRepository;

        public WorkerTypeCache(
            ICacheManager cacheManager,
            IRepository<WorkerType> workerTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _workerTypeRepository = workerTypeRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<WorkerType> GetList()
        {
            var cacheKey = "WorkerTypes@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache(CacheName)
                .Get(cacheKey, () => _workerTypeRepository.GetAll().ToList());
        }

        public WorkerType GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public WorkerType GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<WorkerType> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "WorkerTypes@" + (_abpSession.TenantId ?? 0); }
        }
    }
}