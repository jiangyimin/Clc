using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields.Entities;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Depot))]
    public class DepotDto : EntityDto
    {
        [Required]
        public string Cn { get; set; }
        [Required]
        public string Name { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public int? Radius { get; set; }
        public string activeRouteNeedCheckin { get; set; }
        public int? RelyDepotId { get; set; }
    }
}