using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Clc.Fields.Entities;
using Microsoft.AspNetCore.Http;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Vehicle))]
    public class VehicleDto : EntityDto, ICustomValidate, IShouldNormalize
    {
        private const int MaxPhotoLength = AppConsts.MaxPhotoLength;

        [Required]
        public int DepotId { get; set; }
        
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Vehicle.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 车牌
        /// </summary>
        [Required]
        [StringLength(Vehicle.LicenseLength)]
        public string License { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public byte[] Photo { get; set; }

        public IFormFile PhotoFile { get; set; }

        #region interface
        public void AddValidationErrors(CustomValidationContext context)
        {
            if (PhotoFile != null && PhotoFile.Length > MaxPhotoLength)
                context.Results.Add(new ValidationResult("照片文件不能大于10K!"));
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