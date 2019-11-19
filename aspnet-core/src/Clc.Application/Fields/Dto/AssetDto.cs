using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Asset))]
    public class AssetDto : EntityDto
    {
        public int DepotId { get; set; }
        
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Asset.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Asset.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Required]
        [StringLength(Asset.MaxCategoryLength)]
        public string Category { get; set; }

        /// <summary>
        /// 存放地
        /// </summary>
        [Required]
        [StringLength(Asset.MaxAddressLength)]
        public string Address { get; set; }

        /// <summary>
        /// 保管责任人
        /// </summary>
        [StringLength(Asset.MaxAddressLength)]
        public string ChargePerson { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime UseDate { get; set; }

        /// <summary>
        /// 预计报废日期
        /// </summary>
        public DateTime RetireDate { get; set; }

        [StringLength(Asset.MaxRemarkLength)]
        public string Remark { get; set; }

    }
}

