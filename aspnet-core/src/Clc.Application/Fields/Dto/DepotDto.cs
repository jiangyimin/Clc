using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Depot))]
    public class DepotDto : EntityDto
    {
        [Required]
        [StringLength(Depot.MaxCnLength)]
        public string Cn { get; set; }
        [Required]
        [StringLength(Depot.MaxNameLength)]
        public string Name { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public int? Radius { get; set; }
        
        public string ActiveRouteNeedCheckin { get; set; }
        
        [StringLength(Depot.MaxPasswordLength)]
        public string UnlockScreenPassword { get; set; }

        public string ReportTo { get; set; }
    }
}