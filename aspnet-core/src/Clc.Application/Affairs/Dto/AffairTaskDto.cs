using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Affairs;

namespace Clc.Affairs
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
        public string IsTomorrow { get; set; }

        [StringLength(AffairTask.MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

