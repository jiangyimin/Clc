using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Issues.Dto;
using Clc.Authorization;
using Clc.Routes;
using Clc.Runtime;
using Clc.Works;
using Clc.Clients;
using Clc.Works.Dto;

namespace Clc.Issues
{
    [AbpAuthorize(PermissionNames.Pages_Arrange, PermissionNames.Pages_Monitor)]
    public class IssueAppService : ClcAppServiceBase, IIssueAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<Issue> _issueRepository;
        private readonly IRepository<EmergDoorRecord> _doorRepository;

        public IssueAppService(IRepository<Issue> issueRepository, 
            IRepository<EmergDoorRecord> doorRepository)    
        {
            _issueRepository = issueRepository;
            _doorRepository = doorRepository;
        }

        public async Task<PagedResultDto<IssueDto>> GetIssuesAsync(PagedResultRequestDto input)
        {         
            int workerId = await GetCurrentUserWorkerIdAsync();
            var query = _issueRepository.GetAllIncluding(x => x.CreateWorker, x => x.ProcessWorker)
                .Where(x => x.DepotId == WorkManager.GetWorkerDepotId(workerId));
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging
            query = query.OrderByDescending(x => x.CreateTime);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<IssueDto>(
                totalCount,
                entities.Select(MapToIssueDto).ToList()
            );
        }

        public async Task<List<IssueDto>> GetOndutyIssuesAsync(DateTime dt)
        {
            var query = _issueRepository.GetAllIncluding(x => x.Depot)
                .Where(x => x.CreateTime.Date == dt && x.ProcessStyle == "值班信息");
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<IssueDto>>(entities);
        }

        public async Task InsertAsync(IssueInputDto input)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            workerId = WorkManager.GetCaptainOrAgentId(workerId);     // Agent

            var entity = new Issue();
            entity.DepotId = WorkManager.GetWorkerDepotId(workerId);
            entity.CreateWorkerId = workerId;
            entity.CreateTime = DateTime.Now;
            entity.ProcessStyle = input.ProcessStyle;
            entity.Content = input.Content;
            entity.ProcessTime = DateTime.Now;
            // entity.ProcessWorkerId = workerId;
            await _issueRepository.InsertAsync(entity);
        }

        public async Task ProcessIssue(int id, string processContent)
        {
            int workerId = await GetCurrentUserWorkerIdAsync();
            var entity = await _issueRepository.GetAsync(id);
            entity.ProcessTime = DateTime.Now;
            entity.ProcessWorkerId = workerId;
            entity.ProcessContent = processContent;
            await _issueRepository.UpdateAsync(entity);         
        }

        private IssueDto MapToIssueDto(Issue issue)
        {
            var dto = ObjectMapper.Map<IssueDto>(issue);
            return dto;
        }

    }
}