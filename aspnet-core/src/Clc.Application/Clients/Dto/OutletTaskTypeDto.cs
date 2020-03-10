using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Clients;

namespace Clc.Clients.Dto
{
    [AutoMap(typeof(OutletTaskType))]
    public class OutletTaskTypeDto : EntityDto
    {
        [Required]
        public int OutletId { get; set; }
        
        [Required]
        public int TaskTypeId { get; set; }
        
        /// <summary>
        /// 大队
        /// </summary>
        public int? DepotId { get; set; }

        [Required]
        public float Price { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(OutletTaskType.MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

