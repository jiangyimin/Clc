using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Clients;
using Clc.Runtime;

namespace Clc.Routes
{
    /// <summary>
    /// RouteOutBox
    /// </summary>
    [Description("线路出箱")]
    public class RouteOutBox : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteTaskId { get; set; }

        [Required]
        public int BoxRecordId { get; set; }
        public BoxRecord BoxRecord { get; set; }

        // public DateTime? DeliverTime { get; set; }
    }
}

