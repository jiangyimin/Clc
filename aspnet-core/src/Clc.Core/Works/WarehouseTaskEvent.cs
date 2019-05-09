using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;

namespace Clc.Works
{
    /// <summary>
    /// WhAffairWorker Entity
    /// </summary>
    [Description("库房工作事件")]
    public class WarehouseTaskEvent : Entity
    {
        public const int MaxNameLength = 50;
        public const int MaxContentLength = 50;
        public const int MaxIssurerLength = 20;
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int WarehouseTaskId { get; set; }
        public WarehouseTask WarehouseTask { get; set; }
        
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
        [StringLength(MaxContentLength)]
        public string Content { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        [StringLength(MaxIssurerLength)]
        public string Issurer { get; set; }
    }
}

