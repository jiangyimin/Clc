using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Authorization.Users;
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
        public const int ShiftNameListLength = 50;

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
        /// 角色用户名
        /// </summary>
        [StringLength(User.MaxNameLength)]
        public string RoleUserName { get; set; }
    }
}

