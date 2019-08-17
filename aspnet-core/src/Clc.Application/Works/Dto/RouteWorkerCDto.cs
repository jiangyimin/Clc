using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Routes;

namespace Clc.Works.Dto
{
    /// <summary>
    /// ArtRouteWorkerDto
    /// </summary>
    public class RouteWorkerCDto
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }

        public int WorkRoleId { get; set; }
    }
}

