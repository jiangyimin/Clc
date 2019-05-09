using Abp.Authorization;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;

namespace Clc.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
