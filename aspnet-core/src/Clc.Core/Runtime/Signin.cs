using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;

namespace Clc.Runtime
{
    /// <summary>
    /// Signin Entity
    /// </summary>
    [Description("签到")]
    public class Signin : Entity, IMustHaveTenant
    {                
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        // 签到地点
        [Required]
        public int DepotId { get; set; }
        public Depot Depot { get; set; }

        // 日期
        public DateTime CarryoutDate { get; set; }
        
        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        
        // 签到时间
        public DateTime SigninTime { get; set; }

        public string SigninStyle { get; set; }

    }
}

