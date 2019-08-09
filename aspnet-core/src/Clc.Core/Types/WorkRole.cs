using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types
{
    /// <summary>
    /// WorkRole Entity
    /// </summary>
    [Description("工作角色")]
    public class WorkRole : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 2;
        public const int MaxNameLength = 8;
        public const int MaxDutiesLength = ClcConsts.NormalStringLength;
        public const int MaxArticleTypeListLength = ClcConsts.NormalStringLength;

        // Implement of IMustHaveTenant
        public int TenantId { get; set; }
       
        /// <summary>
        /// 编号/序号
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
        /// 缺省的工作岗位
        /// </summary>
        public int? DefaultPostId { get; set; }
        public Post DefaultPost { get; set; }

        /// <summary>
        /// 职责列表（职责名系统内定）
        /// </summary>
        [StringLength(MaxDutiesLength)]
        public string Duties { get; set; }

        [StringLength(MaxArticleTypeListLength)]
        public string ArticleTypeList { get; set; }
        /// <summary>
        /// 必须安排
        /// </summary>
        [DefaultValue(true)]
        public bool mustHave { get; set; }
        /// <summary>
        /// 最多人数
        /// </summary>
        public int MaxNum { get; set; }
    }
}

