using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Routes;

namespace Clc.Runtime
{
    /// <summary>
    /// VehicleRecord Entity
    /// </summary>
    [Description("加油记录")]
    public class VehicleRecord : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = 64;
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 填写时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 外键：车辆
        /// </summary>
        [Required]
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        /// <summary>
        /// 外键：填写人
        /// </summary>
        [Required]
        public int WorkerId { get; set; }
        public virtual Worker Worker { get; set; }

        [Required]
        public double CurrentMileage { get; set; }

        [Required]
        public double Quantity { get; set; }

        
        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }       
    }
}

