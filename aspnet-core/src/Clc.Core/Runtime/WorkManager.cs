using Abp.Dependency;
using Abp.Domain.Services;
using Clc.Fields;
using Clc.Runtime.Cache;

namespace Clc.Works
{
    public class WorkManager : IDomainService
    {
        private readonly IWorkerCache _workerCache;
        private readonly IWorkplaceCache _workplaceCache;
        //private readonly IWorkerCache _workerCache;
        public WorkManager(IWorkerCache workerCache,
            IWorkplaceCache workplaceCache)
        {
            _workerCache = workerCache;
            _workplaceCache = workplaceCache;
        }

        public int GetWorkerDepotId(int workerId)
        {
            return _workerCache[workerId].DepotId;
        }

        public Workplace GetWorkplace(int id)
        {
            return _workplaceCache[id];
        }
    }
}