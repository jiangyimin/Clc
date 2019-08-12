using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Clc.Routes;

namespace Clc.PreRoutes.Dto
{
    /// <summary>
    /// AffairDto
    /// </summary>
    [AutoMap(typeof(PreRoute))]
    public class PreRouteDto : EntityDto, ICustomValidate
    {
        /// <summary>
        /// RouteType
        /// </summary>
        [Required]
        public int RouteTypeId { get; set; }
        public string RouteTypeName { get; set; }

        /// <summary>
        /// 线路名
        /// </summary>
        [Required]
        [StringLength(Route.MaxNameLength)]
        public string RouteName { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        // [Required]
        public int? VehicleId { get; set; }
        // only mapfrom
        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }


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

        // only for mds.js 
        public string Postfix { get; } = "";

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (string.Compare(StartTime, EndTime) >= 0) 
                 context.Results.Add(new ValidationResult("出发时间不能晚于返回时间"));
        }

    }
}

