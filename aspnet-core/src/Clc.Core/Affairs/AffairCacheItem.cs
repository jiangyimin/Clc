using System.Collections.Generic;
using Abp.AutoMapper;

namespace Clc.Affairs
{
    /// <summary>
    /// Affair Entity
    /// </summary>
    [AutoMapFrom(typeof(Affair))]
    public class AffairCacheItem
    {
        /// <summary>
        /// Id Of Affair
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Workplace
        /// </summary>
        public int WorkplaceId { get; set; }
        // public Workplace Workplace { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        // public bool IsTomorrow { get; set; }

        public List<AffairWorkerCacheItem> Workers { get; set; }
        public List<AffairTaskCacheItem> Tasks { get; set; }

    }
}

