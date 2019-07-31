using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Fields.Entities
{
    /// <summary>
    /// Vehicle Entity
    /// </summary>
    [Description("押运车辆")]
    public class Vehicle : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 8;
        public const int LicenseLength = 7;

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
        [StringLength(LicenseLength)]
        public string License { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }
    }
}

