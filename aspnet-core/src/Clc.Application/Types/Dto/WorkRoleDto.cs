using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(WorkRole))]
    public class WorkRoleDto : EntityDto
    {
        [Required]
        [StringLength(WorkRole.MaxNameLength)]
        public string Name { get; set; } 

        public int? DefaultPostId { get; set; }

        [StringLength(WorkRole.MaxDutiesLength)]
        public string Duties { get; set; }

        /// <summary>
        /// 必须安排
        /// </summary>
        public bool mustHave { get; set; }
        
        /// <summary>
        /// 最多人数
        /// </summary>
    }
}