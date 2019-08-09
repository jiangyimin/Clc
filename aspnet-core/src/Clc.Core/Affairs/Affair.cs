using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        /// 状态（生成，活动，结束）
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
        public bool IsTomorrow { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

