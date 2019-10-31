using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

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
        /// 缺省的角色名称
        /// </summary>
        [StringLength(MaxNameLength)]
        public string DefaultWorkRoleName { get; set; }    

        /// <summary>
        /// 企业微信应用名
        /// </summary>
        [StringLength(MaxNameLength)]
        public string AppName { get; set; }    
    }
}

