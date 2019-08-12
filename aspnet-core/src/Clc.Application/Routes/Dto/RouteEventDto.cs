using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes
{
    /// <summary>
    /// RouteEnventDto
    /// </summary>
    [AutoMap(typeof(RouteEvent))]
    public class RouteEventDto : EntityDto
    {
        public int RouteId { get; set; }
        
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime EventTime { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Issurer { get; set; }

        // only for mds.js 
        public string Postfix { get; } = "Event";
    }
}

