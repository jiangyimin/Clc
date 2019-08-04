using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Clc.Clients.Entities
{
    /// <summary>
    /// Customer Entity
    /// </summary>
    [Description("押运客户")]
    public class Customer : Entity, IMustHaveTenant
    {
        public const int MaxCnLength = 4;
        public const int MaxNameLength = 50;
        public const int MaxContactLength = 50;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

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
        [StringLength(MaxContactLength)]
        public string Contact { get; set; }
    }
}

