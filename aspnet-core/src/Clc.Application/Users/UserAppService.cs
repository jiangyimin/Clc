using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Clc.Authorization;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;
using Clc.Roles.Dto;
using Clc.Users.Dto;
using Clc.Runtime.Cache;
using Clc.Fields;

namespace Clc.Users
{
    [AbpAuthorize(PermissionNames.Pages_Setup)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IWorkerCache  _workerCache;
        private readonly IPostCache _postCache;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IWorkerCache workerCache,
            IPostCache postCache)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _workerCache = workerCache;
            _postCache = postCache;
        }

        public override async Task<UserDto> Create(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> Update(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRoles(user, input.RoleNames));
            }

            return await Get(input);
        }

        public override async Task Delete(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles(bool isWorkerRole)
        {
            var roles = await _roleRepository.GetAllListAsync();
            roles = roles.Where(x => x.IsWorkerRole == isWorkerRole).ToList();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        public async Task resetWorkerUsersToLatest()
        {
            var users = await Repository.GetAllListAsync();
            foreach (WorkerListItem worker in _workerCache.GetList())
            {
                string roleName = _postCache[worker.PostId].WorkerRoleName;
               
                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    // Skip unexisted role
                    try 
                    {
                        await _roleManager.GetRoleByNameAsync(roleName);
                    }
                    catch
                    {
                        continue;
                    }

                    // User
                    string userName = "Worker" + worker.Cn;
                    var user = users.FirstOrDefault(x => x.UserName == userName);
                    if (user == null)
                    {
                        // Add WorkerUser
                        var newUser = User.CreateUser(AbpSession.TenantId??0, userName, User.WorkerUserDefaultPassword);
                        newUser.WorkerId = worker.Id;
                        CheckErrors(await _userManager.CreateAsync(newUser));
                        CheckErrors(await _userManager.AddToRoleAsync(newUser, roleName));                      
                    }
                    else 
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (!roles.Contains(roleName))
                        {
                            CheckErrors(await _userManager.RemoveFromRolesAsync(user, roles));
                            CheckErrors(await _userManager.AddToRoleAsync(user, roleName));
                        }
                        CheckErrors(await _userManager.ChangePasswordAsync(user, User.WorkerUserDefaultPassword));
                    }
                }
            }
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roles = _roleManager.Roles.Where(r => user.Roles.Any(ur => ur.RoleId == r.Id)).Select(r => r.NormalizedName);
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
                .WhereIf(input.IsWorker, x => x.WorkerId.HasValue == true )
                .WhereIf(!input.IsWorker, x => x.WorkerId.HasValue == false )
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                .WhereIf(input.From.HasValue, x => x.CreationTime >= input.From.Value.LocalDateTime)
                .WhereIf(input.To.HasValue, x => x.CreationTime <= input.To.Value.LocalDateTime);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

