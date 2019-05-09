using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types.Entities
{
    /// <summary>
    /// TaskType Entity
    /// </summary>
    [Description("工作角色")]
    public class WorkRole : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 2;
        public const int MaxNameLength = 8;
        public const int MaxDutiesLength = 100;
        public const int MaxCategoryLength = 100;

 
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

        public WorkerType DefaultWorkerType { get; set; }
        public int DefaultWorkerTypeId { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [Required]
        [StringLength(MaxCategoryLength)]
        public string Category { get; set; }
        /// <summary>
        /// 职责
        /// </summary>
        [Required]
        [StringLength(MaxDutiesLength)]
        public string Duties { get; set; }
    }
}

