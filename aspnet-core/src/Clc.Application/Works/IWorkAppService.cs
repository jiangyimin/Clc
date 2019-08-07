using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Clc.Works
{
    public interface IWorkAppService : IApplicationService
    {
        bool VerifyUnlockPassword(string password);
        string GetTodayString();
    }
}
