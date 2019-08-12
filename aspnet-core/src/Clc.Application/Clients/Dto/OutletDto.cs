using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Clients;

namespace Clc.Clients.Dto
{
    [AutoMap(typeof(Outlet))]
    public class OutletDto : EntityDto
    {
        // 客户外键
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Outlet.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Outlet.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 交接密码
        /// </summary>
        [StringLength(Outlet.MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 交接密文
        /// </summary>
        [StringLength(Outlet.MaxPasswordLength)]
        public string Ciphertext { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        [StringLength(Outlet.MaxContactLength)]
        public string Contact { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        [StringLength(Outlet.MaxContactLength)]
        public string Weixins { get; set; }
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

