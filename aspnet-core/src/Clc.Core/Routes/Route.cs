using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Routes
{
    /// <summary>
    /// Route Entity
    /// </summary>
    [Description("线路")]
    public class Route : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 20;
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
        /// 线路编号
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string RouteName { get; set; }
        /// <summary>
        /// RouteType
        /// </summary>
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int? AltVehicleId { get; set; }
        public Vehicle AltVehicle { get; set; }

        /// <summary>
        /// RouteType
        /// </summary>
        [Required]
        public int RouteTypeId { get; set; }
        public RouteType RouteType { get; set; }

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

        // 预计里程
        public float? Mileage { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }
        public DateTime CreateTime { get; set; }


        // 实际选填的开始结束时间
        public DateTime SetoutTime { get; set; }
        public DateTime ReturnTime { get; set; }

        public float ActualMileage { get; set; }

        [ForeignKey("RouteId")]
        public virtual List<RouteWorker> Workers { get; set; }
        
        [ForeignKey("RouteId")]
        public virtual List<RouteTask> Tasks { get; set; }
        
        [ForeignKey("RouteId")]
        public virtual List<RouteArticle> Articles { get; set; }

        [ForeignKey("RouteId")]
        public virtual List<RouteInBox> InBoxes { get; set; }
        
        [ForeignKey("RouteId")]
        public virtual List<RouteOutBox> OutBoxes { get; set; }
    }
}

