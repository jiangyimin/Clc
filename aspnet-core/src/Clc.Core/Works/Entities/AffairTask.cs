using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields.Entities;
using Clc.Types.Entities;

namespace Clc.Works.Entities
{
    /// <summary>
    /// AffairTask Entity
    /// </summary>
    [Description("内务任务")]
    public class AffairTask : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int AffairId { get; set; }
        public Affair Affair { get; set; }

        /// <summary>
        /// Workplace
        /// </summary>
        [Required]
        public int WorkplaceId { get; set; }
        public Workplace Workplace { get; set; }

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
        public bool isTomorrow { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

