using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Clc.Fields
{
    /// <summary>
    /// GasStation Entity
    /// </summary>
    [Description("加油站")]
    public class GasStation : Entity, IMustHaveTenant
    {        
        public const int MaxCnLength = 4;
        public const int MaxNameLength = 20;

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

        public string Remark { get; set; }

        public string DepotList { get; set; }
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

