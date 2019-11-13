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
    /// EmergDoorRecord Entity
    /// </summary>
    [Description("应急开门记录")]
    public class EmergDoorRecord : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        /// <summary>
        /// 申请信息一: 创建时间
        /// </summary>
         [Required]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 申请信息二：需要开的门（即对应的场所）。与任务的场所不一定相同
        /// </summary>
        [Required]
        public int WorkplaceId { get; set; }
        public virtual Workplace Workplace { get; set; }

        /// <summary>
        /// 申请信息三：对应的大队重要任务
        /// </summary>
        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }

        /// <summary>
        /// 审批信息：审批
        /// </summary>
        public string Leader { get; set; }

        /// <summary>
        /// 审批信息：开门密码
        /// </summary>
        public string EmergDoorPassword { get; set; }

        /// <summary>
        /// 下面为开门人信息
        /// </summary>
        // 任务号
        public int? MonitorAffairId { get; set; }
        public virtual Affair MonitorAffair { get; set; }

        // 处理时间
        public DateTime? ProcessTime { get; set; }
        public string Remark { get; set; }
    }
}

