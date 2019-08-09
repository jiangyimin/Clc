using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Types;
using Clc.Fields;

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
        [StringLength(Workplace.MaxWorkRolesLength)]
        public string WorkRoles { get; set; }
        
        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(Workplace.ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 共享运行中心列表
        /// </summary>
        [StringLength(Workplace.ShareDepotListLength)]
        public string ShareDepotList { get; set; }
        
        public int MinDuration { get; set; }     
        /// <summary>
        /// 每班最长时长
        /// </summary>
        public int MaxDuration { get; set; }     
        /// <summary>
        /// 是否云端开门
        public string HasCloudDoor { get; set; }
    }
}

