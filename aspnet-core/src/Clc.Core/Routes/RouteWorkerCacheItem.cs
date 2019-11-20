using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes
{
    /// <summary>
    /// RouteCacheItem
    /// </summary>
    [AutoMapFrom(typeof(RouteWorker))]
    public class RouteWorkerCacheItem : EntityDto
    {
        public int WorkerId { get; set; }
        
        public int? AltWorkerId { get; set; }

        public int WorkRoleId { get; set; }
    }
}

