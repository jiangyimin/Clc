using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Clc.Authorization;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;

namespace Clc.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly ClcDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(ClcDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            // Admin RoleAndUser
            string[] permissions = new string[] {
                PermissionNames.Pages_Setup, PermissionNames.Pages_Types, PermissionNames.Pages_Fields, PermissionNames.Pages_Customers
            };
            CreateRoleAndUser(StaticRoleNames.Tenants.Admin, permissions, AbpUserBase.AdminUserName, User.DefaultPassword);

            // Hrm RoleAndUser
            permissions = new string[] { PermissionNames.Pages_Hrm };
            CreateRoleAndUser(StaticRoleNames.Tenants.Hrm, permissions, StaticRoleNames.Tenants.Hrm, User.UserDefaultPassword);

            // Hrq RoleAndUser
            permissions = new string[] { PermissionNames.Pages_Hrq };
            CreateRoleAndUser(StaticRoleNames.Tenants.Hrq, permissions, StaticRoleNames.Tenants.Hrq, User.UserDefaultPassword);

            // Query RoleAndUser
            permissions = new string[] { PermissionNames.Pages_Query };
            CreateRoleAndUser(StaticRoleNames.Tenants.Query, permissions, StaticRoleNames.Tenants.Query, User.UserDefaultPassword);

            // PlaceC RoleAndUser
            permissions = new string[] { PermissionNames.Pages_PlaceC };
            CreateRoleAndUser(StaticRoleNames.Tenants.PlaceC, permissions, StaticRoleNames.Tenants.PlaceC, User.RoleUserPassword);

            // 分部角色
            // PlaceA
            permissions = new string[] { PermissionNames.Pages_PlaceA };
            CreateRoleAndUser(StaticRoleNames.Tenants.PlaceA, permissions, StaticRoleNames.Tenants.PlaceA, User.RoleUserPassword);

            // PlaceB
            permissions = new string[] { PermissionNames.Pages_PlaceB };
            CreateRoleAndUser(StaticRoleNames.Tenants.PlaceB, permissions, StaticRoleNames.Tenants.PlaceB, User.RoleUserPassword);
            
            // Captain
            permissions = new string[] { 
                PermissionNames.Pages_PreArrange, PermissionNames.Pages_TodayArrange, PermissionNames.Pages_Aux
            };
            CreateRoleAndUser(StaticRoleNames.Tenants.Captain, permissions, StaticRoleNames.Tenants.Captain, User.RoleUserPassword);

            // Aux
            permissions = new string[] { PermissionNames.Pages_Aux };
            CreateRoleAndUser(StaticRoleNames.Tenants.Aux, permissions, StaticRoleNames.Tenants.Aux, User.RoleUserPassword);
        }

        private void CreateRoleAndUser(string roleName, string[] permissions, string userName, string password)
        {
            var role = CreateRole(roleName, permissions);
            if (role == null) 
                return;
            // user
            var user = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == userName);
            if (user == null)
            {
                user = CreateUser(_tenantId, userName, password);
                _context.Users.Add(user);
                _context.SaveChanges();

                // Assign role to user
                _context.UserRoles.Add(new UserRole(_tenantId, user.Id, role.Id));
                _context.SaveChanges();
            }
        }

        private Role CreateRole(string name, string[] permissions)
        {
            // role
            var role = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == name);
            if (role == null)
            {
                role = _context.Roles.Add(new Role(_tenantId, name, name) { IsStatic = true }).Entity;
                _context.SaveChanges();

                // Grant permission to role
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting 
                    { 
                        TenantId = _tenantId, 
                        Name = permission, 
                        IsGranted = true, 
                        RoleId = role.Id 
                    })
                );
                _context.SaveChanges();
            }
             
            return role;
        }

        private User CreateUser(int tenantId, string name, string password)
        {
            var user = new User
            {
                TenantId = _tenantId,
                UserName = name,
                Name = name,
                Surname = name,
                EmailAddress = name + ClcConsts.UserEmailServerName,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();
            user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, password);
            user.IsEmailConfirmed = true;
            user.IsActive = true;
            return user;
        }
    }
}
