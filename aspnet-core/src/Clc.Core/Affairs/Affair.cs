using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Fields;

namespace Clc.Affairs
{
    /// <summary>
    /// Affair Entity
    /// </summary>
    [Description("内务")]
    public class Affair : Entity, IMustHaveTenant
    {
        public const int MaxContentLength = 30;
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：中心Id
        /// </summary>
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// Workplace
        /// </summary>
        [Required]
        public int WorkplaceId { get; set; }
        public Workplace Workplace { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        [Required]
        [StringLength(MaxContentLength)]
        public string Content { get; set; }
        
        /// <summary>
        /// 状态（生成，活动，关闭）
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string EndTime { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey("AffairId")]
        public virtual List<AffairWorker> Workers { get; set; }

        [ForeignKey("AffairId")]
        public virtual List<AffairTask> Tasks { get; set; }
    }
}

