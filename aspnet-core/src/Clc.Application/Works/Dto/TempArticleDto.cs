using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Affairs;

namespace Clc.Works.Dto
{
    /// <summary>
    /// AffairEnventDto
    /// </summary>
    [AutoMap(typeof(AffairEvent))]
    public class TempArticleDto : EntityDto
    {
        public int AffairId { get; set; }
        
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime EventTime { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Issurer { get; set; }

        public DateTime? TakeTime { get; set; }

    }
}

