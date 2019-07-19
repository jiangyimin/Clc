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

        public static int GetDepotId(this IAbpSession session)
        {
            var id = GetClaimValue(session, "DEPOTID");
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("请用工作人员编号登录！(可能需要重新登录)");
            }
            else
            {
                return int.Parse(id);
            }          
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
