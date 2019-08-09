using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Clc.Clients;

namespace Clc.Runtime.Cache
{
    public class CustomerCache : EntityListCache<Customer, Customer, Customer>, ICustomerCache, ITransientDependency
    {
        public CustomerCache(ICacheManager cacheManager, IRepository<Customer> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }
    public class OutletCache : EntityListCache<Outlet, Outlet, Outlet>, IOutletCache, ITransientDependency
    {
        public OutletCache(ICacheManager cacheManager, IRepository<Outlet> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }
}
