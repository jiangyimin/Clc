using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;

namespace Clc.Fields.Cache
{
    public class WorkerCache : IWorkerCache, IEventHandler<EntityChangedEventData<Worker>>
    {
        private readonly string CacheName = "CachedWorker";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Worker> _workerRepository;

        public WorkerCache(
            ICacheManager cacheManager,
            IRepository<Worker> workerRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _workerRepository = workerRepository;
            _abpSession = abpSession;
        }

        public List<Worker> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _workerRepository.GetAll().ToList());
        }

        public Worker GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public Worker GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<Worker> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Depots@" + (_abpSession.TenantId ?? 0); }
        }
    }
}