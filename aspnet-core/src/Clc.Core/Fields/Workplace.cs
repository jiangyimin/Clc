using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Runtime;

namespace Clc.Fields
{
    /// <summary>
    /// Workplace Entity
    /// </summary>
    [Description("任务地点")]
    public class Workplace : Entity, IMustHaveTenant
    {
        public const int MaxNameLength = 8;
        public const int MaxWorkRolesLength = ClcConsts.NormalStringLength;
         public const int ArticleTypeListLength = ClcConsts.NormalStringLength;
        public const int ShareDepotListLength = ClcConsts.NormalStringLength;
        public const int IpAddressLength = 20;
        public const int AskOpenStyleLength = 20;
        public const int MaxPasswordLength = 8;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        [Required]
        public int DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        /// <summary>
        /// 地点名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// 工作角色列表
        /// </summary>
        [Required]
        [StringLength(MaxWorkRolesLength)]
        public string WorkRoles { get; set; }
        
        /// <summary>
        /// 可领用物品列表
        /// </summary>
        [StringLength(ArticleTypeListLength)]
        public string ArticleTypeList { get; set; }

        /// <summary>
        /// 共享运行中心列表
        /// </summary>
        [StringLength(ShareDepotListLength)]
        public string ShareDepotList { get; set; }

        /// <summary>
        /// 云端开门
        /// </summary>
        [StringLength(Workplace.IpAddressLength)]

        public string DoorIp { get; set; }
        [StringLength(Workplace.IpAddressLength)]
        public string CameraIp { get; set; }

        // Rules

        /// <summary>
        /// 每班最短时长
        /// </summary>
        public int MinDuration { get; set; }     

        /// <summary>
        /// 每班最长时长
        /// </summary>
        public int MaxDuration { get; set; }   

        /// <summary>
        /// 申请开门提前时间（分钟）
        /// </summary>
        public int AskOpenLead { get; set; }

        /// <summary>
        /// 申请开门截止时间（分钟）
        /// </summary>
        public int AskOpenDeadline { get; set; }


        /// <summary>
        /// 开门方式：1）直接申请 2）验证后申请 3）领抢任务申请
        /// </summary>
        [StringLength(AskOpenStyleLength)]
        public string AskOpenStyle { get; set; }
        
        /// <summary>
        ///  紧急开门密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string EmergPassword { get; set; }
        #region methods

        public string CheckTimeZone(string startTime, string endTime)
        {
            DateTime start = ClcUtils.GetDateTime(startTime);
            DateTime end = ClcUtils.GetDateTime(endTime);
            if (start > end)
                return "结束时间不能小于开始时间!";
            if (end.Subtract(start).TotalHours < MinDuration)
                return string.Format("时段长度小于规定({0}小时)", MinDuration);
            if (end.Subtract(start).TotalHours > MaxDuration)
                return string.Format("时段长度大于规定({0}小时)", MaxDuration);

            return null;
        }
        #endregion
    }
}

