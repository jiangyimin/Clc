using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(RouteType))]
    public class RouteTypeDto : EntityDto
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(RouteType.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 工作角色列表
        /// </summary>
        [Required]
        [StringLength(RouteType.MaxWorkRolesLength)]
        public string WorkRoles { get; set; }
        /// <summary>
        /// 签到提前时间（分钟）
        /// </summary>
        public int CheckinLead { get; set; }

        /// <summary>
        /// 领物关闭时间（分钟）
        /// </summary>
        public int LendArticleDeadline { get; set; }
    }
}