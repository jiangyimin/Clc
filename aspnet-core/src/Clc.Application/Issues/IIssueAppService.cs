using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.Issues.Dto;
using Clc.Works.Dto;

namespace Clc.Issues
{
    public interface IIssueAppService : IApplicationService
    {
        Task<PagedResultDto<IssueDto>> GetIssuesAsync(PagedResultRequestDto requestDto);

        Task<List<IssueDto>> GetOndutyIssuesAsync(DateTime dt);

        Task InsertAsync(IssueInputDto input);

        Task<string> ProcessIssue(int id, string processContent);
    }
}
