using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes.Dto
{
    /// <summary>
    /// RouteInBoxDto
    /// </summary>
    [AutoMap(typeof(RouteOutBox))]
    public class RouteOutBoxDto : EntityDto
    {
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// Box
        /// </summary>
        [Required]
        public int BoxId { get; set; }
        public string BoxCn { get; set; }

        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteTaskId { get; set; }

        [Required]
        public int BoxRecordId { get; set; }

        // only for mds.js 
        public string Postfix { get; } = "OutBox";
    }
}

