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
    public class WorkRoleCache : IWorkRoleCache, IEventHandler<EntityChangedEventData<WorkRole>>
    {
        private readonly string CacheName = "CachedWorkRole";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WorkRole> _workRoleRepository;

        public WorkRoleCache(
            ICacheManager cacheManager,
            IRepository<WorkRole> workRoleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _workRoleRepository = workRoleRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<WorkRole> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _workRoleRepository.GetAll().ToList());
        }

        public WorkRole GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public WorkRole GetByName(string name)
        {
            return GetList().FirstOrDefault(d => d.Name == name);
        }
        
        public void HandleEvent(EntityChangedEventData<WorkRole> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "WorkRoles@" + (_abpSession.TenantId ?? 0); }
        }
    }
}