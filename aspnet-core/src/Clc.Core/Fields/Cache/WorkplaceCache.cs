using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public class WorkplaceCache : IWorkplaceCache, IEventHandler<EntityChangedEventData<Workplace>>
    {
        private readonly string CacheName = "CachedWorkplace";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Workplace> _workplaceRepository;

        public WorkplaceCache(
            ICacheManager cacheManager,
            IRepository<Workplace> workplaceRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _workplaceRepository = workplaceRepository;
            _abpSession = abpSession;
        }

        public List<Workplace> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _workplaceRepository.GetAll().ToList());
        }

        public Workplace GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public Workplace GetByName(int depotId, string name)
        {
            return GetList().FirstOrDefault(d => d.DepotId == depotId && d.Name == name);
        }
        
        public void HandleEvent(EntityChangedEventData<Workplace> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Workplaces@" + (_abpSession.TenantId ?? 0); }
        }
    }
}