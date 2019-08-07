using Clc.Types.Entities;

namespace Clc.Runtime.Cache
{
    public interface IAffairTypeCache : IEntityListCache<AffairType, AffairType, AffairType>
    {
    }

    public interface IArticleTypeCache : IEntityListCache<ArticleType, ArticleType, ArticleType>
    {
    }

    public interface IPostCache : IEntityListCache<Post, Post, Post>
    {
    }

    public interface IRouteTypeCache : IEntityListCache<RouteType, RouteType, RouteType>
    {
    }

    public interface ITaskTypeCache : IEntityListCache<TaskType, TaskType, TaskType>
    {
    }

    public interface IWorkRoleCache : IEntityListCache<WorkRole, WorkRole, WorkRole>
    {
    }   
}