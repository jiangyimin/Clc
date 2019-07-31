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
    public class VehicleCache : IVehicleCache, IEventHandler<EntityChangedEventData<Vehicle>>
    {
        private readonly string CacheName = "CachedVehicle";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleCache(
            ICacheManager cacheManager,
            IRepository<Vehicle> vehicleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _vehicleRepository = vehicleRepository;
            _abpSession = abpSession;
        }

        public List<Vehicle> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _vehicleRepository.GetAll().ToList());
        }

        public Vehicle GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public Vehicle GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<Vehicle> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Vehices@" + (_abpSession.TenantId ?? 0); }
        }
    }
}