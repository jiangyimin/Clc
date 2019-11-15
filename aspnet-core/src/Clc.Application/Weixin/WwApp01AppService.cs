using System;
using System.Linq;
using System.Text.RegularExpressions;
using Abp.Domain.Repositories;
using Clc.Routes;
using Clc.Works;
using Clc.Weixin.Dto;
using Clc.Fields;
using Clc.Runtime.Cache;
using Clc.Runtime;

namespace Clc.Weixin
{
    public class WwApp01AppService : ClcAppServiceBase, IWwApp01AppService
    {
        public WorkManager WorkManager { get; set; }
        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<EmergDoorRecord> _emergDoorRepository;

        public WwApp01AppService(IRepository<Issue> issueRepository, IRepository<EmergDoorRecord> emergDoorRepository)
        {
            _issueRepository = issueRepository;
            _emergDoorRepository = emergDoorRepository;
        }

        public void InsertDoorEmerg(int depotId, int workerId, int doorId, string content)
        {
            using(CurrentUnitOfWork.SetTenantId(1))
            {
            }
        }
    }
}