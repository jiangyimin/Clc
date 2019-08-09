using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Clients;

namespace Clc.Clients.Dto
{
    [AutoMap(typeof(Customer))]
    public class CustomerDto : EntityDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Customer.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Customer.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Rfid 
        /// </summary>
        [StringLength(Customer.MaxContactLength)]
        public string Contact { get; set; }
    }
}

