using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Clients;
using Clc.Fields;
using Clc.Types;

namespace Clc.Routes
{
    /// <summary>
    /// RouteTask Entity
    /// </summary>
    [Description("线路任务")]
    public class RouteTask : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;
        public const int MaxOutletIdentifyInfoLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }
        public virtual Route Route { get; set; }

        /// <summary>
        /// 预计达到时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string ArriveTime { get; set; }

        /// <summary>
        /// 外键：银行网点
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }

        /// <summary>
        /// 外键：任务类型
        /// </summary>
        [Required]
        public int TaskTypeId { get; set; }
        public virtual TaskType TaskType { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 身份确认时间
        /// </summary>
        public DateTime? IdentifyTime { get; set; }

        [StringLength(MaxOutletIdentifyInfoLength)]
        public string OutletIdentifyInfo { get; set; }

        public float? Price { get; set; }

        [ForeignKey("RouteTaskId")]
        public virtual List<RouteInBox> InBoxes { get; set; }

        [ForeignKey("RouteTaskId")]
        public virtual List<RouteOutBox> OutBoxes { get; set; }
    }
}

