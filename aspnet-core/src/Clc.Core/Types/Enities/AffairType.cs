using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types.Entities
{
    /// <summary>
    /// AffairType Entity
    /// </summary>
    [Description("内务类型")]
    public class AffairType : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 1;
        public const int MaxNameLength = 8;

        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
       
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }
        
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 是否云端开门
        public bool hasCloudDoor { get; set; }

        /// <summary>
        /// 可安排到线路
        public bool toRoute { get; set; }

        public int MinDuration { get; set; }     
        /// <summary>
        /// 每班最长时长
        /// </summary>
        public int MaxDuration { get; set; }     
    }
}

