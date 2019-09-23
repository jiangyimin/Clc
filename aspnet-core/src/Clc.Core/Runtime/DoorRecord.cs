using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Affairs;
using Clc.Fields;
using Clc.Routes;

namespace Clc.Runtime
{
    /// <summary>
    /// DoorRecord Entity
    /// </summary>
    [Description("开门记录")]
    public class DoorRecord : Entity, IMustHaveTenant
    {
        public const int MaxWorkersLength = 64;
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：场所
        /// </summary>
        [Required]
        public int WorkplaceId { get; set; }
        public virtual Workplace Workplace { get; set; }

        /// <summary>
        /// 开门人（任务号）
        /// </summary>
        [Required]
        public int OpenAffairId { get; set; }
        public virtual Affair OpenAffair { get; set; }

        /// <summary>
        /// 申请任务（任务号）
        /// </summary>
        public int? ApplyAffairId { get; set; }
        public virtual Affair ApplyAffair { get; set; }

        /// <summary>
        /// 开门时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
    }
}

