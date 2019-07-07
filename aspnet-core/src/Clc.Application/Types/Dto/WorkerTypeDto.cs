using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(WorkerType))]
    public class WorkerTypeDto : EntityDto
    {
        [Required]
        [StringLength(WorkerType.MaxNameLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(WorkerType.MaxNameLength)]
        public string Name { get; set; }        
    }
}