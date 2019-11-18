using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Runtime;
using Clc.Works;

namespace Clc.Monitors
{

    [AbpAuthorize]
    public class MonitorAppService : ClcAppServiceBase, IMonitorAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<AskDoorRecord> _askDoorRepository;
        private readonly IRepository<EmergDoorRecord> _emergDoorRepository;

        public MonitorAppService()
        {
        }

    }
}