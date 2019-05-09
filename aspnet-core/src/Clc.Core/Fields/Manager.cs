﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Authorization.Roles;

namespace Clc.Fields
{
    /// <summary>
    /// Manager Entity
    /// </summary>
    [Description("公司管理人员")]
    public class Manager : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Worker.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(Worker.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(Worker.MobileLength)]
        public string Mobile { get; set; }

        //
        // 摘要:
        //     Current Role of the manager.
        public Role Role { get; set; }
        public int? RoleId { get; set; }
    }
}

