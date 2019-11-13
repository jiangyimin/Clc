using System;
using Abp.AutoMapper;

namespace Clc.Affairs
{
    /// <summary>
    /// AffairWorker Entity
    /// </summary>
    [AutoMapFrom(typeof(AffairTask))]
    public class AffairTaskCacheItem
    {
        public int Id { get; set; }
        public int WorkplaceId { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Remark { get; set; }
    }
}

