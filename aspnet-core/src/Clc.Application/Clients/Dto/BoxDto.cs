using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Clients;

namespace Clc.Clients.Dto
{
    [AutoMap(typeof(Box))]
    public class BoxDto : EntityDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Box.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Box.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 外键，网点
        /// </summary>
        [Required]
        public int OutletId { get; set; }
        
        /// <summary>
        /// Rfid 
        /// </summary>
        [StringLength(Box.MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

