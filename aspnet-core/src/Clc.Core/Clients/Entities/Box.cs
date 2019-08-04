using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Clients.Entities
{
    /// <summary>
    /// Box Entity
    /// </summary>
    [Description("尾箱")]
    public class Box : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 10;
        public const int MaxNameLength = 20;
        public const int MaxRemarkLength = 50;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 外键，网点
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        public Outlet Outlet { get; set; }

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
        /// Rfid 
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public string Contact { get; set; }
    }
}

