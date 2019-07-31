using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Domain.Entities;
using Clc.Types.Entities;
using Clc.Fields.Entities;

namespace Clc.Works
{
    /// <summary>
    /// WhAffair Entity
    /// </summary>
    [Description("库房工作任务")]
    public class WarehouseTask : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = 50;
        public const int MaxStatusLength = 4;
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：中心Id
        /// </summary>
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// 库房
        /// </summary>
        [Required]
        [StringLength(Workplace.MaxNameLength)]
        public string WarehorseName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态（安排，活动，结束, 日结）
        /// </summary>
        [Required]
        [StringLength(MaxStatusLength)]
        public string Status { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

