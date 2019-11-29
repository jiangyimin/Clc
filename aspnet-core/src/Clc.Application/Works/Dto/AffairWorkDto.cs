using System;
using System.Collections.Generic;
using Clc.Affairs;

namespace Clc.Works.Dto
{
    /// <summary>
    /// MyAffairWork Model
    /// </summary>
    public class AffairWorkDto
    {
        public bool AltCheck { get; set; }
        public string Today { get; set; }
        public int DepotId { get; set; }
        public int AffairId { get; set; }

        public string Content { get; set; }
        public int WorkplaceId { get; set; }
        public string WorkplaceName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Workers { get; set; }

        public AffairWorkDto SetAffair(AffairCacheItem affair, string wpName, bool altCheck)
        {
            AltCheck = altCheck;
            Today = DateTime.Now.ToString("yyyy-MM-dd");
            if (affair != null)
            {
                AffairId = affair.Id;
                Content = affair.Content;
                WorkplaceId = affair.WorkplaceId;
                WorkplaceName = wpName;
                StartTime = affair.StartTime;
                EndTime = affair.EndTime;
            }
            return this;
        }
    }
}

