using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types.Entities
{
    /// <summary>
    /// TaskType Entity
    /// </summary>
    [Description("工作人员类型")]
    public class WorkerType : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 2;
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
    }
}

