using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using Clc.Fields.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Clc.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";
        public const string UserDefaultPassword = "123456";
        public const string WorkerUserDefaultPassword = "WorkerUser@123456";
       
        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }

        public static User CreateUser(int tenantId, string name, string password)
        {
            var user = new User
            {
                TenantId = tenantId,
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
