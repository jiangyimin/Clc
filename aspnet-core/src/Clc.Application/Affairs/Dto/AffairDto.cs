using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using Clc.Works.Entities;

namespace Clc.Affairs.Dto
{
    /// <summary>
    /// AffairDto
    /// </summary>
    [AutoMap(typeof(Affair))]
    public class AffairDto : EntityDto, IShouldNormalize, ICustomValidate
    {
        public int DepotId { get; set; }

        public DateTime CarryoutDate { get; set; }

        /// <summary>
        /// Workplace
        /// </summary>
        // [Required]
        public int WorkplaceId { get; set; }

        /// <summary>
        /// 状态（生成，活动，结束, 日结）
        /// </summary>
        [StringLength(2)]
        public string Status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        [StringLength(ClcConsts.TimeLength)]
        public string EndTime { get; set; }
        public string IsTomorrow { get; set; }

        [StringLength(Affair.MaxRemarkLength)]
        public string Remark { get; set; }

        public string WorkplaceName { get; set; }
        public string CreateWorkerName { get; set; }

        #region Inteface
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Status)) 
                Status = "安排";
        }       

        public void AddValidationErrors(CustomValidationContext context)
        {
            // WorkManager workManager = IocManager.Instance.Resolve<DomainManager>();

            // VaultType vaultType = domainManager.GetVaultType(VaultTypeId);
            
            // if (string.Compare(vaultType.EarliestTime, StartTime) > 0)
            //     context.Results.Add(new ValidationResult("开始时间不能早于金库操作类型的最早时间!"));

            // if (string.Compare(vaultType.LatestTime, EndTime) < 0)
            //     context.Results.Add(new ValidationResult("结束时间不能晚于金库操作类型的最晚时间!"));

            // if (string.Compare(StartTime, EndTime) >= 0)
            //     context.Results.Add(new ValidationResult("结束时间不能小于开始时间!"));
        }

        #endregion      

    }
}

