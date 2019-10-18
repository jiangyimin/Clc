using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Clc.Authorization.Roles;
using Clc.Fields;
using Microsoft.AspNetCore.Http;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(WorkerFile))]
    public class WorkerFileDto : EntityDto
    {
        public int WorkerId { get; set; }
        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }
        
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(WorkerFile.MaxFileCnLength)]
        public string FileCn { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime Hiredate { get; set; }

        /// <summary>
        /// 姓别(男女)
        /// </summary>
        [Required]
        [StringLength(WorkerFile.MaxSelectLength)]
        public string Sex { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 身份有效期
        /// </summary>
        public DateTime? EndValidity { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [StringLength(WorkerFile.MaxEthnicityLength)]
        public string Ethnicity { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        [StringLength(WorkerFile.MaxNativeplaceLength)]
        public string Nativeplace { get; set; }

        /// <summary>
        /// 户籍地址
        /// </summary>
        [StringLength(WorkerFile.MaxAddressLength)]
        public string ResidenceAddress { get; set; }

        /// <summary>
        /// 户籍派出所
        /// </summary>
        [StringLength(WorkerFile.MaxSelectLength)]
        public string PoliceStation { get; set; }

        /// <summary>
        /// 现住地址
        /// </summary>
        [StringLength(WorkerFile.MaxAddressLength)]
        public string Address { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        [Required]
        [StringLength(WorkerFile.MaxSelectLength)]
        public string PoliticalStatus { get; set; }

        /// <summary>
        /// 文化程度
        /// </summary>
        [Required]
        [StringLength(WorkerFile.MaxSelectLength)]
        public string Education { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string School { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public int Stature { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        [StringLength(WorkerFile.MaxSelectLength)]
        public string MaritalStatus { get; set; }
        
        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string Contact { get; set; }
        
        /// <summary>
        /// 驾驶证类型
        /// </summary>
        [StringLength(WorkerFile.MaxSelectLength)]
        public string LicenseType { get; set; }

        /// <summary>
        /// 介绍人信息
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string Introductory { get; set; }
         
        /// <summary>
        /// 购买保险
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string Insurance { get; set; }
         
        /// <summary>
        /// 上岗证编号
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string WorkLicenseCn { get; set; }
         
        /// <summary>
        /// 保安资格证编号
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string CertificateCn { get; set; }

        /// <summary>
        /// 持枪证编号
        /// </summary>
        [StringLength(ClcConsts.NormalStringLength)]
        public string ArmLicenceCn { get; set; }
         
        // 各种记录
        public string CeteficationRecord { get; set; }
        public string JobChangeRecord { get; set; }
        public string MobilityRecord { get; set; }
        public string TrainingRecord { get; set; }
        public string RewardPunishRecord { get; set; }

        /// <summary>
        /// 是否在职
        /// </summary>
        [Required]
        [StringLength(WorkerFile.MaxSelectLength)]
        public string Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 离职档案
        /// </summary>
        [StringLength(WorkerFile.MaxFileCnLength)]
        public string QuitFileCn { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime? Quitdate { get; set; }
    }
}