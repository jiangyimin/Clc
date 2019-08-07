using System.Linq;
using Microsoft.EntityFrameworkCore;
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
                PermissionNames.Pages_Setup, PermissionNames.Pages_Types, PermissionNames.Pages_Fields, PermissionNames.Pages_Clients
            };
            CreateRoleAndUser(StaticRoleNames.Tenants.Admin, permissions, false, AbpUserBase.AdminUserName, User.DefaultPassword);

            // Hrm RoleAndUser
            permissions = new string[] { PermissionNames.Pages_Hrm };
            CreateRoleAndUser(StaticRoleNames.Tenants.Hrm, permissions, false, StaticRoleNames.Tenants.Hrm, User.UserDefaultPassword);

            // Hrq RoleAndUser
            permissions = new string[] { PermissionNames.Pages_Hrq };
            CreateRoleAndUser(StaticRoleNames.Tenants.Hrq, permissions, false, StaticRoleNames.Tenants.Hrq, User.UserDefaultPassword);

            // Query RoleAndUser
            permissions = new string[] { PermissionNames.Pages_Query };
            CreateRoleAndUser(StaticRoleNames.Tenants.Query, permissions, false, StaticRoleNames.Tenants.Query, User.UserDefaultPassword);

            //
            // Worker Roles
            //
            // Captain
            permissions = new string[] { 
                PermissionNames.Pages_PreArrange, PermissionNames.Pages_Arrange, PermissionNames.Pages_Statistic, PermissionNames.Pages_Aux
            };

            CreateRole(StaticRoleNames.Tenants.Captain, permissions, true);

            // Aux
            permissions = new string[] { PermissionNames.Pages_Aux };
            CreateRole(StaticRoleNames.Tenants.Aux, permissions, true);
            // Monitor 
            permissions = new string[] { PermissionNames.Pages_Monitor };
            CreateRole(StaticRoleNames.Tenants.Monitor, permissions, true);

            // Article
            permissions = new string[] { PermissionNames.Pages_Article };
            CreateRole(StaticRoleNames.Tenants.Article, permissions, true);

            // Box
            permissions = new string[] { PermissionNames.Pages_Box };
            CreateRole(StaticRoleNames.Tenants.Box, permissions, true);

            // ArticleAndBox
            permissions = new string[] { PermissionNames.Pages_Article, PermissionNames.Pages_Box };
            CreateRole(StaticRoleNames.Tenants.ArticleAndBox, permissions, true);           
        }

        private void CreateRoleAndUser(string roleName, string[] permissions, bool isWorkerRole, string userName, string password)
        {
            var role = CreateRole(roleName, permissions, isWorkerRole);
            if (role == null) 
                return;
            // user
            var user = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == userName);
            if (user == null)
            {
                user = User.CreateUser(_tenantId, userName, password);
                _context.Users.Add(user);
                _context.SaveChanges();

                // Assign role to user
                _context.UserRoles.Add(new UserRole(_tenantId, user.Id, role.Id));
                _context.SaveChanges();
            }
        }

        private Role CreateRole(string name, string[] permissions, bool isWorkerRole)
        {
            // role
            var role = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == name);
            if (role == null)
            {
                role = _context.Roles.Add(new Role(_tenantId, name, name) { IsStatic = true, IsWorkerRole = isWorkerRole }).Entity;
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
    }
}
