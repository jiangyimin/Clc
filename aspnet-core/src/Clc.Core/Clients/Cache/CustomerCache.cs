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
    public class CustomerCache : ICustomerCache, IEventHandler<EntityChangedEventData<Customer>>
    {
        private readonly string CacheName = "CachedCustomer";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Customer> _customerRepository;

        public CustomerCache(
            ICacheManager cacheManager,
            IRepository<Customer> customerRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _customerRepository = customerRepository;
            _abpSession = abpSession;
        }

        public List<Customer> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _customerRepository.GetAll().ToList());
        }

        public Customer GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public Customer GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<Customer> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Vehices@" + (_abpSession.TenantId ?? 0); }
        }
    }
}