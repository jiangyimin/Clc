using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Types;

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

        public int MinDuration { get; set; }     
        /// <summary>
        /// 每班最长时长
        /// </summary>
        public int MaxDuration { get; set; }     
        /// <summary>
        /// 是否云端开门
        public bool HasCloudDoor { get; set; }

        #region methods

        public string CheckTimeZone(string startTime, string endTime, bool isTomorrow)
        {
            DateTime today = DateTime.Now;
            DateTime tomorrow = today.AddDays(1);
            DateTime start = new DateTime(today.Year, today.Month, today.Day, int.Parse(startTime.Substring(0, 2)), int.Parse(startTime.Substring(3, 2)), 0);
            DateTime end = isTomorrow ? new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, int.Parse(endTime.Substring(0, 2)), int.Parse(endTime.Substring(3, 2)), 0)
                                        : new DateTime(today.Year, today.Month, today.Day, int.Parse(endTime.Substring(0, 2)), int.Parse(endTime.Substring(3, 2)), 0);
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

