using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Routes;

namespace Clc.Works.Dto
{
    /// <summary>
    /// RouteDto
    /// </summary>
    [AutoMapFrom(typeof(Route))]
    public class RouteCDto : EntityDto
    {
        [Required]
        [StringLength(Route.MaxNameLength)]
        public string RouteName { get; set; }

        public string Status { get; set; }

        public int RouteTypeId { get; set; }

        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Remark { get; set; }

        public List<RouteWorkerCDto> Workers { get; set; }
        public List<RouteArticleCDto> Articles { get; set; }

    }
}

