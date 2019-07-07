using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(TaskType))]
    public class TaskTypeDto : EntityDto
    {
        [Required]
        [StringLength(TaskType.MaxNameLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(TaskType.MaxNameLength)]
        public string Name { get; set; }       
    }
}