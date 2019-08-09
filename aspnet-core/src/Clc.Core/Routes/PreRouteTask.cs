using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Clients;
using Clc.Types;

namespace Clc.Routes
{
    /// <summary>
    /// PreRouteTask Entity
    /// </summary>
    [Description("预排线路任务")]
    public class PreRouteTask : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int PreRouteId { get; set; }
        public virtual Route PreRoute { get; set; }

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

        [StringLength(RouteTask.MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

