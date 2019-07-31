using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields.Entities;

namespace Clc.Works
{
    /// <summary>
    /// WhAffairWorker Entity
    /// </summary>
    [Description("库房内务工作人员")]
    public class WarehouseTaskWorker : Entity
    {
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int WarehouseTaskId { get; set; }
        public WarehouseTask WarehouseTask { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        
        /// <summary>
        /// 签入时间
        /// </summary>
        public DateTime? Checkin { get; set; }

        /// <summary>
        /// 签出时间
        /// </summary>
        public DateTime? Checkout { get; set; }
    }
}

