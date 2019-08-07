using Clc.Fields.Entities;

namespace Clc.Runtime.Cache
{
    public interface IArticleCache : IEntityListCache<Article, Article, Article>
    {
    }

    public interface IDepotCache : IEntityListCache<Depot, Depot, Depot>
    {
    }

    public interface IVehicleCache : IEntityListCache<Vehicle, Vehicle, VehicleListItem>
    {
    }

    public interface IWorkerCache : IEntityListCache<Worker, Worker, WorkerListItem>
    {
    }

    public interface IWorkplaceCache : IEntityListCache<Workplace, Workplace, Workplace>
    {
    }
}