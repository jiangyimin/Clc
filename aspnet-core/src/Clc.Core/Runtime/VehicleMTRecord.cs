using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Runtime
{
    /// <summary>
    /// VehicleMTRecord Entity
    /// </summary>
    [Description("车辆维修记录")]
    public class VehicleMTRecord : Entity, IMustHaveTenant
    {        
        public const int MaxContentLength = 512;
        public const int MaxRemarkLength = 512;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        // 创建时间
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }

        // 车辆
        [Required]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        [Required]
        public int VehicleMTTypeId { get; set; }
        public VehicleMTType VehicleMTType { get; set; }

        // 日期
        public DateTime MTDate { get; set; }
        
        public double Price { get; set; }
        
        [Required]
        [StringLength(MaxContentLength)]
        public string Content { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

        // 确认时间
        public DateTime? ConfirmTime { get; set; }

        public int? ProcessWorkerId { get; set; }
        public Worker ProcessWorker { get; set; }

    }
}

