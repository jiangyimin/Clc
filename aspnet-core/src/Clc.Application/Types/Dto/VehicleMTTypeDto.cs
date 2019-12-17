using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(VehicleMTType))]
    public class VehicleMTTypeDto : EntityDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(VehicleMTType.MaxCnLength)]
        public string Cn { get; set; }
        
        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [StringLength(VehicleMTType.MaxNameLength)]
        public string Name { get; set; }
    }
}

