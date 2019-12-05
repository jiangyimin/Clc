using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Affairs.Dto
{
    /// <summary>
    /// AffairTaskDto
    /// </summary>
    [AutoMap(typeof(AffairTask))]
    public class AffairTaskDto : EntityDto
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int AffairId { get; set; }

        /// <summary>
        /// Workplace
        /// </summary>
        [Required]
        public int WorkplaceId { get; set; }
        public string WorkplaceName { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        [Required]
        [StringLength(AffairTask.MaxContentLength)]
        public string Content { get; set; }
        
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

        [StringLength(AffairTask.MaxRemarkLength)]
        public string Remark { get; set; }

        public string CreateWorkerName { get; set; }

        // 实际选填的开始结束时间
        public DateTime? StartTimeActual { get; set; }
        public DateTime? EndTimeActual { get; set; }
        
        // only for mds.js 
        public string Postfix { get; } = "Task";

    }
}

