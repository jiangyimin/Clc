using System;
using System.Linq;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace Clc.Extensions
{
    public static class AbpSessionExtension
    {
        public static string GetClaimValue(this IAbpSession session, string claimType)
        {
            return GetClaimValue(claimType);
        }

        private static string GetClaimValue(string claimType)
        {
            // important
            var PrincipalAccessor = IocManager.Instance.Resolve<IPrincipalAccessor>(); // 使用IOC容器获取当前用户身份认证信息

            var claimsPrincipal = PrincipalAccessor.Principal;
            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;

            return claim.Value;
        }
    }
}
