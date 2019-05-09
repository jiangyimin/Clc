using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Types.Entities
{
    /// <summary>
    /// WarehouseArrangeType Entity
    /// </summary>
    [Description("金库工作类型之角色")]
    public class VaultWorkTypeRole : Entity
    {        
        /// <summary>
        /// 主表
        /// </summary>
        public int VaultWorkTypeId { get; set; }
        public VaultWorkType VaultWorkType { get; set; }

        /// <summary>
        /// 对应的工作角色
        /// </summary>
        public int WorkRoleId { get; set; }
        public WorkRole WorkRole { get; set; }

        /// <summary>
        /// 显示序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 人数， 0表示必须安排1人
        /// </summary>
        public int MaxNum { get; set; }
    }
}