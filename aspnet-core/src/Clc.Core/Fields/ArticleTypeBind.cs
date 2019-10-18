using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Types;

namespace Clc.Fields
{
    /// <summary>
    /// ArticleTypeBind Entity
    /// </summary>
    [Description("绑定方式")]
    public class ArticleTypeBind : Entity, IMustHaveTenant
    {
        public const int MaxBindStyleLength = 8;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 地点名称
        /// </summary>
        [Required]
        public int ArticleTypeId { get; set; }
        public virtual ArticleType ArticleType { get; set; }
        
        /// <summary>
        /// 绑定方式（人，车，线路，机器，网点等）
        /// </summary>
        [StringLength(MaxBindStyleLength)]
        public string BindStyle { get; set; }
    }
}

