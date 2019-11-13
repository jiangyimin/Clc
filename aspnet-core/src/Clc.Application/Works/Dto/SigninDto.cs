using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields;
using Clc.Runtime;

namespace Clc.Works.Dto
{
    /// <summary>
    /// SigninDto Entity
    /// </summary>
    [AutoMapFrom(typeof(Signin))]
    public class SigninDto : EntityDto
    {        
        public const int MaxWorkerLength = 20;
        
        // 签到地点
        [Required]
        public int DepotId { get; set; }

        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        [Required]
        public int WorkerId { get; set; }
        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }
        

        // 签到时间
        public DateTime SigninTime { get; set; }


        // 签到方式
        public string SigninStyle { get; set; }

    }
}

