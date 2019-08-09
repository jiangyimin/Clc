using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Clients;
using Clc.Runtime;

namespace Clc.Routes
{
    /// <summary>
    /// RouteInBox
    /// </summary>
    [Description("线路进箱")]
    public class RouteInBox : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }
        public Route Route { get; set; }

        /// <summary>
        /// Box
        /// </summary>
        [Required]
        public int BoxId { get; set; }
        public Box Box { get; set; }

        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteTaskId { get; set; }
        public RouteTask RouteTask { get; set; }

        [Required]
        public int BoxRecordId { get; set; }
        public BoxRecord BoxRecord { get; set; }
    }
}

