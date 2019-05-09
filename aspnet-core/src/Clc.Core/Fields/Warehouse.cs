using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Clc.Fields
{
    /// <summary>
    /// Warehouse Entity
    /// </summary>
    [Description("库房")]
    public class Warehouse : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;
        public const int ArticleTypeListLength = 50;
        public const int ShiftNameListLength = 50;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 库房名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 每日班次民称列表
        /// </summary>
        [StringLength(ShiftNameListLength)]
        public string ShiftNameList { get; set; }

        /// <summary>
        /// 每班最短时长
        /// </summary>
        public int MinDuration { get; set; }     
        /// <summary>
        /// 每班最长时长
        /// </summary>
        public int MaxDuration { get; set; }     
    }
}

