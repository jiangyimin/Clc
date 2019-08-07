using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Types.Entities;

namespace Clc.Fields.Entities
{
    /// <summary>
    /// Workplace Entity
    /// </summary>
    [Description("工作场地")]
    public class Workplace : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;
        public const int ArticleTypeListLength = ClcConsts.NormalStringLength;
        public const int ShareDepotListLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 场所名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public int AffairTypeId { get; set; }
        public AffairType AffairType { get; set; }
        
        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 共享运行中心列表
        /// </summary>
        [StringLength(ShareDepotListLength)]
        public string ShareDepotList { get; set; }
    }
}

