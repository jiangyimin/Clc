using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Clc.Authorization.Users;
using Clc.MultiTenancy;
using Clc.Works;
using Abp.UI;

namespace Clc
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ClcAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public WorkManager WorkManager { get; set; }

        protected ClcAppServiceBase()
        {
            LocalizationSourceName = ClcConsts.LocalizationSourceName;
        }

        protected virtual async Task<int> GetCurrentUserWorkerIdAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user.WorkerId.HasValue == false)
            {
                throw new UserFriendlyException("必须是运行场所的工作人员登录");
            }
            return user.WorkerId.Value;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
