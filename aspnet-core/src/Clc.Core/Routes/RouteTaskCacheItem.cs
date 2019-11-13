using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes
{
    /// <summary>
    /// RouteTaskCacheItem
    /// </summary>
    [AutoMapFrom(typeof(RouteTask))]
    public class RouteTaskCacheItem : EntityDto
    {
        public int OutletId { get; set; }

        public int TaskTypeId { get; set; }        
    }
}

