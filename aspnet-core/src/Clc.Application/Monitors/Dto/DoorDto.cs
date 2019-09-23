using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields;

namespace Clc.Monitors.Dto
{
    [AutoMap(typeof(Workplace))]
    public class DoorDto : EntityDto
    {
        [Required]
        public string DepotName { get; set; }

        /// <summary>
        /// 场所名称
        /// </summary>
        [Required]
        [StringLength(Workplace.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 云端开门
        [StringLength(Workplace.IpAddressLength)]
        public string DoorIp { get; set; }
        [StringLength(Workplace.IpAddressLength)]
        public string CameraIp { get; set; }
    }
}

