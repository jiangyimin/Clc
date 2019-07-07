using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(WorkRole))]
    public class WorkRoleDto : EntityDto
    {
        [Required]
        [StringLength(WorkRole.MaxNameLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(WorkRole.MaxNameLength)]
        public string Name { get; set; } 

        public int DefaultWorkerTypeId { get; set; }

        [Required]
        [StringLength(WorkRole.MaxCategoryLength)]
        public string Category { get; set; }   

        [Required]
        [StringLength(WorkRole.MaxDutiesLength)]
        public string Duties { get; set; }
    }
}