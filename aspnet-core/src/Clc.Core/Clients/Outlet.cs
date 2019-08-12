using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Clients
{
    /// <summary>
    /// Outlet Entity
    /// </summary>
    [Description("押运网点")]
    public class Outlet : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 6;
        public const int MaxNameLength = 50;
        public const int MaxPasswordLength = 6;
        public const int MaxContactLength = 50;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        // 客户外键
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
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
        /// 交接密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 交接密文
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Ciphertext { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        [StringLength(MaxContactLength)]
        public string Contact { get; set; }

        [StringLength(MaxContactLength)]
        public string Weixins { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }
    }
}

