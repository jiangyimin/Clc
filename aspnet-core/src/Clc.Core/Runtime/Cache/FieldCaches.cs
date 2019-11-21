using System.Collections.Generic;
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
        private IPostCache _postCache;
        public WorkerCache(ICacheManager cacheManager, IRepository<Worker> repository, IObjectMapper objectMapper,
            IPostCache postCache)
            : base(cacheManager, repository, objectMapper)
        {
            _postCache = postCache;
        }
        public override List<WorkerCacheItem> GetList()
        {
            var list = base.GetList();
            if (list != null && list.Count != 0 & string.IsNullOrEmpty(list[0].PostName))
            {
                foreach (var w in list) 
                    w.PostName = _postCache[w.PostId].Name;
            }
            return list;
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