using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;

namespace Clc.Runtime
{
    /// <summary>
    /// Issur Entity
    /// </summary>
    [Description("大队事件")]
    public class Issue : Entity, IMustHaveTenant
    {        
        public const int MaxContentLength = 512;
        public const int MaxProcessStyleLength = 20;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        // 创建时间
        public DateTime CreateTime { get; set; }

        // 大队
        [Required]
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        
        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        [Required]
        public int CreateWorkerId { get; set; }
        public Worker CreateWorker { get; set; }
        
        [Required]
        [StringLength(MaxContentLength)]
        public string Content { get; set; }

        // 目前有三种：1）报告  2）应急开门 3) 值班
        public string ProcessStyle { get; set; }


        // 签到时间
        public DateTime? ProcessTime { get; set; }

        public int? ProcessWorkerId { get; set; }
        public Worker ProcessWorker { get; set; }

        [StringLength(MaxContentLength)]
        public string ProcessContent { get; set; }
    }
}

