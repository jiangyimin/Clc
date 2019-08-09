using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Authorization.Roles;

namespace Clc.Types
{
    /// <summary>
    /// Post Entity
    /// </summary>
    [Description("人员岗位")]
    public class Post : Entity, IMustHaveTenant
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
        /// 岗位名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// 工作人员角色名
        /// </summary>
        [StringLength(Role.MaxNameLength)]
        public string WorkerRoleName { get; set; }
    }
}

