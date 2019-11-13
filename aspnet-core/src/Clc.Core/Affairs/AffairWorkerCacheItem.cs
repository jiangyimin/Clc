using System;
using Abp.AutoMapper;

namespace Clc.Affairs
{
    /// <summary>
    /// AffairWorker Entity
    /// </summary>
    [AutoMapFrom(typeof(AffairWorker))]
    public class AffairWorkerCacheItem
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }

        public int WorkRoleId { get; set; }

        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }

        public DateTime? LastAskDoor { get; set; }

    }
}

