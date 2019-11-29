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
    [Description("申请及开门记录")]
    public class AskDoorRecord : Entity, IMustHaveTenant
    {
        public const int MaxAskWorkersLength = 200;
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 申请信息一: 申请时间
        /// </summary>
         [Required]
        public DateTime AskTime { get; set; }

        /// <summary>
        /// 申请信息二：需要开的门（即对应的场所）。与任务的场所不一定相同
        /// </summary>
        [Required]
        public int WorkplaceId { get; set; }
        public virtual Workplace Workplace { get; set; }


        /// <summary>
        /// 申请信息三：申请任务
        /// </summary>
        public int AskAffairId { get; set; }
        public virtual Affair AskAffair { get; set; }

        /// <summary>
        /// 申请信息四：申请人
        /// </summary>
        [StringLength(MaxAskWorkersLength)]
        public string AskWorkers { get; set; }

        /// <summary>
        /// 申请信息五：对于线路方式的申请，填入RouteId
        /// </summary>
        public int? RouteId { get; set; }
        public virtual Route Route { get; set; }

        // 申请原因
        public string AskReason { get; set; }

        // 审批人
        public string Approver { get; set; }

        /// <summary>
        /// 下面为开门人信息
        /// </summary>
        // 任务号
        public int? MonitorAffairId { get; set; }
        public virtual Affair MonitorAffair { get; set; }

        // 处理时间
        public DateTime? ProcessTime { get; set; }

        // 特殊情况说明
        public string Remark { get; set; }
    }
}

