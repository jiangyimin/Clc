using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Sessions.Dto;

namespace Clc.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
