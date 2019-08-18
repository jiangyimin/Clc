using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Affairs;
using Clc.Clients;
using Clc.Fields;
using Clc.Routes;

namespace Clc.Runtime
{
    /// <summary>
    /// BoxRecord Entity
    /// </summary>
    [Description("尾箱进出记录")]
    public class BoxRecord : Entity, IMustHaveTenant
    {
        public const int MaxWorkersLength = 64;
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键：尾箱
        /// </summary>
        [Required]
        public int BoxId { get; set; }
        public virtual Box Box { get; set; }

        /// <summary>
        /// 外键：领用人
        /// </summary>
        [Required]
        public int RouteTaskId { get; set; }
        public virtual RouteTask RouteTask { get; set; }

        /// <summary>
        /// 进时间
        /// </summary>
        [Required]
        public DateTime InTime { get; set; }

        /// <summary>
        /// 出时间
        /// </summary>
        public DateTime? OutTime { get; set; }

        [Required]
        [StringLength(MaxWorkersLength)]
        public string InWorkers { get; set; }
        
        [StringLength(MaxWorkersLength)]
        public string OutWorkers { get; set; }
    }
}

