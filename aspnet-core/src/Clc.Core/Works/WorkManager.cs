using Abp.Dependency;
using Abp.Domain.Services;
using Clc.Runtime.Cache;

namespace Clc.Works
{
    public class WorkManager : IDomainService
    {
        private readonly IWorkerCache _workerCache;
        public WorkManager(IWorkerCache workerCache)
        {
            _workerCache = workerCache;
        }

        public int GetWorkerDepotId(int workerId)
        {
            return _workerCache[workerId].DepotId;
        }
    }
}