using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Authorization.Roles;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(Post))]
    public class PostDto : EntityDto
    {
        [Required]
        [StringLength(Post.MaxNameLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(Post.MaxNameLength)]
        public string Name { get; set; }       

        [StringLength(Role.MaxNameLength)]
        public string WorkerRoleName { get; set; }
    }
}