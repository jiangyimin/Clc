using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Clc.Fields;

namespace Clc.Runtime.Cache
{
    public class ArticleCache : EntityListCache<Article, Article, Article>, IArticleCache, ITransientDependency
    {
        public ArticleCache(ICacheManager cacheManager, IRepository<Article> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class DepotCache : EntityListCache<Depot, Depot, Depot>, IDepotCache, ITransientDependency
    {
        public DepotCache(ICacheManager cacheManager, IRepository<Depot> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class VehicleCache : EntityListCache<Vehicle, Vehicle, VehicleListItem>, IVehicleCache, ITransientDependency
    {
        public VehicleCache(ICacheManager cacheManager, IRepository<Vehicle> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class WorkerCache : EntityListCache<Worker, Worker, WorkerCacheItem>, IWorkerCache, ITransientDependency
    {
        public WorkerCache(ICacheManager cacheManager, IRepository<Worker> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class WorkplaceCache : EntityListCache<Workplace, Workplace, Workplace>, IWorkplaceCache, ITransientDependency
    {
        public WorkplaceCache(ICacheManager cacheManager, IRepository<Workplace> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }
}