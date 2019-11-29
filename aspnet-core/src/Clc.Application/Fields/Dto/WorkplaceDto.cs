using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

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
        /// 云端开门
        [StringLength(Workplace.IpAddressLength)]
        public string DoorIp { get; set; }
        [StringLength(Workplace.IpAddressLength)]
        public string CameraIp { get; set; }

        /// <summary>
        /// 当班查询提前时间(分钟)
        /// </summary>
        public int DutyLead { get; set; }

        /// <summary>
        /// 申请开门截止时间（分钟）
        /// </summary>
        public int AskOpenDeadline { get; set; }

        /// <summary>
        /// 开门方式：1）直接申请 2）验证后申请 3）领抢任务申请
        /// </summary>
        [StringLength(Workplace.AskOpenStyleLength)]
        public string AskOpenStyle { get; set; }        
    }
}

