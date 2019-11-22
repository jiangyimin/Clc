using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types
{
    /// <summary>
    /// RouteType Entity
    /// </summary>
    [Description("线路类型")]
    public class RouteType : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;
        public const int MaxWorkRolesLength = ClcConsts.NormalStringLength;
 
        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
               
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 工作角色列表
        /// </summary>
        [Required]
        [StringLength(MaxWorkRolesLength)]
        public string WorkRoles { get; set; }
        
        /// <summary>
        /// 领物提前时间（分钟）
        /// </summary>
        public int LendArticleLead { get; set; }

        /// <summary>
        /// 领物关闭时间（分钟）
        /// </summary>
        public int LendArticleDeadline { get; set; }

        /// <summary>
        /// 激活提前时间（分钟）
        /// </summary>
        public int ActivateLead { get; set; }        
    }
}