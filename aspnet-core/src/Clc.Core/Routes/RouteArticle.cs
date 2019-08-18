using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Runtime;

namespace Clc.Routes
{
    /// <summary>
    /// RouteTask Entity
    /// </summary>
    [Description("线路物品")]
    public class RouteArticle : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// Workplace
        /// </summary>
        [Required]
        public int RouteWorkerId { get; set; }

        [Required]
        public int ArticleRecordId { get; set; }
        public virtual ArticleRecord ArticleRecord { get; set; }        
    }
}

