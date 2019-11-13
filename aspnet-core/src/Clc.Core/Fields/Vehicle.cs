using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Fields
{
    /// <summary>
    /// Vehicle Entity
    /// </summary>
    [Description("押运车辆")]
    public class Vehicle : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 8;
        public const int MaxLicenseLength = 10;
        public const int MaxRemarkLength = 50;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [Required]
        [StringLength(MaxLicenseLength)]
        public string License { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }      
    }
}

