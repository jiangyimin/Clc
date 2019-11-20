using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;

namespace Clc.Routes.Dto
{
    /// <summary>
    /// RouteDto
    /// </summary>
    [AutoMap(typeof(Route))]
    public class RouteDto : EntityDto, ICustomValidate
    {
        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// 线路名
        /// </summary>
        [Required]
        [StringLength(Route.MaxNameLength)]
        public string RouteName { get; set; }

        /// <summary>
        /// RouteType
        /// </summary>
        [Required]
        public int RouteTypeId { get; set; }
        public string RouteTypeName { get; set; }

        /// <summary>
        /// 状态（生成，活动，结束）
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Status { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        // [Required]
        public int VehicleId { get; set; }
        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }

        public int? AltVehicleId { get; set; }
        public string AltVehicleCn { get; set; }
        public string AltVehicleLicense { get; set; }
        
        /// <summary>
        /// 预计出发时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string StartTime { get; set; }

        /// <summary>
        /// 预计返回时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string EndTime { get; set; }

        // 预计里程
        public float? Mileage { get; set; }

        [StringLength(Route.MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public string CreateWorkerName { get; set; }

        // 实际选填的开始结束时间
        public DateTime SetoutTime { get; set; }
        public DateTime ReturnTime { get; set; }

        public float ActualMileage { get; set; }
        
        // only for mds.js 
        public string Postfix { get; } = "";

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (string.Compare(StartTime, EndTime) >= 0) 
                 context.Results.Add(new ValidationResult("出发时间不能晚于返回时间"));
        }

    }
}

