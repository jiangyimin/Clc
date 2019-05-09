using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types.Entities
{
    /// <summary>
    /// WarehouseArrangeType Entity
    /// </summary>
    [Description("库房工作类型")]
    public class WarehouseWorkType : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;
 
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
               
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 签到提前时间（分钟）
        /// </summary>
        public int CheckinLead { get; set; }

        /// <summary>
        /// 签到关闭时间（分钟）
        /// </summary>
        public int CheckinDeadline { get; set; }
    }
}