using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Authorization.Users;
using Clc.Types.Entities;
using Clc.Fields.Entities;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Workplace))]
    public class WorkplaceDto : EntityDto
    {
        [Required]
        public int DepotId { get; set; }

        /// <summary>
        /// 场所名称
        /// </summary>
        [Required]
        [StringLength(Workplace.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public int AffairTypeId { get; set; }
        public AffairType AffairType { get; set; }
        
        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(Workplace.ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 角色用户名
        /// </summary>
        [StringLength(User.MaxNameLength)]
        public string RoleUserName { get; set; }
    }
}

