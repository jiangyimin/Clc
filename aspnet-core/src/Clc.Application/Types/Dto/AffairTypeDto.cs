using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Types.Entities;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(AffairType))]
    public class AffairTypeDto : EntityDto
    {
        [Required]
        [StringLength(AffairType.MaxNameLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(AffairType.MaxNameLength)]
        public string Name { get; set; }    
           
        public string hasCloudDoor { get; set; }

        public string toRoute { get; set; }

        /// <summary>
        /// 每班最短时长(小时)
        /// </summary>
        public int MinDuration { get; set; }

        /// <summary>
        /// 每班最长时长(小时）
        /// </summary>
        public int MaxDuration { get; set; }     
    }
}