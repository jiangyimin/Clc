using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Worker))]
    public class WorkerDto : EntityDto
    {
        public int DepotId { get; set; }
        
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Worker.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(Worker.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [StringLength(Worker.MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// 指纹
        /// </summary>
        [StringLength(Worker.FingerLength)]
        public string Finger { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [StringLength(Worker.IDNumberLength)]
        public string IDNumber { get; set; }

        /// <summary>
        /// IDCardNo 
        /// </summary>
        [StringLength(Worker.IDCardNoMaxLength)]
        public string IDCardNo { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(Worker.MobileLength)]
        public string Mobile { get; set; }

        /// <summary>
        /// 微信设备Id
        /// </summary>
        [StringLength(Worker.MaxDeviceId)]
        public string DeviceId { get; set; }

        public string IsActive { get; set; }
    }
}