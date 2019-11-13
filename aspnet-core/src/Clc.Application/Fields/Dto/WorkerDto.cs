using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Clc.Authorization.Roles;
using Clc.Fields;
using Microsoft.AspNetCore.Http;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Worker))]
    public class WorkerDto : EntityDto, ICustomValidate, IShouldNormalize
    {
        private const int MaxPhotoLength = AppConsts.MaxPhotoLength;

        [Required]
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
        /// 岗位
        /// </summary>
        [Required]
        public int PostId { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [StringLength(Worker.MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 登录角色列表
        /// </summary>
        [StringLength(Worker.MaxRoleNamesLength)]        
        public string LoginRoleNames { get; set; }

        /// <summary>
        /// 可担任角色列表
        /// </summary>
        [StringLength(Worker.MaxRoleNamesLength)]
        public string WorkRoleNames { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }

        public int PhotoLength { get; set; }
        public IFormFile PhotoFile { get; set; }

        /// <summary>
        /// Rfid 
        /// </summary>
        [StringLength(Worker.MaxRfidLength)]
        public string Rfid { get; set; }

        /// <summary>
        /// 微信设备Id
        /// </summary>
        [StringLength(Worker.MaxDeviceIdLength)]
        public string DeviceId { get; set; }

        /// <summary>
        /// 附加认证信息
        /// </summary>
        [StringLength(Worker.MaxAdditiveInfoLength)]
        public string AdditiveInfo { get; set; }

        public string IsActive { get; set; }

        public string Finger { get; set; }

        public string Finger2 { get; set; }
        
        #region interface
        public void AddValidationErrors(CustomValidationContext context)
        {
            if (PhotoFile != null && PhotoFile.Length > MaxPhotoLength)
                context.Results.Add(new ValidationResult("照片文件不能大于50K!"));
        }

        public void Normalize()
        {
            if (PhotoFile != null) 
            {
                Photo = new byte[PhotoFile.Length];
                PhotoFile.OpenReadStream().Read(Photo, 0, (int)PhotoFile.Length);
            }
        }

        #endregion
    }
}