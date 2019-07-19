using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Fields
{
    [AutoMap(typeof(Warehouse))]
    public class WarehouseDto : EntityDto
    {
        public int DepotId { get; set; }

        /// <summary>
        /// 库房名称
        /// </summary>
        [Required]
        [StringLength(Warehouse.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(Warehouse.ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 每日班次民称列表
        /// </summary>
        [StringLength(Warehouse.ShiftNameListLength)]
        public string ShiftNameList { get; set; }

        /// <summary>
        /// 每班最短时长
        /// </summary>
        public int MinDuration { get; set; }     
        /// <summary>
        /// 每班最长时长
        /// </summary>
        public int MaxDuration { get; set; }     
    }
}

