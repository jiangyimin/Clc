using System.Collections.Generic;
using Clc.Roles.Dto;
using Clc.Users.Dto;

namespace Clc.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
