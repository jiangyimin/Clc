using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Authorization.Accounts.Dto;

namespace Clc.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
