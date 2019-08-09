using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;

namespace Clc.Routes
{
    /// <summary>
    /// RouteEnvent Entity
    /// </summary>
    [Description("线路事件")]
    public class RouteEvent : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 10;
        public const int MaxDescriptionLength = ClcConsts.LargeStringLength;
        public const int MaxIssurerLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }
        public Route Route { get; set; }
        
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime EventTime { get; set; }

        /// <summary>
        /// 事件名
        /// </summary>
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        [StringLength(MaxIssurerLength)]
        public string Issurer { get; set; }
    }
}

