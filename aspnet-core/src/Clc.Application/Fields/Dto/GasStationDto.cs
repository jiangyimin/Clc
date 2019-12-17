using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Clc.Fields;
using Microsoft.AspNetCore.Http;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(GasStation))]
    public class GasStationDto : EntityDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(GasStation.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(GasStation.MaxNameLength)]
        public string Name { get; set; }

        public string Remark { get; set; }

        public string DepotList { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }
    }
}