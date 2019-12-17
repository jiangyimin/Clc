using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Clients;

namespace Clc.Clients.Dto
{
    [AutoMap(typeof(CustomerTaskType))]
    public class CustomerTaskTypeDto : EntityDto
    {
        [Required]
        public int CustomerId { get; set; }
        
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
        [StringLength(CustomerTaskType.MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

