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
    public class TaskTypeCache : ITaskTypeCache, IEventHandler<EntityChangedEventData<TaskType>>
    {
        private readonly string CacheName = "CachedTaskType";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TaskType> _taskTypeRepository;

        public TaskTypeCache(
            ICacheManager cacheManager,
            IRepository<TaskType> taskTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _taskTypeRepository = taskTypeRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<TaskType> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _taskTypeRepository.GetAll().ToList());
        }

        public TaskType GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public TaskType GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<TaskType> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "TaskTypes@" + (_abpSession.TenantId ?? 0); }
        }
    }
}