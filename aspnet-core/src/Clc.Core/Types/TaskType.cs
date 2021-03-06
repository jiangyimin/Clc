﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types
{
    /// <summary>
    /// TaskType Entity
    /// </summary>
    [Description("押运任务类型")]
    public class TaskType : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 2;
        public const int MaxNameLength = 8;
 
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
       
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }
        
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public bool isTemporary { get; set; }
        public int BasicPrice { get; set; }
    }
}

