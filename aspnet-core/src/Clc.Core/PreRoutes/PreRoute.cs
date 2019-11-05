using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Routes;
using Clc.Types;

namespace Clc.PreRoutes
{
    /// <summary>
    /// PreRoute Entity
    /// </summary>
    [Description("预排线路")]
    public class PreRoute : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：中心Id
        /// </summary>
        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

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
        public virtual RouteType RouteType { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        [Required]
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

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

    }
}

