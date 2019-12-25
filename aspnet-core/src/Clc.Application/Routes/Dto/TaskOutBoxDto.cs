using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes.Dto
{
    /// <summary>
    /// TaskInBoxDto
    /// </summary>
    [AutoMap(typeof(RouteInBox))]
    public class TaskOutBoxDto : EntityDto
    {
        [Required]
        public int RouteId { get; set; }

        public string BoxRecordBoxCn { get; set; }
        public string BoxRecordBoxOutletName { get; set; }

        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteTaskId { get; set; }

        [Required]
        public int BoxRecordId { get; set; }

        public DateTime? BoxRecordOutTime { get; set; }
        public DateTime? BoxRecordReceiveTime { get; set; }
    }
}

