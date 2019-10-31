using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Types;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(WorkRole))]
    public class WorkRoleDto : EntityDto
    {
        /// <summary>
        /// 编号/序号
        /// </summary>
        [Required]
        [StringLength(WorkRole.MaxCnLength)]
        public string Cn { get; set; }
        [Required]
        [StringLength(WorkRole.MaxNameLength)]
        public string Name { get; set; } 
        
        [StringLength(WorkRole.MaxArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        [StringLength(WorkRole.MaxDutiesLength)]
        public string Duties { get; set; }

        /// <summary>
        /// 必须安排
        /// </summary>
        public string MustHave { get; set; }
        
        /// <summary>
        /// 最多人数
        /// </summary>
        public int MaxNum { get; set; }
    }
}