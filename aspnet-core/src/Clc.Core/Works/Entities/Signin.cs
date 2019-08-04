using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields.Entities;

namespace Clc.Works.Entities
{
    /// <summary>
    /// Signin Entity
    /// </summary>
    [Description("签到")]
    public class Signin : Entity, IMustHaveTenant
    {        
        public const int MaxWorkerLength = 20;
        
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        // 签到时间
        public DateTime SigninTime { get; set; }


        // 签到地点
        [Required]
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        
    }
}

