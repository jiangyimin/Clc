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
    public class TaskInBoxDto : EntityDto
    {
        public int RouteId { get; set; }

        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteTaskId { get; set; }

        [Required]
        public int BoxRecordId { get; set; }

        public string BoxRecordBoxCn { get; set; }
        public string BoxRecordBoxOutletName { get; set; }
        public DateTime? BoxRecordPickupTime { get; set; }

    }
}

