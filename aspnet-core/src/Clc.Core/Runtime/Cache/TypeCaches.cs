using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Clc.Types;

namespace Clc.Runtime.Cache
{
    public class ArticleTypeCache : EntityListCache<ArticleType, ArticleType, ArticleType>, IArticleTypeCache, ITransientDependency
    {
        public ArticleTypeCache(ICacheManager cacheManager, IRepository<ArticleType> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class PostCache : EntityListCache<Post, Post, Post>, IPostCache, ITransientDependency
    {
        public PostCache(ICacheManager cacheManager, IRepository<Post> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class RouteTypeCache : EntityListCache<RouteType, RouteType, RouteType>, IRouteTypeCache, ITransientDependency
    {
        public RouteTypeCache(ICacheManager cacheManager, IRepository<RouteType> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class TaskTypeCache : EntityListCache<TaskType, TaskType, TaskType>, ITaskTypeCache, ITransientDependency
    {
        public TaskTypeCache(ICacheManager cacheManager, IRepository<TaskType> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class WorkRoleCache : EntityListCache<WorkRole, WorkRole, WorkRole>, IWorkRoleCache, ITransientDependency
    {
        public WorkRoleCache(ICacheManager cacheManager, IRepository<WorkRole> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class OilTypeCache : EntityListCache<OilType, OilType, OilType>, IOilTypeCache, ITransientDependency
    {
        public OilTypeCache(ICacheManager cacheManager, IRepository<OilType> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

    public class VehicleMTTypeCache : EntityListCache<VehicleMTType, VehicleMTType, VehicleMTType>, IVehicleMTTypeCache, ITransientDependency
    {
        public VehicleMTTypeCache(ICacheManager cacheManager, IRepository<VehicleMTType> repository, IObjectMapper objectMapper)
            : base(cacheManager, repository, objectMapper)
        {
        }
    }

}