using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Clc.Controllers
{
    public abstract class ClcControllerBase: AbpController
    {
        protected ClcControllerBase()
        {
            LocalizationSourceName = ClcConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
