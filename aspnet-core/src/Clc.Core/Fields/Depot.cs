using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Clc.Fields
{
    /// <summary>
    /// Depot Entity
    /// </summary>
    [Description("运行中心")]
    public class Depot : Entity, IMustHaveTenant
    {        
        public const int MaxCnLength = 2;
        public const int MaxNameLength = 8;
        public const int MaxPasswordLength = 8;
        public const int MaxReportToLength = 50;


        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 运行中心名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// 签到半径(米)，如空采用缺省的
        /// </summary>
        public int? Radius { get; set; }
    
        [StringLength(MaxPasswordLength)]
        public string UnlockScreenPassword { get; set; }

        [StringLength(MaxReportToLength)]
        public string ReportTo { get; set; }

        // public DateTime? LastReportDate { get; set; }

        [StringLength(Worker.MaxCnLength)]
        public string AgentCn { get; set; }

        /// <summary>
        /// 激活线路是否需要全部签到
        /// </summary>
        public bool ActiveRouteNeedCheckin { get; set; }

        /// <summary>
        /// 验入允许刷卡
        /// </summary>
        public bool AllowCardWhenCheckin { get; set; }

        /// <summary>
        /// 本地解屏
        /// </summary>
        public bool LocalUnlockScreen { get; set; }
    }
}

