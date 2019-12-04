using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes
{
    /// <summary>
    /// RouteCacheItem
    /// </summary>
    [AutoMapFrom(typeof(Route))]
    public class RouteCacheItem : EntityDto
    {
        public string RouteName { get; set; }
        public int DepotId { get; set; }

        public string Status { get; set; }

        public int RouteTypeId { get; set; }

        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }

        public int? AltVehicleId { get; set; }
        public string AltVehicleCn { get; set; }
        public string AltVehicleLicense { get; set; }
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Remark { get; set; }

        public List<RouteWorkerCacheItem> Workers { get; set; }

        // public List<RouteTaskCacheItem> Tasks { get; set; }
    }
}

