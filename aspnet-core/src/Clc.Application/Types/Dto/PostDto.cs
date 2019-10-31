using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Authorization.Roles;
using Clc.Types;

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

        [StringLength(Post.MaxNameLength)]
        public string DefaultWorkRoleName { get; set; }       

        /// <summary>
        /// 企业微信应用号
        /// </summary>
        [StringLength(Post.MaxNameLength)]
        public string AppName { get; set; }  
          
        [StringLength(Role.MaxNameLength)]
        public string WorkerRoleName { get; set; }
    }
}