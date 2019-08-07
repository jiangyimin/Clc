using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Works.Entities;

namespace Clc.Affairs
{
    /// <summary>
    /// AffairEnventDto
    /// </summary>
    [AutoMap(typeof(AffairEvent))]
    public class AffairEventDto : EntityDto
    {
        public int AffairId { get; set; }
        
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime EventTime { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Issurer { get; set; }
    }
}

