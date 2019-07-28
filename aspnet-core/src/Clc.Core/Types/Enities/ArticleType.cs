using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types.Entities
{
    /// <summary>
    /// ArticleType Entity
    /// </summary>
    [Description("物品类型")]
    public class ArticleType : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 2;
        public const int MaxNameLength = 8;
        public const int MaxBindStyleLength = 8;

 
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
        /// 绑定方式（人，车，线路，机器，网点等）
        /// </summary>
        [StringLength(MaxBindStyleLength)]
        public string BindStyle { get; set; }
    }
}

