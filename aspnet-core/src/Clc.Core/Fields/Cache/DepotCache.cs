using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;

namespace Clc.Fields.Cache
{
    public class DepotCache : IDepotCache, IEventHandler<EntityChangedEventData<Depot>>
    {
        private readonly string CacheName = "CachedDepot";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Depot> _depotRepository;

        public DepotCache(
            ICacheManager cacheManager,
            IRepository<Depot> depotRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _depotRepository = depotRepository;
            _abpSession = abpSession;
        }

        public List<Depot> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _depotRepository.GetAll().ToList());
        }

        public Depot GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public void HandleEvent(EntityChangedEventData<Depot> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Depots@" + (_abpSession.TenantId ?? 0); }
        }
    }
}