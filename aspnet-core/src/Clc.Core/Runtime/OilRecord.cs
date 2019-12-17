using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Runtime
{
    /// <summary>
    /// OilRecord Entity
    /// </summary>
    [Description("加油记录")]
    public class OilRecord : Entity, IMustHaveTenant
    {        
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
        public int GasStationId { get; set; }
        public GasStation GasStation { get; set; }

        [Required]
        public int OilTypeId { get; set; }
        public OilType OilType { get; set; }

        public double Quantity { get; set; }
        public double Price { get; set; }

        public double Mileage { get; set; }
        
        
        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }


        // 确认时间
        public DateTime? ConfirmTime { get; set; }

        public int? ProcessWorkerId { get; set; }
        public Worker ProcessWorker { get; set; }

    }
}

