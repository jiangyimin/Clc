using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Clc.Clients.Entities;

namespace Clc.Clients.Cache
{
    public class OutletCache : IOutletCache, IEventHandler<EntityChangedEventData<Outlet>>
    {
        private readonly string CacheName = "CachedOutlet";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Outlet> _outletRepository;

        public OutletCache(
            ICacheManager cacheManager,
            IRepository<Outlet> outletRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _outletRepository = outletRepository;
            _abpSession = abpSession;
        }

        public List<Outlet> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _outletRepository.GetAll().ToList());
        }

        public Outlet GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public Outlet GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<Outlet> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Vehices@" + (_abpSession.TenantId ?? 0); }
        }
    }
}