using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using System;

namespace Clc.Fields
{
    /// <summary>
    /// WorkerFile Entity
    /// </summary>
    [Description("人员档案")]
    public class Asset : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 20;
        public const int MaxCategoryLength = 10;
        public const int MaxNameLength = 50;
        public const int MaxAddressLength = 50;
        public const int MaxRemarkLength = 200;
 
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 所属Depot
        /// </summary>
        public int DepotId { get; set; }
        public Depot Depot { get; set; }
        
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Required]
        [StringLength(MaxCategoryLength)]
        public string Category { get; set; }

        /// <summary>
        /// 存放地
        /// </summary>
        [Required]
        [StringLength(MaxAddressLength)]
        public string Address { get; set; }

        /// <summary>
        /// 保管责任人
        /// </summary>
        [StringLength(MaxAddressLength)]
        public string ChargePerson { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime UseDate { get; set; }

        /// <summary>
        /// 预计报废日期
        /// </summary>
        public DateTime RetireDate { get; set; }

        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }

    }
}

