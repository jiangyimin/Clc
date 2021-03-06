using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes.Dto
{
    /// <summary>
    /// RouteTaskDto
    /// </summary>
    [AutoMap(typeof(RouteTask))]
    public class RouteTaskDto : EntityDto
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string ArriveTime { get; set; }

        /// <summary>
        /// Outlet
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        public string OutletCn { get; set; }
        public string OutletName { get; set; }

        [Required]
        public int TaskTypeId { get; set; }
        public string TaskTypeName { get; set; }

        [StringLength(RouteTask.MaxRemarkLength)]
        public string Remark { get; set; }

        public DateTime? IdentifyTime { get; set; }

        public string Rated { get; set; }
        public string OutletIdentifyInfo { get; set; }

        public string CreateWorkerCn { get; set; }
        public string CreateWorkerName { get; set; }
        public string InBoxList { get; set; }
        public string OutBoxList { get; set; }
        public int InBoxNum { get; set; }
        public int OutBoxNum { get; set; }
        // only for mds.js 
        public string Postfix { get; } = "Task";
    }
}

