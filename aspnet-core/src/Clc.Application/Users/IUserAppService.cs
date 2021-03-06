using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.Roles.Dto;
using Clc.Users.Dto;

namespace Clc.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles(bool isWorkerRole = false);

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task AddWorkerUsers();

        Task UpdateWorkerUser(EntityDto<long> input);
    }
}
